using System;
using System.Runtime.InteropServices;

namespace XTensions
{
    /// <summary>
    /// Contains imported X-Ways X-Tension API methods and easier to use wrappers
    /// </summary>
    public static class ImportedMethods
    {

        // The following delegates are used to call the X-Tension API methods. Those
        // commented out are not yet implemented.
        public delegate long XWFGetSizeDelegate(IntPtr hVolumeOrItem
            , ItemSizeType lpOptional);
        public static XWFGetSizeDelegate XWF_GetSize;

        public delegate IntPtr XWFGetVolumeNameDelegate(IntPtr hVolume
            , IntPtr lpString, VolumeNameType nType);
        public static XWFGetVolumeNameDelegate XWF_GetVolumeName;

        public delegate void XWFGetVolumeInformationDelegate(IntPtr hVolume
            , out VolumeFileSystem lpFileSystem, out uint nBytesPerSector
            , out uint nSectorsPerCluster, out long nClusterCount
            , out long nFirstClusterSectorNo);
        public static XWFGetVolumeInformationDelegate XWF_GetVolumeInformation;

        public delegate bool XWFGetBlockDelegate(IntPtr hVolume, out long lpStartOfs
            , out long lpEndOfs);
        public static XWFGetBlockDelegate XWF_GetBlock;

        public delegate bool XWFSetBlockDelegate(IntPtr hVolume, long lpStartOfs
            , long lpEndOfs);
        public static XWFSetBlockDelegate XWF_SetBlock;

        public delegate bool XWFGetSectorContentsDelegate(IntPtr hVolume
            , long nSectorNo, IntPtr lpDescr, out long lpItemID);
        public static XWFGetSectorContentsDelegate XWF_GetSectorContents;

        /*
        public delegate IntPtr XWFOpenVolumeDelegate(IntPtr hEvidence);
        public static XWFOpenVolumeDelegate XWF_OpenVolume;
        */

        public delegate IntPtr XWFOpenItemDelegate(IntPtr hVolume, long nItemID
            , ItemOpenModes nFlags);
        public static XWFOpenItemDelegate XWF_OpenItem;

        public delegate void XWFCloseDelegate(IntPtr hVolumeOrItem);
        public static XWFCloseDelegate XWF_Close;

        public delegate uint XWFReadDelegate(IntPtr hVolumeOrItem, long Offset
            , IntPtr lpBuffer, uint nNumberOfBytesToRead);
        public static XWFReadDelegate XWF_Read;

        /*
        public delegate long XWFWriteDelegate(IntPtr hVolumeOrItem, long nOffset
            , IntPtr lpBuffer, uint nNumberOfBytesToWrite);
        public static XWFWriteDelegate XWF_Write;
        */

        public delegate long XWFGetCasePropDelegate(IntPtr pReserved, int nPropType
            , IntPtr pBuffer, int nBufSize);
        public static XWFGetCasePropDelegate XWF_GetCaseProp;

        public delegate IntPtr XWFGetFirstEvObjDelegate(IntPtr pReserved);
        public static XWFGetFirstEvObjDelegate XWF_GetFirstEvObj;

        public delegate IntPtr XWFGetNextEvObjDelegate(IntPtr hPrevEvidence
            , IntPtr pReserved);
        public static XWFGetNextEvObjDelegate XWF_GetNextEvObj;

        /*
        public delegate IntPtr XWFDeleteEvObjDelegate(IntPtr hEvidence);
        public static XWFDeleteEvObjDelegate XWF_DeleteEvObj;
        */

        public delegate IntPtr XWFCreateEvObjDelegate(EvidenceObjectCategory nType
            , EvidenceObjectType nDiskID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpPath, IntPtr pReserved);
        public static XWFCreateEvObjDelegate XWF_CreateEvObj;

        public delegate IntPtr XWFOpenEvObjDelegate(IntPtr hEvidence
            , EvidenceOpenOptions nFlags);
        public static XWFOpenEvObjDelegate XWF_OpenEvObj;

        public delegate void XWFCloseEvObjDelegate(IntPtr hEvidence);
        public static XWFCloseEvObjDelegate XWF_CloseEvObj;

