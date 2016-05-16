using System;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

// In order to to allow DLL functions to be exported, Robert Giesecke's Unmanaged 
// Exports solution is used. https://nuget.org/packages/UnmanagedExports

namespace XTensions
{
    /// <summary>
    /// A group of functions that step through XWF's use of the X-Tension.
    /// </summary>
    public class ExportedMethods
    {
        // Toggle debugging.
        private static Boolean DEBUGGING = false;
        
        // The volume handle and action type are used across multiple functions.
        private static IntPtr _currentVolumeHandle;
        private static XTensionActionSource? _currentAction = null;

        /// <summary>
        /// Mandatory to export. Will be called before anything else in order to inform 
        /// the DLL of the version of XWF that is loading the DLL, already when the 
        /// X-Tension is selected by the user in X-Ways Forensics.
        /// </summary>
        /// <param name="nVersion">The higher word specifies the version number. For 
        /// example 1640 means v16.4. The third highest byte specified the service 
        /// release number. The lowest byte specifies the current language of the user 
        /// interface of the calling program.</param>
        /// <param name="nFlags">Information about the calling program. See XTInitFlags.
        /// </param>
        /// <param name="hMainWnd">A pointer to the main window.</param>
        /// <param name="lpReserved">Unknown.</param>
        /// <returns>You may return -1 to prevent further use of the DLL. For example, 
        /// if certain functions that you require are not present (i.e. not exported by 
        /// a certain version of XWF). Otherwise you must return 1, optionally combined
        /// </returns>
        [DllExport]
        public static int XT_Init(CallerInformation nVersion
            , CallingProgramCapabilities nFlags, IntPtr hMainWnd, IntPtr lpReserved)
        {

            // Check the release capabilites. Not necessary for our purposes since we
            // control the releases of XWF used.
            //if ((nFlags & XTInitFlags.XT_INIT_XWF) > 0)
            //{
            //    // Called by XWF
            //}
            //
            //if ((nFlags & XTInitFlags.XT_INIT_BETA) > 0)
            //{
            //    // Called by a Beta version of the application
            //}
            //
            //if ((nFlags & XTInitFlags.XT_INIT_QUICKCHECK) > 0)
            //{
            //    // Called to check if the API accepts the calling application
            //}

            // If importing functions fails, we return -1 to prevent further use of the 
            // X-Tension.
            if (!ImportedMethods.Import()) return -1;

            if (DEBUGGING)
            {
                HelperMethods.XWF_OutputMessage(String.Format(
                    "Version: {0}, Service Release: {1}, Language: {2}, nFlags: {3}, "
                  + "Main Window Pointer: {4}"
                  , nVersion.Version, nVersion.ServiceRelease, nVersion.Language, nFlags
                  , hMainWnd));
            }

            // Return Value 1: Continue using X-Tension.
            return 1;
        }

        /// <summary>
        /// If exported (optional), will be called just before the DLL is unloaded to 
        /// give you a chance to dispose any allocated memory, save certain data 
        /// permanently etc.
        /// </summary>
        /// <param name="lpReserved">Unknown. Currently always NULL.</param>
        /// <returns>Should always return 0.</returns>
        [DllExport]
        public static int XT_Done(IntPtr lpReserved)
        {
            return 0;
        }

        /// <summary>
        /// If exported (optional), will be called when the user requests to see 
        /// information about the DLL.You can display copyright notices, a version 
        /// number, a brief description of the exported functionality, extensive help on 
        /// how to use it, from where in X-Ways Forensics to call it with what settings 
        /// etc. You could even display a dialog window where the user can change 
        /// settings for this X-Tension, which you store in the Windows registry or in a 
        /// permanent file.
        /// </summary>
        /// <param name="hParentWnd">A pointer to the parent window.</param>
        /// <param name="lpReserved">Unknown. Currently always NULL.</param>
        /// <returns>Should always return 0.</returns>
        [DllExport]
        public static int XT_About(IntPtr hParentWnd, IntPtr lpReserved)
        {
            HelperMethods.XWF_OutputMessage(
                "X-Tension Template: Describe your extension here.");

            return 0;
        }

