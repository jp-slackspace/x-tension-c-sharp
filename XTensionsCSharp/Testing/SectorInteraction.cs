using System;

namespace XTensions.Testing
{
    public static class SectorInteraction
    {
        private static void ProcessSector(IntPtr volume, Int64 sectorNumber)
        {
            SectorInformation sectorInfo = HelperMethods.GetSectorContents(volume
                , sectorNumber);

            HelperMethods.OutputMessage("Sector Number: " + sectorNumber
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
        /// <param name="volume">The current volume pointer.</param>
        /// <param name="operationType">The current operation type.</param>
        /// <returns></returns>
        public static void RunTest(IntPtr volume
            , XTensionActionSource operationType = XTensionActionSource.MainMenu)
        {
            HelperMethods.OutputMessage("SECTOR INTERACTION TESTING MODULE"
                , OutputMessageOptions.Level1 | OutputMessageOptions.Header);

            // GetSize() test.
            HelperMethods.OutputMessage("GetSize() Test"
                , OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            var itemPhysicalSize = HelperMethods.GetSize(volume
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
            HelperMethods.OutputMessage("GetSectorContents() Test"
                , OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            ProcessSector(volume, 100);
            ProcessSector(volume, 1000);
            ProcessSector(volume, 8696);
            ProcessSector(volume, 10000);
            ProcessSector(volume, 100000);
            ProcessSector(volume, 1000000);
            ProcessSector(volume, 10000000);
            ProcessSector(volume, 100000000);
            ProcessSector(volume, 1000000000);
        }
    }
}
