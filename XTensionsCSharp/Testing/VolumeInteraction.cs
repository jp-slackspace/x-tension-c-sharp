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
            HelperMethods.OutputHeader("VOLUME INTERACTION TESTING MODULE"
                , OutputMessageLevel.Level1);

            // XWF_GetSize() test.
            HelperMethods.OutputHeader("XWF_GetSize() Test"
                , OutputMessageLevel.Level2);

            var itemPhysicalSize = HelperMethods.GetSize(hVolume
                , ItemSizeType.PhysicalSize);
            HelperMethods.OutputMessage("Volume Physical Size: "
                + itemPhysicalSize, OutputMessageLevel.Level3);

            var itemLogicalSize = HelperMethods.GetSize(hVolume
                , ItemSizeType.LogicalSize);
            HelperMethods.OutputMessage("Volume Logical Size: "
                + itemLogicalSize, OutputMessageLevel.Level3);

            var itemValidDateLength = HelperMethods.GetSize(hVolume
                , ItemSizeType.ValidDataLength);
            HelperMethods.OutputMessage("Volume Valid Data Length: "
                + itemValidDateLength, OutputMessageLevel.Level3);

            HelperMethods.OutputEmptyLine();

            // XWF_GetVolumeName() test.
            // TODO: Find out what the different types are exactly.
            HelperMethods.OutputHeader("XWF_GetVolumeName() Test"
                , OutputMessageLevel.Level2);

            string type1VolumeName = HelperMethods.GetVolumeName(hVolume
                , VolumeNameType.Type1);
            HelperMethods.OutputMessage("Type 1 Volume Name: " + type1VolumeName
                , OutputMessageLevel.Level3);

            string type2VolumeName = HelperMethods.GetVolumeName(hVolume
                , VolumeNameType.Type2);
            HelperMethods.OutputMessage("Type 2 Volume Name: " + type2VolumeName
                , OutputMessageLevel.Level3);

            string type3VolumeName = HelperMethods.GetVolumeName(hVolume
                , VolumeNameType.Type3);
            HelperMethods.OutputMessage("Type 3 Volume Name: " + type3VolumeName
                , OutputMessageLevel.Level3);

            HelperMethods.OutputEmptyLine();

            // XWF_GetVolumeInformation() test.
            HelperMethods.OutputHeader("XWF_GetVolumeInformation() Test"
                , OutputMessageLevel.Level2);

            VolumeInformation volumeInformation
                = HelperMethods.GetVolumeInformation(hVolume);

            HelperMethods.OutputMessage("File System: "
                + volumeInformation.FileSystem, OutputMessageLevel.Level3);
            HelperMethods.OutputMessage("Bytes/Sector: "
                + volumeInformation.BytesPerSector, OutputMessageLevel.Level3);
            HelperMethods.OutputMessage("Sectors/Cluster: "
                + volumeInformation.SectorsPerCluster, OutputMessageLevel.Level3);
            HelperMethods.OutputMessage("Cluster Count: "
                + volumeInformation.ClusterCount, OutputMessageLevel.Level3);
            HelperMethods.OutputMessage("First Cluster Sector Number: "
                + volumeInformation.FirstClusterSectorNumber, OutputMessageLevel.Level3);

            HelperMethods.OutputEmptyLine();
        }
    }
}