        /// <summary>
        /// If exported (optional), will be called immediately for a volume when volume 
        /// snapshot refinement or some other action starts before items or search hits 
        /// in that volume are processed individually.
        /// </summary>
        /// <param name="hVolume">A pointer to the current volume.</param>
        /// <param name="hEvidence">A pointer to an evidence object if hVolume 
        /// represents evidence, otherwise 0.</param>
        /// <param name="nOpType">How the X-Tension is executed within X-Ways Forensics,
        /// i.e. from the main menu, volume snapshot refinement, simultaneous search,
        /// etc.</param>
        /// <param name="lpReserved">Unknown.</param>
        /// <returns>Possible negative return values consist of the following (
        /// Full negative return value evaluation only for XT_ACTION_RVS): 
        /// -4 : Indicates that XWF should stop the whole operation (e.g. volume 
        ///      snapshot refinement) altogether.
        /// -3 : Indicates that the X-Tension shouldn't be used for the remainder of the 
        ///      whole operation, for example because your X-Tension is not supposed to 
        ///      do anything for that kind of operation as indicated by nOpType.
        /// -2 : Exclude the particular volume from the operation.
        /// -1 : Prevent any other functions of this X-Tension to be called for this 
        ///      particular volume, not even XT_Finalize.
        /// 
        /// Positive return values/combination of flags (Can be combined):
        ///  0 : Default; if you just want XT_Finalize to be called. Will also be
        ///      assumed if you do not export XT_Prepare.
        ///  1 : If you want X-Ways Forensics to call your implementation of
        ///      XT_ProcessItem[Ex] (whichever is exported) for each item of this volume
        ///      snapshot.
        ///  2 : In case of XT_ACTION_RVS, same, but to receive calls of XT_ProcessItem
        ///      (if exported) after all other individual item refinement operations 
        ///      instead of before.
        ///  4 : In case of XT_ACTION_RVS, to signal XWF that you may create more items 
        ///      in the volume snapshot, so that for example the user will definitely be 
        ///      informed of how many item were added (v16.5 and later only).</returns>
        [DllExport]
        public static int XT_Prepare(IntPtr hVolume, IntPtr hEvidence
            , XTensionActionSource nOpType, IntPtr lpReserved)
        {
            // Store the volume handle for use in ProcessItem[Ex].
            _currentVolumeHandle = hVolume;

            // Store the action type for use in ProcessItem[Ex].
            _currentAction = nOpType;

            // Run testing.  Eventually need to break this down.
            //Testing.CaseDetails.RunTest(hVolume, nOpType);
            //Testing.BlockManipulation.RunTest(hVolume, nOpType);
            //Testing.VolumeInteraction.RunTest(hVolume, nOpType);
            //Testing.SectorInteraction.RunTest(hVolume, nOpType);
            Testing.ItemInteraction.RunTest(hVolume, nOpType);

            // Return Value 2: In case of XT_ACTION_RVS, same, but to receive calls of 
            // XT_ProcessItem (if exported) after all other individual item refinement 
            // operations instead of before.
            return 2;
        }

        /// <summary>
        /// If exported (optional), will be called when volume snapshot refinement or 
        /// another operation has completed.
        /// </summary>
        /// <param name="hVolume">A pointer to the current volume.</param>
        /// <param name="hEvidence">A pointer to an evidence object if hVolume 
        /// represents evidence, otherwise 0.</param>
        /// <param name="nOpType">How the X-Tension is executed within X-Ways Forensics,
        /// i.e. from the main menu, volume snapshot refinement, simultaneous search,
        /// etc.</param>
        /// <param name="lpReserved">Unknown.</param>
        /// <returns>Return 1 if the current directory listing in the directory browser 
        /// of the active data window has to be refreshed after XT_ACTION_DBC (usually 
        /// not necessary, perhaps when adding new files to the directory, has an effect 
        /// in v17.6 and later only), or otherwise 0.</returns>
        [DllExport]
        public static int XT_Finalize(IntPtr hVolume, IntPtr hEvidence
            , XTensionActionSource nOpType, IntPtr lpReserved)
        {
            // Indicates that there is no current action executing.
            _currentAction = null;

            // Return Value 0: No directory listing refresh needed.
            return 0;
        }


        /// <summary>
        /// If exported (optional) and wanted by XT_Prepare, will be called for each 
        /// file in the volume snapshot that is targeted for refinement or selected 
        /// and targeted with the directory browser context menu. Prefered over 
        /// XT_ProcessItem if you merely need to retrieve information about each file 
        /// and don't need to also read their data. The files can still be read if 
        /// XWF_OpenItem is implemented but are not done by default.  Shouldn't be 
        /// exported/enabled at the same time as XT_ProcessItemEx.
        /// </summary>
        /// <param name="nItemID">The ID of the current item.</param>
        /// <param name="lpReserved">Unknown.</param>
        /// <returns>Return -1 if you want X-Ways Forensics to stop the current 
        /// operation (e.g. volume snapshot refinement), -2 if you want have XWF skip
        /// all other volume snapshot refinement operations for this file, otherwise 0.
        /// </returns>
        [DllExport]
        public static long XT_ProcessItem(int nItemID, IntPtr lpReserved)
        {
            HelperMethods.XWF_OutputMessage("Processing " + nItemID);
            // Store the item name, full path, type, and size for later use.
            string itemName = HelperMethods.XWF_GetItemName(nItemID);
            string itemPath = HelperMethods.GetFullPath(nItemID);
            ItemType itemType = HelperMethods.XWF_GetItemType(nItemID);
            long itemSize = HelperMethods.XWF_GetItemSize(nItemID);

            HelperMethods.XWF_OutputMessage("Item Name: " + itemName);
            //HelperMethods.XWF_OutputMessage("Item Path: " + itemPath);
            HelperMethods.XWF_OutputMessage("Item Type: " + itemType.Type);
            HelperMethods.XWF_OutputMessage("Item Size: " + itemSize);

            // Get a pointer to the current item's data.
            IntPtr hItem = HelperMethods.XWF_OpenItem(_currentVolumeHandle, nItemID
                , ItemOpenModes.LogicalContents);

            // Read the current item's data into a byte array.
            byte[] contents = HelperMethods.ReadItem(hItem);

            if (contents == null)
            {
                // Do something because we failed to read the data.
                HelperMethods.XWF_OutputMessage("Failed to read item contents");

                // 
                // Put logic here for handling failure to read file.
                //
            }
            else
            {
                // Do stuff with the data.
                HelperMethods.XWF_OutputMessage("Item contents read successfully.");

                // 
                // Put logic here for handling read file data.
                //
            }

            // Return Value 0: Continue using X-Tension.
            return 0;
        }

