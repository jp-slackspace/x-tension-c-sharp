using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace XTensions
{
    public static class HelperMethods
    {
        private static readonly int _volumeNameBufferLength = 256 * 2;
        private static readonly int _sectorContentsBufferLength = 512 * 2;
        private static readonly int _itemTypeDescriptionBufferLength = 1024 * 2;
        private static readonly int _casePropertiesLength = 1024 * 2;
        private static readonly int _eventObjectPropertiesLength = 128 * 2;

        /// <summary>
        /// Retrieve the size of the provided volume or file.
        /// </summary>
        /// <param name="volumeOrItem">A pointer to a volume or item.</param>
        /// <param name="sizeType">The type (XWFGetSizeType) of size to retrieve.
        /// </param>
        /// <returns>Returns the size of the volume or file.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static long XWF_GetSize(IntPtr volumeOrItem, ItemSizeType sizeType
            = ItemSizeType.PhysicalSize)
        {
            // Fail if a volume or item pointer weren't provided.
            if (volumeOrItem == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            return ImportedMethods.XWF_GetSize(volumeOrItem, sizeType);
        }

        /// <summary>
        /// Retrieves the UTF-16 name of the provided volume, using 255 characters at 
        /// most.
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="volumeNameType">The volume name type (XWFVolumeNameType) to
        /// return.</param>
        /// <returns>Returns the volume name in the type specified.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static string XWF_GetVolumeName(IntPtr volume
            , VolumeNameType volumeNameType = VolumeNameType.Type1)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Allocate a buffer to receive the volume name, call the API function and
            // get volume name, and clean up.
            IntPtr Buffer = Marshal.AllocHGlobal(_volumeNameBufferLength);
            ImportedMethods.XWF_GetVolumeName(volume, Buffer, volumeNameType);
            string str = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            return str;
        }

        /// <summary>
        /// Retrieves various information about a given volume.
        /// </summary>
        /// <param name="hVolume">A pointer to a volume.</param>
        /// <returns>Returns the volume information in a VolumeInformation struct.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static VolumeInformation XWF_GetVolumeInformation(IntPtr volume)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            VolumeInformation info = new VolumeInformation();

            // Call the API function, getting the volume's information.
            ImportedMethods.XWF_GetVolumeInformation(volume, out info.FileSystem
                , out info.BytesPerSector, out info.SectorsPerCluster
                , out info.ClusterCount, out info.FirstClusterSectorNumber);

            return info;
        }

        /// <summary>
        /// Retrieves the boundaries of the currently selected block, if any.
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <returns>Returns the block boundaries in a BlockBoundaries struct.  These 
        /// boundaries will be 0 and -1 respectively if no block is selected.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static BlockBoundaries XWF_GetBlock(IntPtr volume)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            BlockBoundaries boundaries = new BlockBoundaries();

            // Call the API function, getting the block's boundaries.
            bool result = ImportedMethods.XWF_GetBlock(volume
                , out boundaries.StartOffset, out boundaries.EndOffset);

            return boundaries;
        }

        /// <summary>
        /// Set the boundaries (provided as a BlockBoundaries struct) of a new block.
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="boundaries">A BlockBoundaries struct indicating the starting
        /// and ending offsets of the new block.</param>
        /// <returns>Returns True if successful and False if the provided boundaries 
        /// exceed the size of the volume.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static bool XWF_SetBlock(IntPtr volume, BlockBoundaries boundaries)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Return results of API call to set the block.
            return ImportedMethods.XWF_SetBlock(volume, boundaries.StartOffset
                , boundaries.EndOffset);
        }

        /// <summary>
        /// Clear an existing block.
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static bool XWF_ClearBlock(IntPtr volume)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Return results of API call to clear the block.
            return ImportedMethods.XWF_SetBlock(volume, 0
                , -1);
        }

        /// <summary>
        /// Retrieves information about a provided sector.
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="sectorNumber">The sector number.</param>
        /// <returns>Returns the sector information in a SectorInformation struct.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static SectorInformation XWF_GetSectorContents(IntPtr volume
            , long sectorNumber)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            SectorInformation SectorInformation;

            // Allocate a buffer to receive the sector contents description.
            IntPtr Buffer = Marshal.AllocHGlobal(_sectorContentsBufferLength);

            // Get the results from the sector information API call and clean up.
            SectorInformation.IsAllocated = ImportedMethods.XWF_GetSectorContents(volume
                , sectorNumber, Buffer, out SectorInformation.OwnerItemID);

            // The state of the buffer determines whether there is a description.
            SectorInformation.Description = (Buffer != IntPtr.Zero)
                ? Marshal.PtrToStringUni(Buffer) : null;
            Marshal.FreeHGlobal(Buffer);

            return SectorInformation;
        }

        /// <summary>
        /// Opens the provided item for reading. Available in v16.5 and later.
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="itemId">The item's Id.</param>
        /// <param name="options">Options for opening, using ItemOpenModes flag.</param>
        /// <returns>Returns a handle to the open item, or IntPtr.Zero if unsuccessful
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static IntPtr XWF_OpenItem(IntPtr volume, long itemId
            , ItemOpenModes options = ItemOpenModes.LogicalContents)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            try
            {
                return ImportedMethods.XWF_OpenItem(volume, itemId, options);
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
        /// <param name="volumeOrItem">An open volume or item.</param>
        /// <returns>Returns true if a successfull, otherwise false.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static bool XWF_Close(IntPtr volumeOrItem)
        {
            // Fail if a volume or item pointer weren't provided.
            if (volumeOrItem == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            ImportedMethods.XWF_Close(volumeOrItem);
            return true;
        }

        /// <summary>
        /// Reads the specified number of bytes from a specified position in a specified 
        /// item.
        /// </summary>
        /// <param name="volumeOrItem">A pointer to a volume or item.</param>
        /// <param name="offset">The offset to read from.</param>
        /// <param name="numberOfBytesToRead">The number of bytes to read.</param>
        /// <returns>Returns a byte array of the bytes read.</returns>
        /// <remarks>Version 1.0 coding complete. 
        /// - Todo: Does XWF_Read really need to use a DWORD (uint) for number of bytes to 
        /// read?</remarks>
        public static byte[] XWF_Read(IntPtr volumeOrItem, long offset = 0
            , int numberOfBytesToRead = 0)
        {
            // Fail if a volume or item pointer weren't provided.
            if (volumeOrItem == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Read the full file from the provided offset if the provided number of 
            // bytes to read is 0.
            if (numberOfBytesToRead == 0)
            {
                numberOfBytesToRead = (int)(XWF_GetSize(volumeOrItem
                    , ItemSizeType.PhysicalSize) - offset);
            }

            // Initialize and create a pointer to the buffer.
            IntPtr Buffer = Marshal.AllocHGlobal(numberOfBytesToRead);

            // Call XWF_Read to read the item's data into the buffer.
            ImportedMethods.XWF_Read(volumeOrItem, offset, Buffer
                , (uint)numberOfBytesToRead);

            // Copy the buffer contents into a byte array and cleanup the buffer.
            byte[] contents = new byte[numberOfBytesToRead];
            Marshal.Copy(Buffer, contents, 0, numberOfBytesToRead);
            Marshal.FreeHGlobal(Buffer);

            return contents;
        }

        /// <summary>
        /// Retrieves information about the current case.
        /// </summary>
        /// <returns>Returns a CaseProperties structure.</returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Need to handle when -1 is returned from API call; indicating that no case
        /// is loaded.</remarks>
        public static CaseProperties XWF_GetCaseProps()
        {
            CaseProperties props = new CaseProperties();

            // Read the title.
            int bufferSize = (int)_casePropertiesLength;
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero, (int)CasePropertyType.CaseTitle
                , bufferPtr, bufferSize);
            props.CaseTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Read the examiner.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseExaminer, bufferPtr, bufferSize);
            props.CaseExaminer = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Read the case file path.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseFilePath, bufferPtr, bufferSize);
            props.CaseFilePath = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Read the case directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseDirectory, bufferPtr, bufferSize);
            props.CaseDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            return props;
        }

        /// <summary>
        /// Retrieves a handle to the first evidence object in the case. In conjunction
        /// with XWF_GetNextEvObj this function allows to enumerate all evidence objects
        /// of the case. Available from v17.6.
        /// </summary>
        /// <returns>Returns a pointer to the first evidence objector, or NULL if the 
        /// active case has no evidence objects or (in releases from June 2016) if no 
        /// case is active.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static IntPtr XWF_GetFirstEvObj()
        {
            return ImportedMethods.XWF_GetFirstEvObj(IntPtr.Zero);
        }

        /// <summary>
        /// Given a pointer to the previous evidence object, retrieves the next evidence
        /// object in the chain. Available from v17.6.
        /// </summary>
        /// <param name="hPrevEvidence">Previous evidence object.</param>
        /// <returns>Returns a pointer to the next evidence object.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static IntPtr XWF_GetNextEvObj(IntPtr PreviousEvidence)
        {
            // Handle case where zero pointer is provided.
            if (PreviousEvidence == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            return ImportedMethods.XWF_GetNextEvObj(PreviousEvidence, IntPtr.Zero);
        }

        /*
        /// <summary>
        /// NOT CURRENTLY IMPLEMENTED. Removes the specified evidence object from the 
        /// case. 
        /// </summary>
        /// <param name="EvidenceObject">Evidence object to be deleted.</param>
        /// <returns></returns>
        public static IntPtr XWF_DeleteEvObj(IntPtr EvidenceObject)
        {
            return IntPtr.Zero;
        }
        */

        /// <summary>
        /// Creates one or more evidence objects from one source (which can be a medium,
        /// disk/volume image, memory dump, or a directory/path). A case must already be 
        /// loaded. If more than 1 evidence object is created (for example for a physical 
        /// disk that contains partitions, which count as evidence objects themselves), 
        /// use XWF_GetNextEvObj to find them. Available in v16.5 and
        /// later.
        /// </summary>
        /// <param name="EvidenceType">The evidence object type.</param>
        /// <param name="objectPath">Path in case of a file or directory, otherwise NULL.
        /// </param>
        /// <returns>Returns the first evidence object created, or NULL in case of an
        /// error.</returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Not sure a marshalled type is needed in the parameters.</remarks>
        public static IntPtr XWF_CreateEvObj(EvidenceObjectType evidenceType
            , [MarshalAs(UnmanagedType.LPWStr)] string objectPath = null)
        {
            // Make sure a path was provided if one is expected.
            if ((evidenceType == EvidenceObjectType.DiskImage
                || evidenceType == EvidenceObjectType.MemoryDump
                || evidenceType == EvidenceObjectType.Directory
                || evidenceType == EvidenceObjectType.File) && objectPath == null)
            {
                return IntPtr.Zero;
            }

            EvidenceObjectCategory EvidenceType;

            // Determine the type based on the disk ID provided.
            switch (evidenceType)
            {
                case EvidenceObjectType.DiskImage:
                    EvidenceType = EvidenceObjectCategory.Image;
                    evidenceType = EvidenceObjectType.FileBased;
                    break;
                case EvidenceObjectType.MemoryDump:
                    EvidenceType = EvidenceObjectCategory.MemoryDump;
                    evidenceType = EvidenceObjectType.FileBased;
                    break;
                case EvidenceObjectType.Directory:
                    EvidenceType = EvidenceObjectCategory.Directory;
                    evidenceType = EvidenceObjectType.FileBased;
                    break;
                case EvidenceObjectType.File:
                    EvidenceType = EvidenceObjectCategory.File;
                    evidenceType = EvidenceObjectType.FileBased;
                    break;
                default:
                    EvidenceType = EvidenceObjectCategory.Disk;
                    break;
            }

            return ImportedMethods.XWF_CreateEvObj(EvidenceType, evidenceType, objectPath
                , IntPtr.Zero);
        }

        /// <summary>
        /// If not currently open, opens the specified evidence object in a data window
        /// (and at the operating system level opens the corresponding disk or image
        /// file), interprets the image file (if the evidence object is an image), loads
        /// or takes the volume snapshot and returns a handle to the volume that the
        /// evidence object represents. Use this function if you wish to read data from 
        /// the volume or process the volume snapshot. Potentially time-consuming. 
        /// Available from v17.6. Options must be EvidenceOpenOptions.None in v18.0 and 
        /// older.
        /// </summary>
        /// <param name="evidence">A pointer to the evidence object.</param>
        /// <param name="options">EvidenceOpenOptions open options.</param>
        /// <returns>Returns a handle to the volume that the evidence object represents 
        /// or returns 0 if unsuccessful.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static IntPtr XWF_OpenEvObj(IntPtr evidence, EvidenceOpenOptions options)
        {
            return ImportedMethods.XWF_OpenEvObj(evidence, options);
        }

        /// <summary>
        /// Closes the specified evidence object if it is open currently and unloads the
        /// volume snapshot, otherwise does nothing. Available from v17.6.
        /// </summary>
        /// <param name="evidence">The evidence object to close.</param>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static void XWF_CloseEvObj(IntPtr evidence)
        {
            ImportedMethods.XWF_CloseEvObj(evidence);
        }

        /// <summary>
        /// Retrieves information about the specified evidence object. Does not require
        /// that the evidence object be open. Available from v17.6.
        /// </summary>
        /// <param name="evidence">A pointer to the evidence object.</param>
        /// <returns>Returns a EvidenceObjectProperites struct.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static EvidenceObjectProperties XWF_GetEvObjProp(IntPtr evidence)
        {
            // Fail if an evidence pointer wasn't provided.
            if (evidence == IntPtr.Zero) throw new ArgumentException(
                "Zero evidence pointer provided.");

            EvidenceObjectProperties props = new EvidenceObjectProperties();

            // Get the object number.
            props.objectNumber = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.EvidenceObjectNumber, IntPtr.Zero);

            // Get the object ID.
            props.objectID = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.EvidenceObjectID, IntPtr.Zero);

            // Get the parent object ID.
            props.parentObjectID = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.ParentEvidenceObjectID, IntPtr.Zero);

            // Get the title.
            long tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.Title, IntPtr.Zero);
            props.title = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Initialize the buffer for later use.
            int bufferSize = _eventObjectPropertiesLength;

            // Get the extended title.
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.ExtendedTitle, bufferPtr);
            props.extendedTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the abbreviated title.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.AbbreviatedTitle, bufferPtr);
            props.abbreviatedTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the internal name.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.InternalName, IntPtr.Zero);
            props.internalName = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the description.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.Description, IntPtr.Zero);
            props.description = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the examiner comments.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.ExaminerComments, IntPtr.Zero);
            props.examinerComments = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the internally used directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.InternallyUsedDirectory, bufferPtr);
            props.internallyUsedDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the output directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.OutputDirectory, bufferPtr);
            props.outputDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the size in bytes.
            props.SizeInBytes = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.SizeInBytes, IntPtr.Zero);

            // Get the volume snapshot file count.
            props.VolumeSnapshotFileCount = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.VolumeSnapshotFileCount, IntPtr.Zero);

            // Get the flags.
            props.Flags = (EvidenceProperties)ImportedMethods.XWF_GetEvObjProp(
                evidence, EvidencePropertyType.Flags, IntPtr.Zero);

            // Get the file system identifier.
            props.FileSystemIdentifier
                = (VolumeFileSystem)ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.FileSystemIdentifier, IntPtr.Zero);

            // Get the hash type.
            props.HashType = (HashType)ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.HashType, IntPtr.Zero);

            // Get the hash value.
            if (props.HashType == HashType.Undefined)
            {
                props.HashValue = null;
            }
            else
            {
                bufferPtr = Marshal.AllocHGlobal(bufferSize);
                int hashSize = (int)ImportedMethods.XWF_GetEvObjProp(evidence
                    , EvidencePropertyType.HashValue, bufferPtr);
                Byte[] hash1 = new Byte[hashSize];
                Marshal.Copy(bufferPtr, hash1, 0, hashSize);
                props.HashValue = hash1;
                Marshal.FreeHGlobal(bufferPtr);
            }

            // Get the creation time.
            props.CreationTime = DateTime.FromFileTime(ImportedMethods.XWF_GetEvObjProp(
                evidence, EvidencePropertyType.CreationTime, IntPtr.Zero));

            // Get the modification time.
            props.ModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetEvObjProp(evidence
                    , EvidencePropertyType.ModificationTime, IntPtr.Zero));

            // Get the hash 2 type.
            props.HashType2 = (HashType)ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.HashType2, IntPtr.Zero);

            // Get the hash 2 value.
            if (props.HashType2 == HashType.Undefined)
            {
                props.HashValue2 = null;
            }
            else
            {
                bufferPtr = Marshal.AllocHGlobal(bufferSize);
                int hashSize = (int)ImportedMethods.XWF_GetEvObjProp(evidence
                    , EvidencePropertyType.HashValue2, bufferPtr);
                Byte[] hash2 = new Byte[hashSize];
                Marshal.Copy(bufferPtr, hash2, 0, hashSize);
                props.HashValue2 = hash2;
                Marshal.FreeHGlobal(bufferPtr);
            }

            return props;
        }

        /// <summary>
        /// Retrieves a handle to the evidence object with the specified unique ID. The 
        /// unique ID of an evidence object remains the same after closing and re-opening 
        /// a case, whereas the handle will likely change. The evidence object number may 
        /// also change. That happens if the user re-orders the evidence objects in the 
        /// case. The unique ID, however, is guaranteed to never change and also 
        /// guaranteed to be unique within the case (actually likely unique even across 
        /// all the cases that the user will even deal with) and can be used to reliably 
        /// recognize a known evidence object. Available from v18.7.
        /// </summary>
        /// <param name="evidenceObjectId"></param>
        /// <returns>Returns a pointer to the evidence object corresponding to the 
        /// specified evidence object Id or NULL if not found.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        private static IntPtr XWF_GetEvObj(uint evidenceObjectId)
        {
            return ImportedMethods.XWF_GetEvObj(evidenceObjectId);
        }

        /// <summary>
        /// Gets the name of a report table, null if none, or the maximum number of
        /// report table names if reportTableId is set to -1. Valid report table IDs 
        /// range from 0 to (maximum number: -1).  Available in v17.7 and later.
        /// </summary>
        /// <param name="reportTableId">An existing report table ID or -1 to get the 
        /// maximum number of report tables.</param>
        /// <param name="informationOptions">ReportTableInformationOptions options.
        /// Should be ReportTableInformationOptions.None before v18.1.</param>
        /// <returns>Returns the name of a given report table or null if none.</returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Right now catching exceptions; need to figure out what's happening.
        /// - Todo: Need to test what happens when -1 if supplied.</remarks>
        public static string XWF_GetReportTableInfo(int reportTableId
            , ReportTableInformationOptions informationOptions)
        {
            try
            {
                IntPtr ReportNameBuffer = ImportedMethods.XWF_GetReportTableInfo(
                    IntPtr.Zero, reportTableId, informationOptions);
                string ReportName = Marshal.PtrToStringUni(ReportNameBuffer);
                Marshal.FreeHGlobal(ReportNameBuffer);

                return ReportName;
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
        /// <param name="evidence">A pointer to the evidence object.</param>
        /// <param name="options">Optional. Return list flags.</param>
        /// <returns></returns>
        /// <remarks>Version 0.5 coding complete.
        /// - Todo: Need to test the output and build an array of items returned.
        /// - Todo: Need to figure out what the flags are used for.
        /// </remarks>
        public static IntPtr XWF_GetEvObjReportTableAssocs(IntPtr evidence
            , uint options = 0x01)
        {
            IntPtr lpValue;
            IntPtr associationList = ImportedMethods.XWF_GetEvObjReportTableAssocs(
                evidence, options, out lpValue);

            return lpValue;
        }

        /// <summary>
        /// Defines to which volume's volume snapshot subsequent calls of the below
        /// functions apply should you wish to change that.
        /// </summary>
        /// <param name="volume">A pointer to the specified volume.</param>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static void XWF_SelectVolumeSnapshot(IntPtr volume)
        {
            // Fail if an evidence pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            ImportedMethods.XWF_SelectVolumeSnapshot(volume);
        }

        /// <summary>
        /// Retrieves information about the current volume snapshot. Available in v17.4
        /// and later.
        /// </summary>
        /// <param name="propertyType">Property type.</param>
        /// <param name="specialItemType">Optional. Special item type. Only required
        /// when propertyType is SpecialItemID.</param>
        /// <returns>Returns the property requested.</returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Not sure we need this since we have another method below to get all 
        /// properties.</remarks>
        public static long XWF_GetVSProp(VolumeSnapshotPropertyType propertyType
            , SpecialItemType specialItemType = SpecialItemType.Ununsed)
        {
            return ImportedMethods.XWF_GetVSProp(propertyType, specialItemType);
        }

        /// <summary>
        /// Retrieves all available properties of the current volume snapshot.  Available
        /// in v17.4 and above.
        /// </summary>
        /// <returns>Returns the properties in a VolumeSnapshotProperties structure.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static VolumeSnapshotProperties XWF_GetVSProps()
        {
            VolumeSnapshotProperties Properties = new VolumeSnapshotProperties();

            // Get the root directory.
            Properties.rootDirectory = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.RootDirectory);
            // Get the "Path Unknown" directory.
            Properties.pathUnknownDirectory = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.PathUnknownDirectory);
            // Get the "Carved Files" directory.
            Properties.carvedFilesDirectory = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.CarvedFilesDirectory);
            // Get the free space file.
            Properties.freeSpaceFile = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.FreeSpaceFile);
            // Get the "System Volume Information" directory.
            Properties.systemVolumeInformationDirectory = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.SystemVolumeInformationDirectory);
            // Get the Windows EDB file.
            Properties.windowsEDBFile = XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.WindowsEDBFile);
            // Get the first hash.
            Properties.hashType1 = (HashType)XWF_GetVSProp(
                VolumeSnapshotPropertyType.HashType1
                , SpecialItemType.Ununsed);
            // Get the second hash.
            Properties.hashType2 = (HashType)XWF_GetVSProp(
                VolumeSnapshotPropertyType.HashType2
                , SpecialItemType.Ununsed);

            return Properties;
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
        /// <remarks>Version 1.0 coding complete.</remarks>
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
        /// <param name="directoryId">The directory ID.</param>
        /// <returns>Returns the accumulated number of all files under a provided
        /// directory or file and all it's subdirectories</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static uint XWF_GetFileCount(uint directoryId)
        {
            return ImportedMethods.XWF_GetFileCount(directoryId);
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
        /// <param name="itemName">The name of the item.</param>
        /// <param name="options">Creation flags (XWFCreateItemFlags).</param>
        /// <returns>Returns the ID of the newly created item, or -1 if an error
        /// occurred (e.g. out of memory).</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static int XWF_CreateItem(
            [MarshalAs(UnmanagedType.LPWStr)] string itemName, CreateItemOptions options)
        {
            return ImportedMethods.XWF_CreateItem(itemName, options);
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
        /// <param name="fileName">The name that this file will have in the volume 
        /// snapshot, which may be different from its source file name if you are
        /// attaching an external file.</param>
        /// <param name="options">Creation flags (XWFCreateFileFlags).</param>
        /// <param name="parentItemId">The file's parent ID, if needed.</param>
        /// <param name="pSourceInfo">More information about the source of the file's
        /// data. The exact meaning depends on the flags.</param>
        /// <returns>Returns the Id of the newly created item or -1 if an error occurred.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static int XWF_CreateFile(
            [MarshalAs(UnmanagedType.LPWStr)] string fileName, CreateFileOptions options
            , uint parentItemId, IntPtr sourceInformation)
        {
            return ImportedMethods.XWF_CreateFile(fileName, options, parentItemId
                , sourceInformation);
        }

        /// <summary>
        /// Retrieves a pointer to the null-terminated name of the specified item (file
        /// or directory) in UTF-16. You may call XWF_GetItemName and XWF_GetItemParent
        /// repeatedly until you reach the root directory and concatenate the results to
        /// get the full path of an item.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <returns>Returns name of the item.</returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Implement path builder helper method.
        /// - Todo: Currently catching all exceptions; need to test further.</remarks>
        public static string XWF_GetItemName(int itemId)
        {
            string result;

            try
            {
                IntPtr Buffer = ImportedMethods.XWF_GetItemName(itemId);
                result = Marshal.PtrToStringUni(Buffer);
                Marshal.FreeHGlobal(Buffer);
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
        /// <param name="itemId">The item ID.</param>
        /// <returns>Returns the size of the item, or -1 if unknown.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static long XWF_GetItemSize(long itemId)
        {
            return ImportedMethods.XWF_GetItemSize(itemId);
        }

        /// <summary>
        /// Sets the size of the item in bytes, using -1 when the size is unknown.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="size">The size of the item, or -1 if unknown.</param>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static void XWF_SetItemSize(int itemId, int size)
        {
            ImportedMethods.XWF_SetItemSize(itemId, size);
        }

        /// <summary>
        /// Retrieves the offset of the file system data structure (e.g. NTFS FILE 
        /// record) where the item is defined. If negative, the absolute value is the 
        /// offset where a carved file starts on the volume. 0 if an error occurred. 
        /// 0xFFFFFFFF if not available/not applicable. Also retrieves the number of the 
        /// sector from the point of the volume in which the data of the item starts.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <returns>Returns XWFItemOffsets struct with the relative offsets.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static ItemOffsets XWF_GetItemOfs(int itemId)
        {
            long ItemOffset, StartSector;
            ItemOffsets ItemOffsets = new ItemOffsets();

            ImportedMethods.XWF_GetItemOfs(itemId, out ItemOffset, out StartSector);

            if (ItemOffset >= 0)
            {
                ItemOffsets.FileSystemDataStructureOffset = ItemOffset;
                ItemOffsets.CarvedFileVolumeOffset = -1;
            }
            else
            {
                ItemOffsets.FileSystemDataStructureOffset = -1;
                ItemOffsets.CarvedFileVolumeOffset = Math.Abs(ItemOffset);
            }

            ItemOffsets.DataStartSector = StartSector;

            return ItemOffsets;
        }

        /// <summary>
        /// Sets the offset and data sector start of a given item.
        /// </summary>
        /// <param name="ItemId">The item ID.</param>
        /// <param name="itemOffsets">A ItemOffsets struct with the offsets to use.
        /// </param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static bool XWF_SetItemOfs(int ItemId, ItemOffsets itemOffsets)
        {
            long itemOffset;

            if (itemOffsets.FileSystemDataStructureOffset != -1)
            {
                itemOffset = itemOffsets.FileSystemDataStructureOffset;
            }
            else if (itemOffsets.CarvedFileVolumeOffset != -1)
            {
                itemOffset = itemOffsets.CarvedFileVolumeOffset * -1;
            }
            else
            {
                return false;
            }

            ImportedMethods.XWF_SetItemOfs(ItemId, itemOffset
                , itemOffsets.DataStartSector);

            return true;
        }

        /// <summary>
        /// Returns information about an item (file or directory) as stored in the 
        /// volume snapshot, such as the original ID or attributes that the item had in 
        /// its defining file system.
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns ItemInformation struct with the given item's information.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static ItemInformation XWF_GetItemInformation(int itemId)
        {
            ItemInformation Information = new ItemInformation();
            bool Status;

            // Get the original Id.
            Information.OriginalItemID = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_ORIG_ID, out Status);

            // Get the attributes.
            Information.Attributes = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_ATTR, out Status);

            // Get the flags.
            Information.Flags = (ItemInformationOptions)
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_FLAGS, out Status);

            // Get the deletion information.
            Information.Deletion = (ItemDeletionStatus)
                ImportedMethods.XWF_GetItemInformation(itemId
                    , ItemInformationType.XWF_ITEM_INFO_DELETION, out Status);

            // Get the classification.
            Information.Classification = (ItemClassifiction)
                ImportedMethods.XWF_GetItemInformation(itemId
                    , ItemInformationType.XWF_ITEM_INFO_CLASSIFICATION, out Status);

            // Get the link count.
            Information.LinkCount = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_LINKCOUNT, out Status);

            // Get the color analysis.
            Information.ColorAnalysis = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_COLORANALYSIS, out Status);

            // Get the file count.
            Information.FileCount = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_FILECOUNT, out Status);

            // Get the embedded offset.
            Information.EmbeddedOffset = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_EMBEDDEDOFFSET, out Status);

            // Get the creation time.
            Information.CreationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_CREATIONTIME, out Status));

            // Get the modification time.
            Information.ModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_MODIFICATIONTIME, out Status));

            // Get the last access time.
            Information.LastAccessTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_LASTACCESSTIME, out Status));

            // Get the entry modification time.
            Information.EntryModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_ENTRYMODIFICATIONTIME
                , out Status));

            // Get the deletion time.
            Information.DeletionTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_DELETIONTIME, out Status));

            // Get the internal creation time.
            Information.InternalCreationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.XWF_ITEM_INFO_INTERNALCREATIONTIME
                , out Status));

            return Information;
        }

        /// <summary>
        /// Sets information about an item (file or directory) in the volume snapshot.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="informationType"></param>
        /// <param name="informationValue"></param>
        /// <remarks>Todo: Everything!</remarks>
        public static void XWF_SetItemInformation(int itemId
            , ItemInformationType informationType, long informationValue)
        {
        }

        /// <summary>
        /// Retrieves a textual description of the type of the specified file and 
        /// returns information about the status of the type detection of the file: 
        /// 0 = not verified, 1 = too small, 2 = totally unknown, 3 = confirmed, 
        /// 4 = not confirmed, 5 = newly identified, 6 (v18.8 and later only) = mismatch
        /// detected. ­1 means error.
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns a ItemType structure with the file type and description.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static ItemType XWF_GetItemType(int itemId)
        {
            ItemType Results = new ItemType();

            // Allocate a buffer to receive the type description.
            IntPtr bufferPtr = Marshal.AllocHGlobal(_itemTypeDescriptionBufferLength);

            // Get the results from the API function, including the type description.
            Results.Type = ImportedMethods.XWF_GetItemType(itemId, bufferPtr
                , _itemTypeDescriptionBufferLength);
            Results.Description = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            return Results;
        }

        /// <summary>
        /// Sets a description of the type of the specified file (or specify NULL if not 
        /// required) and information about the status of the type detection of the file.
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <param name="typeDescription">A type description.</param>
        /// <param name="itemType">The item type category.</param>
        /// <returns></returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Catching all exceptions; need to investigate possibilities.
        /// - Todo: Convert to static method.</remarks>
        public static bool XWF_SetItemType(int itemId, string typeDescription
            , ItemTypeCategory itemType)
        {
            try
            {
                ImportedMethods.XWF_SetItemType(itemId, typeDescription, itemType);
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
        /// <param name="itemId">The item ID.</param>
        /// <returns>Returns the parent ID of the given item, or -1 if there is none.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static int XWF_GetItemParent(int itemId)
        {
            return ImportedMethods.XWF_GetItemParent(itemId);
        }

        /// <summary>
        /// Sets the parent of a given child item.
        /// </summary>
        /// <param name="childItemID">The child ID.</param>
        /// <param name="parentItemID">The parent ID.</param>
        /// <returns>Return true is successful, otherwise false.</returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Catching all exceptions; need to invetigate possibilities.
        /// - Todo: What happens when invalid child or parents is given?</remarks>
        public static bool XWF_SetItemParent(int childItemID, int parentItemID)
        {
            try
            {
                ImportedMethods.XWF_SetItemParent(childItemID, parentItemID);
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
            for (int bufferLength = BufferLengthStep; ; bufferLength += BufferLengthStep)
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
                int NullCharacterIndex = Str.IndexOf((char)0);
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

        public static string Hexlify(byte[] ba)
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

            while (hCurrent != IntPtr.Zero)
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
                return XWF_Read(hItem, 0, bufferSize);
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
                ,
                lpSearchTerms = searchTerms
                ,
                nFlags = flags
                ,
                nSearchWindow = 0
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
