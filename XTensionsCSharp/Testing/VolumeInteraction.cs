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
            HelperMethods.XWF_OutputHeader("VOLUME INTERACTION TESTING MODULE"
                , OutputMessageLevel.Level1);

            // XWF_GetSize() test.
            HelperMethods.XWF_OutputHeader("XWF_GetSize() Test"
                , OutputMessageLevel.Level2);

            var itemPhysicalSize = HelperMethods.XWF_GetSize(hVolume
                , ItemSizeType.PhysicalSize);
            HelperMethods.XWF_OutputMessage("Volume Physical Size: "
                + itemPhysicalSize, OutputMessageLevel.Level3);

            var itemLogicalSize = HelperMethods.XWF_GetSize(hVolume
                , ItemSizeType.LogicalSize);
            HelperMethods.XWF_OutputMessage("Volume Logical Size: "
                + itemLogicalSize, OutputMessageLevel.Level3);

            var itemValidDateLength = HelperMethods.XWF_GetSize(hVolume
                , ItemSizeType.ValidDataLength);
            HelperMethods.XWF_OutputMessage("Volume Valid Data Length: "
                + itemValidDateLength, OutputMessageLevel.Level3);

            HelperMethods.XWF_OutputEmptyLine();

            // XWF_GetVolumeName() test.
            // TODO: Find out what the different types are exactly.
            HelperMethods.XWF_OutputHeader("XWF_GetVolumeName() Test"
                , OutputMessageLevel.Level2);

            string type1VolumeName = HelperMethods.XWF_GetVolumeName(hVolume
                , VolumeNameType.Type1);
            HelperMethods.XWF_OutputMessage("Type 1 Volume Name: " + type1VolumeName
                , OutputMessageLevel.Level3);

            string type2VolumeName = HelperMethods.XWF_GetVolumeName(hVolume
                , VolumeNameType.Type2);
            HelperMethods.XWF_OutputMessage("Type 2 Volume Name: " + type2VolumeName
                , OutputMessageLevel.Level3);

            string type3VolumeName = HelperMethods.XWF_GetVolumeName(hVolume
                , VolumeNameType.Type3);
            HelperMethods.XWF_OutputMessage("Type 3 Volume Name: " + type3VolumeName
                , OutputMessageLevel.Level3);

            HelperMethods.XWF_OutputEmptyLine();

            // XWF_GetVolumeInformation() test.
            HelperMethods.XWF_OutputHeader("XWF_GetVolumeInformation() Test"
                , OutputMessageLevel.Level2);

            VolumeInformation volumeInformation
                = HelperMethods.XWF_GetVolumeInformation(hVolume);

            HelperMethods.XWF_OutputMessage("File System: "
                + volumeInformation.FileSystem, OutputMessageLevel.Level3);
            HelperMethods.XWF_OutputMessage("Bytes/Sector: "
                + volumeInformation.BytesPerSector, OutputMessageLevel.Level3);
            HelperMethods.XWF_OutputMessage("Sectors/Cluster: "
                + volumeInformation.SectorsPerCluster, OutputMessageLevel.Level3);
            HelperMethods.XWF_OutputMessage("Cluster Count: "
                + volumeInformation.ClusterCount, OutputMessageLevel.Level3);
            HelperMethods.XWF_OutputMessage("First Cluster Sector Number: "
                + volumeInformation.FirstClusterSectorNumber, OutputMessageLevel.Level3);

            HelperMethods.XWF_OutputEmptyLine();
        }
    }
}