        /*
        /// <summary>
        /// If exported (optional) and wanted by XT_Prepare, will be called for each 
        /// item (file or directory) in the volume snapshot that is targeted for 
        /// refinement or selected and targeted with the directory browser context menu. 
        /// The item will be opened for reading prior to the function call. Implement 
        /// and export this function if you need to read the item's data, which you can 
        /// do using the hItem parameter. Shouldn't be exported/enabled at the same time 
        /// as XT_ProcessItem.</summary>
        /// <param name="nItemID">The ID of the current item.</param>
        /// <param name="hItem">A pointer to the item's data.</param>
        /// <param name="lpReserved">Unknown.</param>
        /// <returns>Per the API documentation, return -1 if you want X-Ways Forensics 
        /// to stop the current operation (e.g. volume snapshot refinement), otherwise 
        /// 0.</returns>
        [DllExport]
        public static int XT_ProcessItemEx(int nItemID, IntPtr hItem
            , IntPtr lpReserved)
        {
            //
            // Logic here.
            //

            // Return Value 0: Continue using X-Tension.
            return 0;
        }
        */

        /// <summary>
        /// If exported (optional), will be called by v16.9 and later if an X-Tension is 
        /// loaded for use with a simultaneous search, so that the X-Tension can enter 
        /// predefined search terms into the dialog window for use with the search. The 
        /// X-Tension can also learn about the current search settings (the active code 
        /// pages and some other settings through the flags field) and could inform the 
        /// user of necessary adjustments for the search to work as intended by the 
        /// X-Tension.
        /// </summary>
        /// <param name="PSInfo">Struct containing the current search settings.</param>
        /// <param name="CPages">Struct containing the code page settings.</param>
        /// <returns>Return 1 if you have made adjustments to the search terms, or 0 if 
        /// not, or -1 if you are not happy with the current settings at all and want 
        /// the X-Tension to be unselected. Adjustments to the flags or the code pages 
        /// are ignored.</returns>
        [DllExport]
        public static Int32 XT_PrepareSearch(ref SearchPreparationInformation PSInfo
            , IntPtr CPages)
        {
            if (DEBUGGING)
            {
                HelperMethods.XWF_OutputMessage(string.Format(
                    "Search Terms: {0}, Search Flags: {1}"
                  , PSInfo.lpSearchTerms, PSInfo.nFlags));
                
                // Unused code page
                if (CPages != IntPtr.Zero)
                {
                    var codePages = (CodePages)Marshal.PtrToStructure(CPages
                                        , typeof(CodePages));

                    HelperMethods.XWF_OutputMessage(string.Format(
                        ", Code Pages: {0}, {1}, {2}, {3}, {4}"
                        , codePages.nCodePage1, codePages.nCodePage2
                        , codePages.nCodePage3, codePages.nCodePage4
                        , codePages.nCodePage5));
                }
            }

            //
            // Put logic here for handling search preparation.
            //
            // Example code for adjusting search terms
            // PSInfo.lpSearchTerms = PSInfo.lpSearchTerms + "adjusted";
            // PSInfo.nBufLen = (uint)(PSInfo.lpSearchTerms.Length + 1);
            //
            // // Return Value 1: Since we made adjustments to the search terms.
            // return 1;
            //

            // Return Value 0: No search term adjustments made.
            return 0;
        }

        /// <summary>
        /// If exported (optional), will be called for each search hit, either when it 
        /// is found or, in a future version of XWF, later if selected by the user in a 
        /// search hit list.
        /// </summary>
        /// <param name="info">Struct containing details about the search hit.</param>
        /// <returns>Return 0 unless if you want XWF to abort the search (return -1) or 
        /// you want XWF to stop calling you (return -2).</returns>
        [DllExport]
        public static int XT_ProcessSearchHit(ref SearchHitInformation info)
        {
            if (DEBUGGING)
            {
                HelperMethods.XWF_OutputMessage(string.Format(
                    "Search Item ID: {0}, Flags: {1}"
                  , info.nItemID, info.nFlags));
            }

            //
            // Put logic here for processing search hits.
            //

            // Return Value 0: Continue searching.
            return 0;
        }
    }
}
