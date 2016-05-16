using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace XTensions
{
    public static class HelperMethods
    {
        private static readonly int VolumeNameBufferLength = 256 * 2;
        private static readonly int SectorContentsBufferLength = 512 * 2;
        private static readonly int ItemTypeDescriptionBufferLength = 1024 * 2;
        private static readonly int CasePropertiesLength = 1024 * 2;
        private static readonly int EventObjectPropertiesLength = 128 * 2;

        /// <summary>
        /// Retrieve the size of the provided volume or file.
        /// </summary>
        /// <param name="volumeOrItem">A pointer to the related volume or item.</param>
        /// <param name="sizeType">The type (XWFGetSizeType) of size to retrieve.
        /// </param>
        /// <returns>Returns the size of the volume or file.</returns>
        public static long XWF_GetSize(IntPtr volumeOrItem, ItemSizeType sizeType 
            = ItemSizeType.PhysicalSize)
        {
            // Fail if a volume or item pointer wasn't provided.
            if (volumeOrItem == IntPtr.Zero) return -1;

            // Return the API function call results.
            return ImportedMethods.XWF_GetSize(volumeOrItem, sizeType);
        }

        /// <summary>
        /// Retrieves the UTF-16 name of a given volume, using 255 characters at most.
        /// </summary>
        /// <param name="volume">A pointer to the related volume.</param>
        /// <param name="volumeNameType">The volume name type (XWFVolumeNameType).
        /// </param>
        /// <returns>Returns the volume name.</returns>
        public static string XWF_GetVolumeName(IntPtr volume
            , VolumeNameType volumeNameType = VolumeNameType.Type1)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) return null;

            // Allocate a buffer to receive the volume name, call the API function and
            // get volume name, and clean up.
            IntPtr bufferPtr = Marshal.AllocHGlobal(VolumeNameBufferLength);
            ImportedMethods.XWF_GetVolumeName(volume, bufferPtr, volumeNameType);
            string str = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            return str;
        }

        /// <summary>
        /// Retrieves various information about a given volume.
        /// </summary>
        /// <param name="hVolume">A pointer to the related volume.</param>
        /// <returns>Returns the volume information in a VolumeInformation struct.
        /// </returns>
        public static VolumeInformation XWF_GetVolumeInformation(IntPtr hVolume)
        {
            VolumeInformation info = new VolumeInformation();

            // Call the API function, getting the various volume information.
            ImportedMethods.XWF_GetVolumeInformation(hVolume, out info.FileSystem
                , out info.BytesPerSector, out info.SectorsPerCluster
                , out info.ClusterCount, out info.FirstClusterSectorNo);

            return info;
        }

        /// <summary>
        /// Retrieves the boundaries of the currently selected block, if any.
        /// </summary>
        /// <param name="hVolume">A pointer to the provided volume.</param>
        /// <returns>Returns a structure containing the block extents.  These extents 
        /// will be 0, and -1 respectively if no block is selected.</returns>
        public static BlockBoundaries XWF_GetBlock(IntPtr hVolume)
        {
            BlockBoundaries boundaries = new BlockBoundaries();

            // Call the API function to get the starting and ending offset of the
            // selected block.
            bool result = ImportedMethods.XWF_GetBlock(hVolume
                , out boundaries.StartOffset, out boundaries.EndOffset);

            return boundaries;
        }

        /// <summary>
        /// Set the boundaries of a block.
        /// </summary>
        /// <param name="hVolume">A pointer to the provided volume.</param>
        /// <param name="boundaries">A BlockBoundaries struct indicating the starting
        /// and ending offsets of the new block.  The ending offset should be set to -1
        /// if an existing block is to be cleared.</param>
        /// <returns>Returns TRUE if successful and FALSE if the boundaries specified 
        /// exceed the size of the volume.</returns>
        public static bool XWF_SetBlock(IntPtr hVolume, BlockBoundaries boundaries)
        {
            // Call the API function to set the block and return TRUE if successful and
            // FALSE if the boundaries specified exceed the size of the volume.
            return ImportedMethods.XWF_SetBlock(hVolume, boundaries.StartOffset
                , boundaries.EndOffset);
        }

        /// <summary>
        /// Retrieves information about a certain sector on a volume.
        /// </summary>
        /// <param name="hVolume">A pointer to the provided volume.</param>
        /// <param name="nSectorNo">The sector number.</param>
        /// <returns>Returns the sector information in a SectorInformation struct.
        /// </returns>
        public static SectorInformation XWF_GetSectorContents(IntPtr hVolume
            , long nSectorNo)
        {
            SectorInformation sectorInfo;

            // Allocate a buffer to receive the sector contents description.
            IntPtr bufferPtr = Marshal.AllocHGlobal(SectorContentsBufferLength);

            // Get the results from the API function for the sector information and
            //clean up.
            sectorInfo.IsAllocated = ImportedMethods.XWF_GetSectorContents(hVolume
                , nSectorNo, bufferPtr, out sectorInfo.OwnerItemID);
            if (bufferPtr != IntPtr.Zero)
            {
                sectorInfo.Description = Marshal.PtrToStringUni(bufferPtr);
                Marshal.FreeHGlobal(bufferPtr);
            }
            else
            {
                sectorInfo.Description = null;
            }

            return sectorInfo;
        }

        /// <summary>
        /// Opens the specified item in the specified volume for reading. Available in
        /// v16.5 and later.
        /// </summary>
        /// <param name="hVolume">A pointer to the related volume.</param>
        /// <param name="nItemID">The item ID.</param>
        /// <param name="nFlags">Open flags; use XWFOpenFlags enum.</param>
        /// <returns>Returns a handle to the open item, or 0 if unsuccessful</returns>
        public static IntPtr XWF_OpenItem(IntPtr hVolume, long nItemID
            , ItemOpenModes nFlags = ItemOpenModes.LogicalContents)
        {
            try
            {
                return ImportedMethods.XWF_OpenItem(hVolume, nItemID, nFlags);
            }
            catch (System.AccessViolationException e)
            {
                XWF_OutputMessage("Exception: " + e);
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Closes a volume that was opened with the XWF_OpenVolume function or an item 
        /// that was opened with the XWF_OpenItem function. Available in v16.5 and
        /// later.
        /// </summary>
        /// <param name="hVolumeOrItem">An open volume or item.</param>
        /// <returns>Returns true if a successfull, otherwise false.</returns>
        public static bool XWF_Close(IntPtr volumeOrItem)
        {
            if (volumeOrItem != IntPtr.Zero)
            {
                ImportedMethods.XWF_Close(volumeOrItem);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reads the specified number of bytes from the specified position in the 
        /// specified volume or item.
        /// </summary>
        /// <param name="hVolumeOrItem">A pointer to the related volume or item.</param>
        /// <param name="Offset">The offset to read from.</param>
        /// <param name="nNumberOfBytesToRead">The number of bytes to read.</param>
        /// <returns>Returns a byte array of the bytes read.</returns>
        public static byte[] XWF_Read(IntPtr hVolumeOrItem, long nOffset = 0
            , uint nNumberOfBytesToRead = 0)
        {
            if (hVolumeOrItem != IntPtr.Zero)
            {
                // If number of bytes to read is not specified, assume we want to read
                // the full file from the provided offset, if any.
                if (nNumberOfBytesToRead == 0)
                {
                    nNumberOfBytesToRead = (uint) XWF_GetSize(hVolumeOrItem
                        , ItemSizeType.PhysicalSize) - (uint)nOffset;
                }

                // Initialize and create a pointer to the buffer.
                int bufferSize = (int)nNumberOfBytesToRead;
                IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);

                // Call XWF_Read to read the item into the buffer.
                ImportedMethods.XWF_Read(hVolumeOrItem, nOffset, bufferPtr
                    , nNumberOfBytesToRead);

                // Copy the buffer contents into a byte array and cleanup the buffer.
                byte[] contents = new byte[bufferSize];
                Marshal.Copy(bufferPtr, contents, 0, bufferSize);
                Marshal.FreeHGlobal(bufferPtr);

                return contents;
            }

            return null;
        }

        /// <summary>
        /// Retrieves information about the current case.
        /// </summary>
        /// <returns>Returns a CaseProperties structure.</returns>
        public static CaseProperties XWF_GetCaseProps()
        {
            CaseProperties props = new CaseProperties();

            // Read the title.
            int bufferSize = (int)CasePropertiesLength;
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseTitle, bufferPtr, bufferSize);
            props.caseTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Read the examiner.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseExaminer, bufferPtr, bufferSize);
            props.caseExaminer = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Read the case file path.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseFilePath, bufferPtr, bufferSize);
            props.caseFilePath = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Read the case directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseDirectory, bufferPtr, bufferSize);
            props.caseDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            return props;
        }

        /// <summary>
        /// Retrieves a handle to the first evidence object in the case. In conjunction
        /// with XWF_GetNextEvObj this function allows to enumerate all evidence objects
        /// of the case.
        /// </summary>
        /// <returns>Returns a pointer to the first evidence object.</returns>
        public static IntPtr XWF_GetFirstEvObj()
        {
            return ImportedMethods.XWF_GetFirstEvObj(IntPtr.Zero);
        }

        /// <summary>
        /// Given a pointer to the previous evidence object, retrieves the next evidence
        /// object in the chain.
        /// </summary>
        /// <param name="hPrevEvidence">Previous evidence object.</param>
        /// <returns>Returns a pointer to the next evidence object.</returns>
        public static IntPtr XWF_GetNextEvObj(IntPtr hPrevEvidence)
        {
            // If a zero pointer is provided as previous evidence, return the same.
            if (hPrevEvidence == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            return ImportedMethods.XWF_GetNextEvObj(hPrevEvidence, IntPtr.Zero);
        }

        /// <summary>
        /// Creates one or more evidence objects from one source (which can be a medium,
        /// disk/volume image, memory dump, or a directory/path). Available in v16.5 and
        /// later.
        /// </summary>
        /// <param name="nDiskID">The XWFEvidenceObjTypeID disk type ID</param>
        /// <param name="lpPath">Path in case of a file or directory, otherwise NULL.
        /// </param>
        /// <returns>Returns the first evidence object created, or NULL in case of an
        /// error.</returns>
        public static IntPtr XWF_CreateEvObj(EvidenceObjectType nDiskID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpPath = null)
        {
            // Make sure a path was provided if one is expected.
            if ((nDiskID == EvidenceObjectType.DiskImage
                || nDiskID == EvidenceObjectType.MemoryDump
                || nDiskID == EvidenceObjectType.Directory
                || nDiskID == EvidenceObjectType.File) && lpPath == null)
            {
                return IntPtr.Zero;
            }

            EvidenceObjectCategory nType;

            // Determine the type based on the disk ID provided.
            switch (nDiskID)
            {
                case EvidenceObjectType.DiskImage:
                    nType = EvidenceObjectCategory.Image;
                    nDiskID = EvidenceObjectType.FileBased;
                    break;
                case EvidenceObjectType.MemoryDump:
                    nType = EvidenceObjectCategory.MemoryDump;
                    nDiskID = EvidenceObjectType.FileBased;
                    break;
                case EvidenceObjectType.Directory:
                    nType = EvidenceObjectCategory.Directory;
                    nDiskID = EvidenceObjectType.FileBased;
                    break;
                case EvidenceObjectType.File:
                    nType = EvidenceObjectCategory.File;
                    nDiskID = EvidenceObjectType.FileBased;
                    break;
                default:
                    nType = EvidenceObjectCategory.Disk;
                    break;
            }

            return ImportedMethods.XWF_CreateEvObj(nType, nDiskID, lpPath, IntPtr.Zero);
        }

        /// <summary>
        /// If not currently open, opens the specified evidence object in a data window
        /// (and at the operating system level opens the corresponding disk or image
        /// file), interprets the image file (if the evidence object is an image), loads
        /// or takes the volume snapshot and returns a handle to the volume that the
        /// evidence object represents. Use this function if you wish to read data from 
        /// the volume or process the volume snapshot. Potentially time-consuming. 
        /// Available from v17.6.
        /// </summary>
        /// <param name="hEvidence">A pointer to the evidence object.</param>
        /// <param name="nFlags">XWFOpenEvObjFlags open flags.</param>
        /// <returns>Returns a handle to the volume that the evidence object represents 
        /// or returns 0 if unsuccessful. Must be 0 in v18.0 and older.</returns>
        public static IntPtr XWF_OpenEvObj(IntPtr hEvidence, EvidenceOpenOptions nFlags)
        {
            return ImportedMethods.XWF_OpenEvObj(hEvidence, nFlags);
        }

        /// <summary>
        /// Closes the specified evidence object if it is open currently and unloads the
        /// volume snapshot, otherwise does nothing. Available from v17.6.
        /// </summary>
        /// <param name="hEvidence"></param>
        public static void XWF_CloseEvObj(IntPtr hEvidence)
        {
            ImportedMethods.XWF_CloseEvObj(hEvidence);
        }

        /// <summary>
        /// Retrieves information about the specified evidence object. Does not require
        /// that the evidence object be open.
        /// </summary>
        /// <param name="hEvidence">A pointer to the evidence object.</param>
        /// <returns>Rerturns a struct with the object's properties.</returns>
        public static EvidenceObjectProperties XWF_GetEvObjProp(IntPtr hEvidence)
        {
            EvidenceObjectProperties props = new EvidenceObjectProperties();

            // Get the object number.
            props.objectNumber = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.EvidenceObjectNumber, IntPtr.Zero);

            // Get the object ID.
            props.objectID = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.EvidenceObjectID, IntPtr.Zero);

            // Get the parent object ID.
            props.parentObjectID = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.ParentEvidenceObjectID, IntPtr.Zero);

            // Get the title.
            long tmpPtr = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.Title, IntPtr.Zero);
            props.title = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Initialize the buffer for later use.
            int bufferSize = EventObjectPropertiesLength;

            // Get the extended title.
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.ExtendedTitle, bufferPtr);
            props.extendedTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the abbreviated title.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.AbbreviatedTitle, bufferPtr);
            props.abbreviatedTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the internal name.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.InternalName, IntPtr.Zero);
            props.internalName = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the description.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.Description, IntPtr.Zero);
            props.description = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the examiner comments.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.ExaminerComments, IntPtr.Zero);
            props.examinerComments = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the internally used directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.InternallyUsedDirectory, bufferPtr);
            props.internallyUsedDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the output directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.OutputDirectory, bufferPtr);
            props.outputDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the size in bytes.
            props.SizeInBytes = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.SizeInBytes, IntPtr.Zero);

            // Get the volume snapshot file count.
            props.VolumeSnapshotFileCount = ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.VolumeSnapshotFileCount, IntPtr.Zero);

            // Get the flags.
            props.Flags = (EvidenceProperties) ImportedMethods.XWF_GetEvObjProp(
                hEvidence, EvidencePropertyType.Flags, IntPtr.Zero);

            // Get the file system identifier.
            props.FileSystemIdentifier
                = (VolumeFileSystem) ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.FileSystemIdentifier, IntPtr.Zero);

            // Get the hash type.
            props.HashType = (HashType) ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.HashType, IntPtr.Zero);

            // Get the hash value.
            if (props.HashType == HashType.Undefined)
            {
                props.HashValue = null;
            }
            else
            {
                bufferPtr = Marshal.AllocHGlobal(bufferSize);
                int hashSize = (int) ImportedMethods.XWF_GetEvObjProp(hEvidence
                    , EvidencePropertyType.HashValue, bufferPtr);
                Byte[] hash1 = new Byte[hashSize];
                Marshal.Copy(bufferPtr, hash1, 0, hashSize);
                props.HashValue = hash1;
                Marshal.FreeHGlobal(bufferPtr);
            }

            // Get the creation time.
            props.CreationTime = DateTime.FromFileTime(ImportedMethods.XWF_GetEvObjProp(
                hEvidence, EvidencePropertyType.CreationTime, IntPtr.Zero));

            // Get the modification time.
            props.ModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetEvObjProp(hEvidence
                    , EvidencePropertyType.ModificationTime, IntPtr.Zero));

            // Get the hash 2 type.
            props.HashType2 = (HashType) ImportedMethods.XWF_GetEvObjProp(hEvidence
                , EvidencePropertyType.HashType2, IntPtr.Zero);

            // Get the hash 2 value.
            if (props.HashType2 == HashType.Undefined)
            {
                props.HashValue2 = null;
            }
            else
            {
                bufferPtr = Marshal.AllocHGlobal(bufferSize);
                int hashSize = (int) ImportedMethods.XWF_GetEvObjProp(hEvidence
                    , EvidencePropertyType.HashValue2, bufferPtr);
                Byte[] hash2 = new Byte[hashSize];
                Marshal.Copy(bufferPtr, hash2, 0, hashSize);
                props.HashValue2 = hash2;
                Marshal.FreeHGlobal(bufferPtr);
            }

            /*
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            XWF_GetEvObjProp(hEvidence, XWFGetEvObjPropTypes.HashValue2, bufferPtr);
            props.HashValue2 = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);
            */

            return props;
        }

        /// <summary>
        /// If nReportTableID designates an existing report table in the current case,
        /// returns a pointer to the null-terminated name of that report table, or 
        /// otherwise NULL. nReportTableID may be set to -1 to retrieve the maximum 
        /// number of report tables supported by the active version in the integer 
        /// pointed to by lpOptional. Valid report table IDs range from 0 to (maximum
        /// number: -1).  Available in v17.7 and later.
        /// </summary>
        /// <param name="nReportTableID">An existing report table ID or -1 to get the 
        /// maximum number of report tables.</param>
        /// <param name="lpOptional">Must be 0 before v18.1. Should be 
        /// XWFGetReportTableInfoFlags in v18.1 and beyond.</param>
        /// <returns></returns>
        public static string XWF_GetReportTableInfo(long nReportTableID
            , ReportTableInformationOptions lpOptional)
        {
            try
            {
                IntPtr hReportName = ImportedMethods.XWF_GetReportTableInfo(IntPtr.Zero
                    , nReportTableID, lpOptional);
                string sReportName = Marshal.PtrToStringUni(hReportName);
                Marshal.FreeHGlobal(hReportName);

                return sReportName;
            }
            catch (System.AccessViolationException e)
            {
                XWF_OutputMessage("Exception: " + e);
                return null;
            }
        }

        /// <summary>
        /// Returns a pointer to an internal list that describes all report table
        /// associations of the specified evidence object, or NULL if unsuccessful (for
        /// example if not available any more in a future version). Scanning this list
        /// is a much quicker way to find out which items are associated with a given
        /// report table than calling GetReportTableAssocs for all items in a volume
        /// snapshot, especially if the snapshot is huge. The list consists of 16-bit
        /// report table ID and 32-bit item ID pairs repeatedly, stored back to back.
        /// </summary>
        /// <param name="hEvidence">A pointer to the evidence object.</param>
        /// <param name="nFlags">Optional. Return list flags.</param>
        /// <returns></returns>
        public static IntPtr XWF_GetEvObjReportTableAssocs(IntPtr hEvidence
            , uint nFlags = 0x01)
        {
            IntPtr lpValue;
            IntPtr associationList = ImportedMethods.XWF_GetEvObjReportTableAssocs(
                hEvidence, nFlags, out lpValue);

            return IntPtr.Zero;
        }

        /// <summary>
        /// Defines to which volume's volume snapshot subsequent calls of the below
        /// functions apply should you wish to change that.
        /// </summary>
        /// <param name="hVolume">A pointer to the specified volume.</param>
        public static bool XWF_SelectVolumeSnapshot(IntPtr hVolume)
        {
            // Check for valid provided volume pointer.
            if (hVolume != IntPtr.Zero)
            {
                ImportedMethods.XWF_SelectVolumeSnapshot(hVolume);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves information about the current volume snapshot. Available in v17.4
        /// and later.
        /// </summary>
        /// <param name="nPropType">Property type (XWFGetVSPropType).</param>
        /// <param name="specialItemType">Optional. Special item type. Only required
        /// when nPropType is XWFGetVSPropType.SpecialItemID.</param>
        /// <returns></returns>
        public static long XWF_GetVSProp(VolumeSnapshotPropertyType nPropType
            , SpecialItemType specialItemType = SpecialItemType.Ununsed)
        {
            return ImportedMethods.XWF_GetVSProp(nPropType, specialItemType);
        }

        public static VolumeSnapshotProperties XWF_GetVSProps()
        {
            VolumeSnapshotProperties props = new VolumeSnapshotProperties();

            props.rootDirectory = XWF_GetVSProp(VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.RootDirectory);
            props.pathUnknownDirectory = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.PathUnknownDirectory);
            props.carvedFilesDirectory = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.CarvedFilesDirectory);
            props.freeSpaceFile = XWF_GetVSProp(VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.FreeSpaceFile);
            props.systemVolumeInformationDirectory = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.SystemVolumeInformationDirectory);
            props.windowsEDBFile = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.WindowsEDBFile);
            props.hashType1 = (HashType)XWF_GetVSProp
                (VolumeSnapshotPropertyType.HashType1
                , SpecialItemType.Ununsed);
            props.hashType2 = (HashType)XWF_GetVSProp(
                VolumeSnapshotPropertyType.HashType2
                , SpecialItemType.Ununsed);

            return props;
        }

        /// <summary>
        /// Retrieves the number of items in the current volume snapshot (both files and
        /// directories). Item IDs are consecutive 0-based, meaning the ID of the first
        /// item in the volume snapshot is 0 and the last item is GetItemCount-1. You
        /// address each and every item in that range, be it a file or directory, by 
        /// specifying its ID.
        /// </summary>
        /// <returns>Returns the number of file and directories in the curent volume
        /// snapshot.</returns>
        public static uint XWF_GetItemCount()
        {
            return ImportedMethods.XWF_GetItemCount(IntPtr.Zero);
        }

        /// <summary>
        /// Retrieves the accumulated number of files in the directory with the 
        /// specified ID and all its subdirectories. Also works for files that have
        /// child objects. Not currently supported for the root directory though you may
        /// specify -1 as the ID to get the total file count of the entire volume.
        /// Available from v17.7.
        /// </summary>
        /// <param name="nDirID">The directory ID.</param>
        /// <returns>Returns the accumulated number of all files under a provided
        /// directory or file and all it's subdirectories</returns>
        public static uint XWF_GetFileCount(uint nDirID)
        {
            return ImportedMethods.XWF_GetFileCount(nDirID);
        }

        /// <summary>
        /// Creates a new item (file or directory) in the volume snapshot. May be called
        /// when refining the volume snapshot. Should be followed by calls 
        /// to XWF_SetItemParent, XWF_SetItemSize, XWF_SetItemInformation, and/or 
        /// XWF_SetItemOfs. If via XWF_SetItemParent, you make the new file a child
        /// object of a file (not directory), you are responsible for setting the 
        /// parent's XWF_ITEM_INFO_FLAG_HASCHILDREN flag. For example, if you are
        /// creating a file carved from the sectors of the evidence object, you can
        /// specify the file size using XWF_SetItemSize and the start offset via the
        /// nDefOfs parameter (must be negative) using XWF_SetItemOfs.
        /// </summary>
        /// <param name="lpName">The name of the item.</param>
        /// <param name="flags">Creation flags (XWFCreateItemFlags).</param>
        /// <returns>Returns the ID of the newly created item, or -1 if an error
        /// occurred (e.g. out of memory).</returns>
        public static int XWF_CreateItem(
            [MarshalAs(UnmanagedType.LPWStr)] string lpName, CreateItemOptions flags)
        {
            return ImportedMethods.XWF_CreateItem(lpName, flags);
        }

        /// <summary>
        /// Similar to XWF_CreateItem, but also allows attachment of an external file to
        /// the volume snapshot or to define a file that is an excerpt of another file
        /// (its parent). Returns the ID of the newly created item, or -1 if an error 
        /// occurred (e.g. out of memory). Should be followed by a call to 
        /// XWF_SetItemSize (not necessary if attaching an external file) or 
        /// XWF_SetItemInformation (not necessary when carving a file in a file).
        /// Available from v16.7.
        /// </summary>
        /// <param name="lpName">The name that this file will have in the volume 
        /// snapshot, which may be different from its source file name if you are
        /// attaching an external file.</param>
        /// <param name="flags">Creation flags (XWFCreateFileFlags).</param>
        /// <param name="nParentItemID">The file's parent ID, if needed.</param>
        /// <param name="pSourceInfo">More information about the source of the file's
        /// data. The exact meaning depends on the flags.</param>
        /// <returns></returns>
        public static int XWF_CreateFile(
            [MarshalAs(UnmanagedType.LPWStr)] string lpName, CreateFileOptions flags
            , int nParentItemID, IntPtr pSourceInfo)
        {
            return ImportedMethods.XWF_CreateFile(lpName, flags, nParentItemID
                , pSourceInfo);
        }

        /// <summary>
        /// TODO: Implement get_path helper function.
        /// Retrieves a pointer to the null-terminated name of the specified item (file
        /// or directory) in UTF-16. You may call XWF_GetItemName and XWF_GetItemParent
        /// repeatedly until you reach the root directory and concatenate the results to
        /// get the full path of an item.
        /// </summary>
        /// <param name="nItemID">The item ID.</param>
        /// <returns>Returns name of the item.</returns>
        public static string XWF_GetItemName(int itemID)
        {
            string result;

            try
            {
                IntPtr temp = ImportedMethods.XWF_GetItemName(itemID);
                result = Marshal.PtrToStringUni(temp);
            }
            catch (Exception e)
            {
                XWF_OutputMessage("Exception: " + e);
                return null;
            }

            return result;
        }

        /// <summary>
        /// Retrieves the size of the item (file or directory) in bytes, or -1 when the
        /// size is unknown.
        /// </summary>
        /// <param name="nItemID">The item ID.</param>
        /// <returns>Returns the size of the item, or -1 if unknown.</returns>
        public static long XWF_GetItemSize(long itemID)
        {
            return ImportedMethods.XWF_GetItemSize(itemID);
        }

        /// <summary>
        /// Sets the size of the item in bytes, using -1 when the size is unknown.
        /// </summary>
        /// <param name="nItemID">The item ID.</param>
        /// <param name="size">The size of the item, or -1 if unknown.</param>
        public static void XWF_SetItemSize(int nItemID, int size)
        {
            ImportedMethods.XWF_SetItemSize(nItemID, size);
        }

        /// <summary>
        /// Retrieves the offset of the file system data structure (e.g. NTFS FILE 
        /// record) where the item is defined. If negative, the absolute value is the 
        /// offset where a carved file starts on the volume. 0 if an error occurred. 
        /// 0xFFFFFFFF if not available/not applicable. Also retrieves the number of the 
        /// sector from the point of the volume in which the data of the item starts.
        /// </summary>
        /// <param name="nItemID">The item ID.</param>
        /// <returns>Returns XWFItemOffsets struct with the relative offsets.</returns>
        public static ItemOffsets XWF_GetItemOfs(int ItemID)
        {
            long itemOffset, startSector;
            ItemOffsets itemOffsets = new ItemOffsets();
      
            ImportedMethods.XWF_GetItemOfs(ItemID, out itemOffset, out startSector);

            if (itemOffset >= 0)
            {
                itemOffsets.FileSystemDataStructureOffset = itemOffset;
                itemOffsets.CarvedFileVolumeOffset = -1;
            }
            else
            {
                itemOffsets.FileSystemDataStructureOffset = -1;
                itemOffsets.CarvedFileVolumeOffset = Math.Abs(itemOffset);
            }

            itemOffsets.DataStartSector = startSector;

            return itemOffsets;
        }

        /// <summary>
        /// Sets the offset and data sector start of a given item.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <param name="ItemOffsets">A ItemOffsets struct with the offsets to use.
        /// </param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        public static bool XWF_SetItemOfs(int ItemID, ItemOffsets ItemOffsets)
        {
            long itemOffset;

            if (ItemOffsets.FileSystemDataStructureOffset != -1)
            {
                itemOffset = ItemOffsets.FileSystemDataStructureOffset;
            }
            else if (ItemOffsets.CarvedFileVolumeOffset != -1)
            {
                itemOffset = ItemOffsets.CarvedFileVolumeOffset * -1;
            }
            else
            {
                return false;
            }

            ImportedMethods.XWF_SetItemOfs(ItemID, itemOffset
                , ItemOffsets.DataStartSector);

            return true;
        }

        /// <summary>
        /// Returns information about an item (file or directory) as stored in the 
        /// volume snapshot, such as the original ID or attributes that the item had in 
        /// its defining file system.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <returns>Returns ItemInformation struct with the given item's information.
        /// </returns>
        public static ItemInformation XWF_GetItemInformation(int ItemID)
        {
            ItemInformation info = new ItemInformation();
            bool bStatus;

            info.OriginalItemID = ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_ORIG_ID, out bStatus);

            info.Attributes = ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_ATTR, out bStatus);

            info.Flags = (ItemInformationOptions) 
                ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_FLAGS, out bStatus);

            info.Deletion = (ItemDeletionStatus) 
                ImportedMethods.XWF_GetItemInformation(ItemID
                    , ItemInformationType.XWF_ITEM_INFO_DELETION, out bStatus);

            info.Classification = (ItemClassifiction) 
                ImportedMethods.XWF_GetItemInformation(ItemID
                    , ItemInformationType.XWF_ITEM_INFO_CLASSIFICATION, out bStatus);

            info.LinkCount = ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_LINKCOUNT, out bStatus);

            info.ColorAnalysis = ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_COLORANALYSIS, out bStatus);

            info.FileCount = ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_FILECOUNT, out bStatus);

            info.EmbeddedOffset = ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_EMBEDDEDOFFSET, out bStatus);

            info.CreationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_CREATIONTIME, out bStatus));

            info.ModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_MODIFICATIONTIME, out bStatus));

            info.LastAccessTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_LASTACCESSTIME, out bStatus));

            info.EntryModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_ENTRYMODIFICATIONTIME
                , out bStatus));

            info.DeletionTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_DELETIONTIME, out bStatus));

            info.InternalCreationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(ItemID
                , ItemInformationType.XWF_ITEM_INFO_INTERNALCREATIONTIME
                , out bStatus));

            return info;
        }

        public static bool XWF_SetItemInformation(int ItemID
            , ItemInformationType nInfoType, long nInfoValue)
        {
            return false;
        }

        /*
        public static XWFItemType XWF_GetItemType(int ItemID)
        {
            return ImportedMethods.XWF_GetItemType(ItemID, IntPtr.Zero, 0);
        }
        */

        /// <summary>
        /// Retrieves a textual description of the type of the specified file and 
        /// returns information about the status of the type detection of the file: 
        /// 0 = not verified, 1 = too small, 2 = totally unknown, 3 = confirmed, 
        /// 4 = not confirmed, 5 = newly identified, 6 (v18.8 and later only) = mismatch
        /// detected. ­1 means error.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns>Returns a ItemType structure with the file type and description.
        /// </returns>
        public static ItemType XWF_GetItemType(int itemID)
        {
            ItemType Results = new ItemType();

            // Allocate a buffer to receive the type description.
            IntPtr bufferPtr = Marshal.AllocHGlobal(ItemTypeDescriptionBufferLength);

            // Get the results from the API function, including the type description.
            Results.Type = ImportedMethods.XWF_GetItemType(itemID, bufferPtr
                , ItemTypeDescriptionBufferLength);
            Results.Description = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            return Results;
        }

        public static bool XWF_SetItemType(int nItemID, string sTypeDescr
            , ItemTypeCategory nItemType)
        {
            try
            {
                ImportedMethods.XWF_SetItemType(nItemID, sTypeDescr, nItemType);
            }
            catch (Exception e)
            {
                XWF_OutputMessage("Exception: " + e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the ID of the parent of the specified item, or -1 if the item is the 
        /// root directory or if for some strange reason no parent object is assigned.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <returns>Returns the parent ID of the given item, or -1 if there is none.
        /// </returns>
        public static int XWF_GetItemParent(int ItemID)
        {
            return ImportedMethods.XWF_GetItemParent(ItemID);
        }

        /// <summary>
        /// Sets the parent (by the given ID) of the given child item.
        /// </summary>
        /// <param name="ChildItemID">The child ID.</param>
        /// <param name="ParentItemID">The parent ID.</param>
        /// <returns>Return true is successful, otherwise false.</returns>
        public static bool XWF_SetItemParent(int ChildItemID, int ParentItemID)
        {
            try
            {
                ImportedMethods.XWF_SetItemParent(ChildItemID, ParentItemID);
            }
            catch (Exception e)
            {
                XWF_OutputMessage("Exception: " + e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieves the names of the report tables that the specified item is 
        /// associated with. The names are delimited with a comma and space. If the 
        /// buffer was filled completely, that likely means the specified buffer length 
        /// was insufficient. In v17.6 SR-7 and later, returns the total number of
        /// associations of that file, and lpBuffer may be NULL.
        /// </summary>
        /// <param name="ItemID">The ID of the provided item.</param>
        /// associated with.</param>
        /// <returns>Returns the number of associations of the given item.</returns>
        public static string[] XWF_GetReportTableAssocs(int itemID)
        {
            const int BufferLengthStep = 128;
            string Associations;

            //
            for (int bufferLength = BufferLengthStep;; bufferLength += BufferLengthStep)
            {
                // Allocate a buffer to receive the associations.
                IntPtr Buffer = Marshal.AllocHGlobal(bufferLength);

                // Get the results from the API function, including the associations up
                // the specified buffer length.
                int AssociationsCount = ImportedMethods.XWF_GetReportTableAssocs(itemID
                    , Buffer, bufferLength);

                // If no associations, empty the associations string and return.
                if (AssociationsCount <= 0)
                {
                    return new string[0];
                }

                // Get a string representation of the associations buffer.
                string Str = Marshal.PtrToStringUni(Buffer, bufferLength);
                Marshal.FreeHGlobal(Buffer);

                // Check for a NULL character and continue in the loop if not found
                int NullCharacterIndex = Str.IndexOf((char) 0);
                if (NullCharacterIndex < 0 || NullCharacterIndex >= bufferLength - 1)
                    continue;

                Associations = Str.Substring(0, NullCharacterIndex);

                return Associations.Split(new string[] { ", " }
                    , StringSplitOptions.None);
            }
        }

        public static int XWF_AddToReportTable(int ItemID, string ReportTableName
            , int Flags)
        {
            return 0;
        }

        /// <summary>
        /// Gets the comment (if any) of the given item.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <returns>Returns the comment.</returns>
        public static string XWF_GetComment(int ItemID)
        {
            string result;

            try
            {
                IntPtr temp = ImportedMethods.XWF_GetComment(ItemID);
                result = Marshal.PtrToStringUni(temp);
            }
            catch (Exception e)
            {
                XWF_OutputMessage("Exception: " + e);
                return null;
            }

            return result;
        }

        /// <summary>
        /// Sets the comment of the given item.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <param name="Comment">The comment.</param>
        /// <param name="FlagsHowToAdd">Flags indicating how the comment should be 
        /// added.</param>
        /// <returns>Returns true if successfull, otherwise false.</returns>
        public static bool XWF_AddComment(int ItemID, string Comment
            , AddCommentMode FlagsHowToAdd)
        {
            return ImportedMethods.XWF_AddComment(ItemID, Comment, FlagsHowToAdd);
        }

        /// <summary>
        /// Get the previously extracted metadata of a given item.  Good to use this 
        /// one if metadata has already been extracted from items.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <returns>Returns the previously extracted metadata.</returns>
        public static string XWF_GetExtractedMetadata(int ItemID)
        {
            string result;

            try
            {
                IntPtr temp = ImportedMethods.XWF_GetExtractedMetadata(ItemID);
                result = Marshal.PtrToStringUni(temp);
            }
            catch (Exception e)
            {
                XWF_OutputMessage("Exception: " + e);
                return null;
            }

            return result;
        }

        public static bool XWF_AddExtractedMetadata(int nItemID, string sText
            , AddCommentMode nFlagsHowToAdd)
        {
            return ImportedMethods.XWF_AddExtractedMetadata(nItemID, sText
                , nFlagsHowToAdd);
        }

        public static string XWF_GetHashValue(int nItemID)
        {
            return "";
        }

        public static string XWF_GetMetadata(int nItemID)
        {
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SInfo"></param>
        /// <returns></returns>
        public static int XWFSearchWithoutCodePages(ref SearchInformation SInfo)
        {
            return ImportedMethods.XWF_SearchWithPtrToPages(ref SInfo, IntPtr.Zero);
        }

        public static void XWF_OutputMessage(
            [MarshalAs(UnmanagedType.LPWStr)] string lpMessage
            , OutputMessageLevel level = OutputMessageLevel.Level1
            , OutputMessageOptions nFlags = OutputMessageOptions.None)
        {
            string tab = new string(' ', (int)level * 4);
            ImportedMethods.XWF_OutputMessage(tab + lpMessage, nFlags);
        }

        public static void XWF_OutputEmptyLine()
        {
            XWF_OutputMessage("");
        }

        public static void XWF_OutputHeader(
            [MarshalAs(UnmanagedType.LPWStr)] string lpMessage
            , OutputMessageLevel level = OutputMessageLevel.Level1)
        {
            XWF_OutputMessage(lpMessage, level);
            XWF_OutputMessage("");
        }

        public static string Hexlify(byte [] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        public static ArrayList GetCaseEvidence()
        {
            ArrayList evidence = new ArrayList();

            IntPtr hCurrent = XWF_GetFirstEvObj();
            
            while(hCurrent != IntPtr.Zero)
            {
                evidence.Add(hCurrent);
                hCurrent = XWF_GetNextEvObj(hCurrent);
            }

            return evidence;
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hItem"></param>
        /// <returns></returns>
        public static byte[] ReadItem(IntPtr hItem)
        {
            //If successful - returns contents of the item as a byte array,
            //if failed - returns null.

            //if (ImportedMethods.XWFGetSize != null && ImportedMethods.XWFRead != null)
            //if (ImportedMethods.XWF_Read != null)
            try
            {
                // Get the size of the provided item.
                long size = XWF_GetSize(hItem, ItemSizeType.PhysicalSize);

                // Initialize and create a pointer to the buffer.
                int bufferSize = (int)size;
                IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);

                // Call XWF_Read to read the item into the buffer.
                return XWF_Read(hItem, 0, (uint) bufferSize);
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static SearchInformation CreateSearchInfo(string searchTerms
            , SearchInformationOptions flags)
        {
            SearchInformation info = new SearchInformation
            {
                  hVolume = IntPtr.Zero //the docs say that hVolume should be 0
                , lpSearchTerms = searchTerms
                , nFlags = flags
                , nSearchWindow = 0
            };

            info.iSize = Marshal.SizeOf(info);
            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static string GetFullPath(int itemId)
        {
            /*
            from the docs:
            
            XWF_GetItemParent returns the ID of the parent of the specified item,
            or -1 if the item is the root directory.                         
            */
            
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int parentItemId = HelperMethods.XWF_GetItemParent(itemId);

                /*
                XWFGetItemName returns text "(Root directory)" for the root directory.
                I don't see any sense in putting such kind of a string into the path,
                so, if (parentItemId < 0) then this is a root directory
                and we don't need it's name to be added.
                */
                if (parentItemId < 0) return sb.ToString();

                sb.Insert(0, Path.DirectorySeparatorChar
                    + XWF_GetItemName(itemId));

                itemId = parentItemId;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="externalFilename"></param>
        /// <param name="parentItemId"></param>
        /// <param name="keepExternalFile"></param>
        /// <returns></returns>
        public static int CreateFileFromExternalFile(string name
            , string externalFilename
            , int parentItemId
            , bool keepExternalFile = false)
        {
            IntPtr extFilenamePtr = Marshal.StringToHGlobalUni(externalFilename);       

            int itemId = XWF_CreateFile(name
                , CreateFileOptions.AttachExternalFile
                    | (keepExternalFile ? CreateFileOptions.KeepExternalFile : 0)
                , parentItemId
                , extFilenamePtr);

            Marshal.FreeHGlobal(extFilenamePtr);
            return itemId;
        }
    }
}
