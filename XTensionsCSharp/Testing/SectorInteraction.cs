using System;

namespace XTensions.Testing
{
    public static class SectorInteraction
    {
        private static void ProcessSector(IntPtr hVolume, Int64 nSectorNumber)
        {
            SectorInformation sectorInfo = HelperMethods.GetSectorContents(hVolume
                , nSectorNumber);

            HelperMethods.OutputMessage("Sector Number: " + nSectorNumber
                , OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("Is Sector Allocated: "
                + sectorInfo.IsAllocated, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Sector Description: "
                + sectorInfo.Description, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Sector Owner Item ID: "
                + sectorInfo.OwnerItemID, OutputMessageOptions.Level4);

            HelperMethods.OutputMessage("");
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
            HelperMethods.OutputMessage("SECTOR INTERACTION TESTING MODULE"
                , OutputMessageOptions.Level1 | OutputMessageOptions.Header);

            // GetSize() test.
            HelperMethods.OutputMessage("GetSize() Test"
                , OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            var itemPhysicalSize = HelperMethods.GetSize(hVolume
                , ItemSizeType.PhysicalSize);
            HelperMethods.OutputMessage("Volume Physical Size: " + itemPhysicalSize
                , OutputMessageOptions.Level3);

            Int32 minSize = 1000000000;
            if (itemPhysicalSize < minSize)
            {
                HelperMethods.OutputMessage("Provided item must be at least " 
                    + minSize + " bytes.", OutputMessageOptions.Level3);

                HelperMethods.OutputMessage("");
                return;
            }

            HelperMethods.OutputMessage("");

            // XWF_GetSectorContents() test.
            HelperMethods.OutputMessage("XWF_GetSectorContents() Test"
                , OutputMessageOptions.Level2 | OutputMessageOptions.Header);

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