        public delegate long XWFGetEvObjPropDelegate(IntPtr hEvidence
            , EvidencePropertyType nPropType, IntPtr pBuffer);
        public static XWFGetEvObjPropDelegate XWF_GetEvObjProp;

        /*
        public delegate int XWFSetEvObjPropDelegate(IntPtr hEvidence
            , uint nPropType, IntPtr pBuffer);
        public static XWFSetEvObjPropDelegate XWF_SetEvObjProp;
        */

        public delegate IntPtr XWFGetEvObjDelegate(uint nEvObjID);
        public static XWFGetEvObjDelegate XWF_GetEvObj;

        public delegate IntPtr XWFGetReportTableInfoDelegate(IntPtr pReserved
            , int nReportTableID, ReportTableInformationOptions lpOptional);
        public static XWFGetReportTableInfoDelegate XWF_GetReportTableInfo;

        public delegate IntPtr XWFGetEvObjReportTableAssocsDelegate(IntPtr hEvidence
            , uint nFlags, out IntPtr lpValue);
        public static XWFGetEvObjReportTableAssocsDelegate
            XWF_GetEvObjReportTableAssocs;

        public delegate void XWFSelectVolumeSnapshotDelegate(IntPtr hVolume);
        public static XWFSelectVolumeSnapshotDelegate XWF_SelectVolumeSnapshot;

        public delegate long XWFGetVSPropDelegate(VolumeSnapshotPropertyType nPropType
            , SpecialItemType pBuffer);
        public static XWFGetVSPropDelegate XWF_GetVSProp;

        public delegate uint XWFGetItemCountDelegate(IntPtr pReserved);
        public static XWFGetItemCountDelegate XWF_GetItemCount;

        public delegate uint XWFGetFileCountDelegate(int nDirID);
        public static XWFGetFileCountDelegate XWF_GetFileCount;

        /*
        public delegate int XWFGetSpecialItemIDDelegate(uint nSpecialItem);
        public static XWFGetSpecialItemIDDelegate XWF_GetSpecialItemID;
        */

        public delegate int XWFCreateItemDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpName
            , CreateItemOptions flags);
        public static XWFCreateItemDelegate XWF_CreateItem;

