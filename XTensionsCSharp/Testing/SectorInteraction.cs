using System;

namespace XTensions.Testing
{
    public static class SectorInteraction
    {
        private static void ProcessSector(IntPtr hVolume, Int64 nSectorNumber)
        {
            SectorInformation sectorInfo = HelperMethods.XWF_GetSectorContents(hVolume
                , nSectorNumber);

            HelperMethods.XWF_OutputMessage("Sector Number: " + nSectorNumber
                , OutputMessageLevel.Level3);
            HelperMethods.XWF_OutputMessage("Is Sector Allocated: "
                + sectorInfo.IsAllocated, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Sector Description: "
                + sectorInfo.Description, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Sector Owner Item ID: "
                + sectorInfo.OwnerItemID, OutputMessageLevel.Level4);

            HelperMethods.XWF_OutputEmptyLine();
        }

        /// <summary>
        /// Run a test for the block manipulation methods.
        /// </summary>
        /// <param name="hVolume">The current volume pointer.</param>
        /// <param name="nOpType">The current operation type.</param>
        /// <returns></returns>
        public static void RunTest(IntPtr hVolume
            , XTensionActionSource nOpType = XTensionActionSource.MainMenu)
        {
            HelperMethods.XWF_OutputHeader("SECTOR INTERACTION TESTING MODULE"
                , OutputMessageLevel.Level1);

            // XWF_GetSize() test.
            HelperMethods.XWF_OutputHeader("XWF_GetSize() Test"
                , OutputMessageLevel.Level2);

            var itemPhysicalSize = HelperMethods.XWF_GetSize(hVolume
                , ItemSizeType.PhysicalSize);
            HelperMethods.XWF_OutputMessage("Volume Physical Size: " + itemPhysicalSize
                , OutputMessageLevel.Level3);

            Int32 minSize = 1000000000;
            if (itemPhysicalSize < minSize)
            {
                HelperMethods.XWF_OutputMessage("Provided item must be at least " 
                    + minSize + " bytes.", OutputMessageLevel.Level3);

                HelperMethods.XWF_OutputEmptyLine();
                return;
            }

            HelperMethods.XWF_OutputEmptyLine();

            // XWF_GetSectorContents() test.
            HelperMethods.XWF_OutputHeader("XWF_GetSectorContents() Test"
                , OutputMessageLevel.Level2);

            ProcessSector(hVolume, 100);
            ProcessSector(hVolume, 1000);
            ProcessSector(hVolume, 8696);
            ProcessSector(hVolume, 10000);
            ProcessSector(hVolume, 100000);
            ProcessSector(hVolume, 1000000);
            ProcessSector(hVolume, 10000000);
            ProcessSector(hVolume, 100000000);
            ProcessSector(hVolume, 1000000000);
        }
    }
}
