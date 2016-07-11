using System;

namespace XTensions.Testing
{
    public static class VolumeInteraction
    {
        /// <summary>
        /// Run a test for the block manipulation methods.
        /// </summary>
        /// <param name="hVolume">The current volume pointer.</param>
        /// <param name="nOpType">The current operation type.</param>
        /// <returns></returns>
        public static void RunTest(IntPtr hVolume, XTensionActionSource nOpType)
        {
            HelperMethods.OutputMessage("VOLUME INTERACTION TESTING MODULE"
                , OutputMessageOptions.Level1 | OutputMessageOptions.Header);

            // XWF_GetSize() test.
            HelperMethods.OutputMessage("XWF_GetSize() Test"
                , OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            var itemPhysicalSize = HelperMethods.GetSize(hVolume
                , ItemSizeType.PhysicalSize);
            HelperMethods.OutputMessage("Volume Physical Size: "
                + itemPhysicalSize, OutputMessageOptions.Level3);

            var itemLogicalSize = HelperMethods.GetSize(hVolume
                , ItemSizeType.LogicalSize);
            HelperMethods.OutputMessage("Volume Logical Size: "
                + itemLogicalSize, OutputMessageOptions.Level3);

            var itemValidDateLength = HelperMethods.GetSize(hVolume
                , ItemSizeType.ValidDataLength);
            HelperMethods.OutputMessage("Volume Valid Data Length: "
                + itemValidDateLength, OutputMessageOptions.Level3);

            HelperMethods.OutputMessage("");

            // XWF_GetVolumeName() test.
            // TODO: Find out what the different types are exactly.
            HelperMethods.OutputMessage("XWF_GetVolumeName() Test"
                , OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            string type1VolumeName = HelperMethods.GetVolumeName(hVolume
                , VolumeNameType.Type1);
            HelperMethods.OutputMessage("Type 1 Volume Name: " + type1VolumeName
                , OutputMessageOptions.Level3);

            string type2VolumeName = HelperMethods.GetVolumeName(hVolume
                , VolumeNameType.Type2);
            HelperMethods.OutputMessage("Type 2 Volume Name: " + type2VolumeName
                , OutputMessageOptions.Level3);

            string type3VolumeName = HelperMethods.GetVolumeName(hVolume
                , VolumeNameType.Type3);
            HelperMethods.OutputMessage("Type 3 Volume Name: " + type3VolumeName
                , OutputMessageOptions.Level3);

            HelperMethods.OutputMessage("");

            // XWF_GetVolumeInformation() test.
            HelperMethods.OutputMessage("XWF_GetVolumeInformation() Test"
                , OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            VolumeInformation volumeInformation
                = HelperMethods.GetVolumeInformation(hVolume);

            HelperMethods.OutputMessage("File System: "
                + volumeInformation.FileSystem, OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("Bytes/Sector: "
                + volumeInformation.BytesPerSector, OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("Sectors/Cluster: "
                + volumeInformation.SectorsPerCluster, OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("Cluster Count: "
                + volumeInformation.ClusterCount, OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("First Cluster Sector Number: "
                + volumeInformation.FirstClusterSectorNumber, OutputMessageOptions.Level3);

            HelperMethods.OutputMessage("");
        }
    }
}