        public delegate int XWFCreateFileDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpName
            , CreateFileOptions nCreationFlags, long nParentItemID
            , IntPtr pSourceInfo);
        public static XWFCreateFileDelegate XWF_CreateFile;

        /*
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public delegate string XWFGetItemNameDelegate(int itemID);
        public static XWFGetItemNameDelegate XWF_GetItemName;
        */

        public delegate IntPtr XWFGetItemNameDelegate(int itemID);
        public static XWFGetItemNameDelegate XWF_GetItemName;

        /*
        public delegate void XWFSetItemNameDelegate(int nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpName);
        public static XWFSetItemNameDelegate XWF_SetItemName;
        */

        public delegate long XWFGetItemSizeDelegate(long itemID);
        public static XWFGetItemSizeDelegate XWF_GetItemSize;

        public delegate void XWFSetItemSizeDelegate(int nItemID, long size);
        public static XWFSetItemSizeDelegate XWF_SetItemSize;

        public delegate void XWFGetItemOfsDelegate(int nItemID, out long lpDefOfs
            , out long lpStartSector);
        public static XWFGetItemOfsDelegate XWF_GetItemOfs;

        public delegate void XWFSetItemOfsDelegate(int nItemID, long nDefOfs,
            long nStartSector);
        public static XWFSetItemOfsDelegate XWF_SetItemOfs;

        public delegate long XWFGetItemInformationDelegate(int nItemID
            , ItemInformationType InfoType, out bool lpSuccess);
        public static XWFGetItemInformationDelegate XWF_GetItemInformation;

        public delegate bool XWFSetItemInformationDelegate(int nItemID
            , ItemInformationType InfoType, long nInfoValue);
        public static XWFSetItemInformationDelegate XWF_SetItemInformation;

        public delegate ItemTypeCategory XWFGetItemTypeDelegate(int nItemID
            , IntPtr lpTypeDescr, int nBufferLen);
        public static XWFGetItemTypeDelegate XWF_GetItemType;

        public delegate void XWFSetItemTypeDelegate(int nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpTypeDescr
            , ItemTypeCategory nItemType);
        public static XWFSetItemTypeDelegate XWF_SetItemType;

        public delegate int XWFGetItemParentDelegate(int nItemID);
        public static XWFGetItemParentDelegate XWF_GetItemParent;

        public delegate void XWFSetItemParentDelegate(int nChildItemID
            , int nParentItemID);
        public static XWFSetItemParentDelegate XWF_SetItemParent;

        public delegate int XWFGetReportTableAssocsDelegate(int itemID
            , IntPtr buffer, int bufferLength);
        public static XWFGetReportTableAssocsDelegate XWF_GetReportTableAssocs;

        public delegate AddToReportTableResult XWFAddToReportTableDelegate(int nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpReportTableName
            , AddToReportTableOptions nFlags);
        public static XWFAddToReportTableDelegate XWF_AddToReportTable;

        public delegate IntPtr XWFGetCommentDelegate(int ItemID);
        public static XWFGetCommentDelegate XWF_GetComment;

        public delegate bool XWFAddCommentDelegate(int nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpComment
            , AddCommentMode nHowToAdd);
        public static XWFAddCommentDelegate XWF_AddComment;

        public delegate IntPtr XWFGetExtractedMetadataDelegate(int ItemID);
        public static XWFGetExtractedMetadataDelegate XWF_GetExtractedMetadata;

        public delegate bool XWFAddExtractedMetadataDelegate(int nItemID
            , string lpComment, AddCommentMode nFlagsHowToAdd);
        public static XWFAddExtractedMetadataDelegate XWF_AddExtractedMetadata;

        public delegate bool XWFGetHashValueDelegate(int nItemID, IntPtr lpBuffer);
        public static XWFGetHashValueDelegate XWF_GetHashValue;

        /*
        public delegate bool XWFSetHashValueDelegate(IntPtr nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpComment, uint nFlags);
        public static XWFSetHashValueDelegate XWF_SetHashValue;
        */

        /*
        public delegate void XWFSetItemDataRunsDelegate(IntPtr nItemID
            , IntPtr lpBuffer);
        public static XWFSetItemDataRunsDelegate XWF_SetItemDataRuns;
        */

        [return: MarshalAs(UnmanagedType.LPWStr)]
        public delegate string XWFGetMetadataDelegate(int nItemID, IntPtr hItem);
        public static XWFGetMetadataDelegate XWF_GetMetadata;

        public delegate IntPtr XWFGetRasterImageDelegate(RasterImageInformation RIInfo);
        public static XWFGetRasterImageDelegate XWF_GetRasterImage;

        public delegate int XWFSearchDelegate(ref SearchInformation SInfo
            , ref CodePages CPages);
        public static XWFSearchDelegate XWF_Search;

        public delegate int XWFSearchWithPtrToPagesDelegate(ref SearchInformation SInfo
            , IntPtr CPages);
        public static XWFSearchWithPtrToPagesDelegate XWF_SearchWithPtrToPages;

        /*
        public delegate uint XWFGetSearchHitCountDelegate(IntPtr hVolume);
        public static XWFGetSearchHitCountDelegate XWFGetSearchHitCount;
        */

        /*
        public delegate uint XWFGetSearchHitInfoDelegate(IntPtr hVolume
            , uint nSearchHitNo, out SearchHitInfo Info);
        public static XWFGetSearchHitInfoDelegate XWFGetSearchHitInfo;
        */

        /*
        public delegate uint XWFAddSearchTermDelegate(
            [MarshalAs(UnmanagedType.LPWStr)] string lpSearchTermDescr, uint nFlags);
        public static XWFAddSearchTermDelegate XWFAddSearchTerm;
        */

        [return: MarshalAs(UnmanagedType.LPWStr)]
        public delegate string XWFGetSearchTermDelegate(int nSearchTermID
            , IntPtr pReserved);
        public static XWFGetSearchTermDelegate XWF_GetSearchTerm;

        [return: MarshalAs(UnmanagedType.SysInt)]
        public delegate int XWFGetSearchTermCountDelegate(int nSearchTermID
            , IntPtr pReserved);
        public static XWFGetSearchTermCountDelegate XWF_GetSearchTermCount;

        /*
        public delegate uint XWFAddSearchHitDelegate();
        public static XWFAddSearchHitDelegate XWFAddSearchHit;
        */

        public delegate int XWFAddEventDelegate(EventInformation Evt);
        public static XWFAddEventDelegate XWF_AddEvent;

        public delegate IntPtr XWFGetEventDelegate(uint nEventNo, EventInformation Evt);
        public static XWFGetEventDelegate XWF_GetEvent;

        public delegate IntPtr XWFCreateContainerDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpFileName
            , ContainerCreationOptions nFlags, IntPtr pReserved);
        public static XWFCreateContainerDelegate XWF_CreateContainer;

        public delegate int XWFCopyToContainerDelegate(IntPtr hContainer, IntPtr hItem
            , CopyToContainerOptions nFlags, CopyToContainerMode nMode
            , long nStartOfs, long nEndOfs, IntPtr pReserved);
        public static XWFCopyToContainerDelegate XWF_CopyToContainer;

        public delegate int XWFCloseContainerDelegate(IntPtr hContainer,
            IntPtr pReserved);
        public static XWFCloseContainerDelegate XWF_CloseContainer;

        public delegate void XWFOutputMessageDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpMessage
            , OutputMessageOptions_XWF nFlags = 0);
        public static XWFOutputMessageDelegate OutputMessage;

        public delegate long XWFGetUserInputDelegate([MarshalAs(UnmanagedType.LPWStr)]
            string lpMessage, IntPtr lpBuffer, uint nBufferLen, UserInputOptions nFlags);
        public static XWFGetUserInputDelegate XWF_GetUserInput;

        public delegate void XWFShowProgressDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpCaption
            , ProgressIndicatorOptions nFlags);
        public static XWFShowProgressDelegate XWF_ShowProgress;

        public delegate void XWFSetProgressPercentageDelegate(uint nPercent);
        public static XWFSetProgressPercentageDelegate XWF_SetProgressPercentage;

        public delegate void XWFSetProgressDescriptionDelegate(
            [MarshalAs(UnmanagedType.LPWStr)] string lpStr);
        public static XWFSetProgressDescriptionDelegate XWF_SetProgressDescription;

        public delegate bool XWFShouldStopDelegate();
        public static XWFShouldStopDelegate XWF_ShouldStop;

        public delegate void XWFHideProgressDelegate();
        public static XWFHideProgressDelegate XWF_HideProgress;

        public delegate bool XWFReleaseMemDelegate(IntPtr lpBuffer);
        public static XWFReleaseMemDelegate XWF_ReleaseMem;

        /// <summary>
        /// Imports methods from the XWF API.
        /// </summary>
        /// <typeparam name="T">Function pointer delegate name.</typeparam>
        /// <param name="moduleHandle">Pointer to the module.</param>
        /// <param name="methodName">Matched method name from XWF API.</param>
        /// <returns>Returns function pointer delegate if the method is found; otherwise
        /// throws an exception.</returns>
        private static T GetMethodDelegate<T>(IntPtr moduleHandle, string methodName)
            where T : class
        {
            // Get a pointer to the address of the specified method in the XWF API.
            IntPtr ptr = NativeMethods.GetProcAddress(moduleHandle, methodName);

            // If the method if found, return a function pointer delegate for it.
            if (ptr != IntPtr.Zero)
            {
                return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
            }

            throw new ArgumentException(methodName + " not found!");
        }

        /// <summary>
        /// Retrieves the X-Tensions API function pointers. Should be called once at 
        /// startup (in XT_Init).
        /// </summary>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool Import()
        {
            try
            {
                // Get a handle for the XWF API.
                IntPtr moduleHandle = NativeMethods.GetModuleHandle(IntPtr.Zero);

                XWF_GetSize = GetMethodDelegate<XWFGetSizeDelegate>(
                    moduleHandle, "XWF_GetSize");

                XWF_GetVolumeName = GetMethodDelegate<XWFGetVolumeNameDelegate>(
                    moduleHandle, "XWF_GetVolumeName");

                XWF_GetVolumeInformation 
                    = GetMethodDelegate<XWFGetVolumeInformationDelegate>(
                    moduleHandle, "XWF_GetVolumeInformation");

                XWF_GetBlock = GetMethodDelegate<XWFGetBlockDelegate>(
                    moduleHandle, "XWF_GetBlock");

                XWF_SetBlock = GetMethodDelegate<XWFSetBlockDelegate>(
                    moduleHandle, "XWF_SetBlock");

                XWF_GetSectorContents = GetMethodDelegate<XWFGetSectorContentsDelegate>(
                    moduleHandle, "XWF_GetSectorContents");

                /*
                XWFOpenVolume = GetMethodDelegate<XWFOpenVolumeDelegate>(
                    moduleHandle, "XWF_OpenVolume");
                */

                XWF_OpenItem = GetMethodDelegate<XWFOpenItemDelegate>(
                    moduleHandle, "XWF_OpenItem");

                XWF_Close = GetMethodDelegate<XWFCloseDelegate>(
                    moduleHandle, "XWF_Close");

                XWF_Read = GetMethodDelegate<XWFReadDelegate>(
                    moduleHandle, "XWF_Read");

                /*
                XWFWrite = GetMethodDelegate<XWFWriteDelegate>(
                    moduleHandle, "XWF_Write");
                */

                XWF_GetCaseProp = GetMethodDelegate<XWFGetCasePropDelegate>(
                    moduleHandle, "XWF_GetCaseProp");

                XWF_GetFirstEvObj = GetMethodDelegate<XWFGetFirstEvObjDelegate>(
                    moduleHandle, "XWF_GetFirstEvObj");

                XWF_GetNextEvObj = GetMethodDelegate<XWFGetNextEvObjDelegate>(
                    moduleHandle, "XWF_GetNextEvObj");

                /*
                XWFDeleteEvObj = GetMethodDelegate<XWFDeleteEvObjDelegate>(
                    moduleHandle, "XWF_DeleteEvObj");
                */

                XWF_CreateEvObj = GetMethodDelegate<XWFCreateEvObjDelegate>(
                    moduleHandle, "XWF_CreateEvObj");

                XWF_OpenEvObj = GetMethodDelegate<XWFOpenEvObjDelegate>(
                    moduleHandle, "XWF_OpenEvObj");

                XWF_CloseEvObj = GetMethodDelegate<XWFCloseEvObjDelegate>(
                    moduleHandle, "XWF_CloseEvObj");

                XWF_GetEvObjProp = GetMethodDelegate<XWFGetEvObjPropDelegate>(
                    moduleHandle, "XWF_GetEvObjProp");

                /*
                XWFSetEvObjProp = GetMethodDelegate<XWFSetEvObjPropDelegate>(
                    moduleHandle, "XWF_SetEvObjProp");
                */

                XWF_GetEvObj = GetMethodDelegate<XWFGetEvObjDelegate>(
                    moduleHandle, "XWF_GetEvObj");

                XWF_GetReportTableInfo 
                    = GetMethodDelegate<XWFGetReportTableInfoDelegate>(
                    moduleHandle, "XWF_GetReportTableInfo");

                XWF_GetEvObjReportTableAssocs 
                    = GetMethodDelegate<XWFGetEvObjReportTableAssocsDelegate>(
                    moduleHandle, "XWF_GetEvObjReportTableAssocs");

                XWF_SelectVolumeSnapshot 
                    = GetMethodDelegate<XWFSelectVolumeSnapshotDelegate>(
                    moduleHandle, "XWF_SelectVolumeSnapshot");

                XWF_GetVSProp = GetMethodDelegate<XWFGetVSPropDelegate>(
                    moduleHandle, "XWF_GetVSProp");

                XWF_GetItemCount = GetMethodDelegate<XWFGetItemCountDelegate>(
                    moduleHandle, "XWF_GetItemCount");

                XWF_GetFileCount = GetMethodDelegate<XWFGetFileCountDelegate>(
                    moduleHandle, "XWF_GetFileCount");

                /*
                XWFGetSpecialItemID = GetMethodDelegate<XWFGetSpecialItemIDDelegate>(
                    moduleHandle, "XWF_GetSpecialItemID");
                */

                XWF_CreateItem = GetMethodDelegate<XWFCreateItemDelegate>(
                    moduleHandle, "XWF_CreateItem");

                XWF_CreateFile = GetMethodDelegate<XWFCreateFileDelegate>(
                    moduleHandle, "XWF_CreateFile");

                XWF_GetItemName = GetMethodDelegate<XWFGetItemNameDelegate>(
                    moduleHandle, "XWF_GetItemName");

                /*
                XWFSetItemName = GetMethodDelegate<XWFSetItemNameDelegate>(
                    moduleHandle, "XWF_SetItemName");
                */

                XWF_GetItemSize = GetMethodDelegate<XWFGetItemSizeDelegate>(
                    moduleHandle, "XWF_GetItemSize");

                XWF_SetItemSize = GetMethodDelegate<XWFSetItemSizeDelegate>(
                    moduleHandle, "XWF_SetItemSize");

                XWF_GetItemOfs = GetMethodDelegate<XWFGetItemOfsDelegate>(
                    moduleHandle, "XWF_GetItemOfs");

                XWF_SetItemOfs = GetMethodDelegate<XWFSetItemOfsDelegate>(
                    moduleHandle, "XWF_SetItemOfs");

                XWF_GetItemInformation 
                    = GetMethodDelegate<XWFGetItemInformationDelegate>(
                    moduleHandle, "XWF_GetItemInformation");

                XWF_SetItemInformation 
                    = GetMethodDelegate<XWFSetItemInformationDelegate>(
                    moduleHandle, "XWF_SetItemInformation");

                XWF_GetItemType = GetMethodDelegate<XWFGetItemTypeDelegate>(
                    moduleHandle, "XWF_GetItemType");

                XWF_SetItemType = GetMethodDelegate<XWFSetItemTypeDelegate>(
                    moduleHandle, "XWF_SetItemType");

                XWF_GetItemParent = GetMethodDelegate<XWFGetItemParentDelegate>(
                    moduleHandle, "XWF_GetItemParent");

                XWF_SetItemParent = GetMethodDelegate<XWFSetItemParentDelegate>(
                    moduleHandle, "XWF_SetItemParent");

                XWF_GetReportTableAssocs 
                    = GetMethodDelegate<XWFGetReportTableAssocsDelegate>(
                    moduleHandle, "XWF_GetReportTableAssocs");

                XWF_AddToReportTable = GetMethodDelegate<XWFAddToReportTableDelegate>(
                    moduleHandle, "XWF_AddToReportTable");

                XWF_GetComment = GetMethodDelegate<XWFGetCommentDelegate>(
                    moduleHandle, "XWF_GetComment");

                XWF_AddComment = GetMethodDelegate<XWFAddCommentDelegate>(
                    moduleHandle, "XWF_AddComment");

                XWF_GetExtractedMetadata 
                    = GetMethodDelegate<XWFGetExtractedMetadataDelegate>(
                    moduleHandle, "XWF_GetExtractedMetadata");

                XWF_AddExtractedMetadata 
                    = GetMethodDelegate<XWFAddExtractedMetadataDelegate>(
                    moduleHandle, "XWF_AddExtractedMetadata");

                XWF_GetHashValue = GetMethodDelegate<XWFGetHashValueDelegate>(
                    moduleHandle, "XWF_GetHashValue");

                /*
                XWFSetHashValue = GetMethodDelegate<XWFSetHashValueDelegate>(
                    moduleHandle, "XWF_SetHashValue");
                */

                /*
                XWFSetItemDataRuns = GetMethodDelegate<XWFSetItemDataRunsDelegate>(
                    moduleHandle, "XWF_SetItemDataRuns");
                */

                XWF_GetMetadata = GetMethodDelegate<XWFGetMetadataDelegate>(
                    moduleHandle, "XWF_GetMetadata");

                XWF_GetRasterImage = GetMethodDelegate<XWFGetRasterImageDelegate>(
                    moduleHandle, "XWF_GetRasterImage");

                XWF_Search = GetMethodDelegate<XWFSearchDelegate>(
                    moduleHandle, "XWF_Search");

                XWF_SearchWithPtrToPages 
                    = GetMethodDelegate<XWFSearchWithPtrToPagesDelegate>(
                    moduleHandle, "XWF_Search");

                /*
                XWFGetSearchHitCount = GetMethodDelegate<XWFGetSearchHitCountDelegate>(
                    moduleHandle, "XWF_GetSearchHitCount");
                */

                /*
                XWFGetSearchHitInfo = GetMethodDelegate<XWFGetSearchHitInfoDelegate>(
                    moduleHandle, "XWF_GetSearchHitInfo");
                */

                /*
                XWFAddSearchTerm = GetMethodDelegate<XWFAddSearchTermDelegate>(
                    moduleHandle, "XWF_AddSearchTerm");
                */

                XWF_GetSearchTerm = GetMethodDelegate<XWFGetSearchTermDelegate>(
                    moduleHandle, "XWF_GetSearchTerm");

                XWF_GetSearchTermCount 
                    = GetMethodDelegate<XWFGetSearchTermCountDelegate>(moduleHandle, 
                    "XWF_GetSearchTerm");

                /*
                XWFAddSearchHit = GetMethodDelegate<XWFAddSearchHitDelegate>(
                    moduleHandle, "XWF_AddSearchHit");
                */

                XWF_AddEvent = GetMethodDelegate<XWFAddEventDelegate>(
                    moduleHandle, "XWF_AddEvent");

                XWF_GetEvent = GetMethodDelegate<XWFGetEventDelegate>(
                    moduleHandle, "XWF_GetEvent");

                XWF_CreateContainer = GetMethodDelegate<XWFCreateContainerDelegate>(
                    moduleHandle, "XWF_CreateContainer");

                XWF_CopyToContainer = GetMethodDelegate<XWFCopyToContainerDelegate>(
                    moduleHandle, "XWF_CopyToContainer");

                XWF_CloseContainer = GetMethodDelegate<XWFCloseContainerDelegate>(
                    moduleHandle, "XWF_CloseContainer");

                OutputMessage = GetMethodDelegate<XWFOutputMessageDelegate>(
                    moduleHandle, "OutputMessage");

                XWF_GetUserInput = GetMethodDelegate<XWFGetUserInputDelegate>(
                    moduleHandle, "XWF_GetUserInput");

                XWF_ShowProgress = GetMethodDelegate<XWFShowProgressDelegate>(
                    moduleHandle, "XWF_ShowProgress");

                XWF_SetProgressPercentage 
                    = GetMethodDelegate<XWFSetProgressPercentageDelegate>(
                    moduleHandle, "XWF_SetProgressPercentage");

                XWF_SetProgressDescription 
                    = GetMethodDelegate<XWFSetProgressDescriptionDelegate>(
                    moduleHandle, "XWF_SetProgressDescription");

                XWF_ShouldStop = GetMethodDelegate<XWFShouldStopDelegate>(
                    moduleHandle, "XWF_ShouldStop");

                XWF_HideProgress = GetMethodDelegate<XWFHideProgressDelegate>(
                    moduleHandle, "XWF_HideProgress");

                XWF_ReleaseMem = GetMethodDelegate<XWFReleaseMemDelegate>(
                    moduleHandle, "XWF_RelaseMem");
            }
            catch(Exception e)
            {
                HelperMethods.OutputMessage("Exception: " + e);
                return false;
            }

            return true;            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    static class NativeMethods
    {                        
        // lpModuleName is declared as IntPtr in order to pass NULL through it
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(IntPtr lpModuleName);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
