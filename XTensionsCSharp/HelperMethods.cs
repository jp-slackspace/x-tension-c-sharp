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
        private static readonly uint _userInputLength = 1024 * 2;

        /// <summary>
        /// Returns the size of the volume or physical size of the file to which you 
        /// provide a handle. A helper method for XWF_GetSize().
        /// </summary>
        /// <param name="volumeOrItem">A pointer to a volume or item.</param>
        /// <param name="sizeType">The type of size to retrieve. The default is 
        /// ItemSizeType.PhysicalSize.  Supported from v16.7 SR-8.</param>
        /// <returns>Returns the size of the volume or physical size of the file.
        /// </returns>
        public static long GetSize(IntPtr volumeOrItem, ItemSizeType sizeType
            = ItemSizeType.PhysicalSize)
        {
            // Fail if a volume or file pointer weren't provided.
            if (volumeOrItem == IntPtr.Zero) throw new ArgumentException(
                "Zero volume or file pointer provided.");

            return ImportedMethods.XWF_GetSize(volumeOrItem, sizeType);
        }

        /// <summary>
        /// Retrieves the name of the provided volume, using 255 characters at most. A 
        /// helper method for XWF_GetVolumeName().
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="volumeNameType">The volume name type to return.  The default is
        /// VolumeNameType.Type1.</param>
        /// <returns>Returns the volume name in the type specified.</returns>
        public static string GetVolumeName(IntPtr volume, VolumeNameType volumeNameType 
            = VolumeNameType.Type1)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Allocate a buffer to receive the volume name, call the API function and
            // get volume name, and clean up.
            IntPtr Buffer = Marshal.AllocHGlobal(_volumeNameBufferLength);
            ImportedMethods.XWF_GetVolumeName(volume, Buffer, volumeNameType);
            string VolumeName = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            return VolumeName;
        }

        /// <summary>
        /// Retrieves various information about a given volume. A helper method for 
        /// XWF_GetVolumeInformation().
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <returns>Returns the volume information in a VolumeInformation struct.
        /// </returns>
        public static VolumeInformation GetVolumeInformation(IntPtr volume)
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
        /// Retrieves the boundaries of the currently selected block, if any. A helper 
        /// method for XWF_GetBlock().
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <returns>Returns the block boundaries in a BlockBoundaries struct.  These 
        /// boundaries will be 0 and -1 respectively if no block is selected.</returns>
        public static BlockBoundaries GetBlock(IntPtr volume)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            BlockBoundaries Boundaries = new BlockBoundaries();

            // Call the API function, getting the block's boundaries.
            bool Result = ImportedMethods.XWF_GetBlock(volume
                , out Boundaries.StartOffset, out Boundaries.EndOffset);

            return Boundaries;
        }

        /// <summary>
        /// Set the boundaries (provided as a BlockBoundaries struct) of a new block. A
        /// helper method for XWF_SetBlock().
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="boundaries">A BlockBoundaries struct indicating the starting
        /// and ending offsets of the new block.</param>
        /// <returns>Returns True if successful and False if the provided boundaries 
        /// exceed the size of the volume.</returns>
        public static bool SetBlock(IntPtr volume, BlockBoundaries boundaries)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Return results of API call to set the block.
            return ImportedMethods.XWF_SetBlock(volume, boundaries.StartOffset
                , boundaries.EndOffset);
        }

        /// <summary>
        /// Clear an existing block. A helper method for XWF_ClearBlock().
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        public static void ClearBlock(IntPtr volume)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Return results of API call to clear the block.
            ImportedMethods.XWF_SetBlock(volume, 0, -1);
        }

        /// <summary>
        /// Retrieves information about a provided sector. A helper method for 
        /// XWF_GetSectorContents().
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="sectorNumber">The sector number.</param>
        /// <returns>Returns the sector information in a SectorInformation struct.
        /// </returns>
        public static SectorInformation GetSectorContents(IntPtr volume
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
        /// Opens the provided item for reading. Available in v16.5 and later. A helper
        /// method for XWF_OpenItem().
        /// </summary>
        /// <param name="volume">A pointer to a volume.</param>
        /// <param name="itemId">The item's Id.</param>
        /// <param name="options">Options for opening, using ItemOpenModes flag.</param>
        /// <returns>Returns a handle to the open item, or IntPtr.Zero if unsuccessful
        /// </returns>
        public static IntPtr OpenItem(IntPtr volume, long itemId, ItemOpenModes options 
            = ItemOpenModes.LogicalContents)
        {
            // Fail if a volume pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Catch exception if the item is not able to be opened.  We're going to 
            // output information about the exception and return a zero pointer.
            try
            {
                return ImportedMethods.XWF_OpenItem(volume, itemId, options);
            }
            catch (System.AccessViolationException e)
            {
                OutputMessage("Exception: " + e);
                OutputMessage("It's likely that the provided itemId doesn't exist.");
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Closes a volume or an item that was opened with the OpenItem method. 
        /// Available in v16.5 and later. A helper method for XWF_Close().
        /// </summary>
        /// <param name="volumeOrItem">An open volume or item.</param>
        /// <returns>Returns true if a successfull, otherwise false.</returns>
        public static void CloseItem(IntPtr volumeOrItem)
        {
            // Fail if a volume or item pointer weren't provided.
            if (volumeOrItem == IntPtr.Zero) throw new ArgumentException(
                "Zero volume or item pointer provided");

            ImportedMethods.XWF_Close(volumeOrItem);
        }

        /// <summary>
        /// Reads the specified number of bytes from a specified position in a specified 
        /// item. A helper method for XWF_Read().
        /// </summary>
        /// <param name="volumeOrItem">A pointer to a volume or item.</param>
        /// <param name="offset">The offset to read from.</param>
        /// <param name="numberOfBytesToRead">The number of bytes to read.</param>
        /// <returns>Returns a byte array of the bytes read.</returns>
        public static byte[] Read(IntPtr volumeOrItem, long offset = 0
            , int numberOfBytesToRead = 0)
        {
            // Fail if a volume or item pointer weren't provided.
            if (volumeOrItem == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            // Read the full file from the provided offset if the provided number of 
            // bytes to read is 0.
            if (numberOfBytesToRead == 0)
            {
                numberOfBytesToRead = (int)(GetSize(volumeOrItem
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
        /// Reads an entire item.
        /// </summary>
        /// <param name="item">The specified item.</param>
        /// <returns>Returns a byte array of the item's contents.</returns>
        /// <remarks>Needs Testing. Also need to check size of item to make sure it's 
        /// not bigger than what an int can represent.</remarks>
        public static byte[] ReadItem(IntPtr item)
        {
            // Fail if a volume or item pointer weren't provided.
            if (item == IntPtr.Zero) throw new ArgumentException(
                "Zero item pointer provided");
            
            // Get the size of the provided item.
            int Size = (int)GetSize(item, ItemSizeType.PhysicalSize);

            // Call XWF_Read to read the item into the buffer.
            return Read(item, 0, Size);
        }

        /// <summary>
        /// Retrieves information about the current case. A helper method for 
        /// XWF_GetCaseProp().
        /// </summary>
        /// <returns>Returns a CaseProperties structure.</returns>
        /// <remarks>Test for when no case is loaded.</remarks>
        public static CaseProperties? GetCaseProperties()
        {
            long Status;
            CaseProperties Properties = new CaseProperties();
            
            // Read the title.
            IntPtr Buffer = Marshal.AllocHGlobal(_casePropertiesLength);
            Status = ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseTitle, Buffer, _casePropertiesLength);
            Properties.CaseTitle = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            // A negative status indicates no case is loaded; return null.
            if (Status < 0)
            {
                return null;
            }

            // Read the examiner.
            Buffer = Marshal.AllocHGlobal(_casePropertiesLength);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseExaminer, Buffer, _casePropertiesLength);
            Properties.CaseExaminer = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            // Read the case file path.
            Buffer = Marshal.AllocHGlobal(_casePropertiesLength);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseFilePath, Buffer, _casePropertiesLength);
            Properties.CaseFilePath = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            // Read the case directory.
            Buffer = Marshal.AllocHGlobal(_casePropertiesLength);
            ImportedMethods.XWF_GetCaseProp(IntPtr.Zero
                , (int)CasePropertyType.CaseDirectory, Buffer, _casePropertiesLength);
            Properties.CaseDirectory = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            return Properties;
        }

        /// <summary>
        /// Retrieves a handle to the first evidence object in the case. In conjunction
        /// with XWF_GetNextEvObj this function allows to enumerate all evidence objects
        /// of the case. Available from v17.6. A helper method for XWF_GetFirstEvObj().
        /// </summary>
        /// <returns>Returns a pointer to the first evidence objector, or NULL if the 
        /// active case has no evidence objects or (in releases from June 2016) if no 
        /// case is active.</returns>
        public static IntPtr GetFirstEvidenceObject()
        {
            return ImportedMethods.XWF_GetFirstEvObj(IntPtr.Zero);
        }

        /// <summary>
        /// Given a pointer to the previous evidence object, retrieves the next evidence
        /// object in the chain. Available from v17.6. A helper method for 
        /// XWF_GetNextEvObj().
        /// </summary>
        /// <param name="previousEvidence">Previous evidence object.</param>
        /// <returns>Returns a pointer to the next evidence object.</returns>
        public static IntPtr GetNextEvidenceObject(IntPtr previousEvidence)
        {
            // Handle case where zero pointer is provided.
            if (previousEvidence == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            return ImportedMethods.XWF_GetNextEvObj(previousEvidence, IntPtr.Zero);
        }

        /// <summary>
        /// Get an array of all evidence object pointers for the current case.
        /// </summary>
        /// <returns>Returns an array of all evidence object pointers for the current 
        /// case.</returns>
        public static ArrayList GetCaseEvidence()
        {
            ArrayList evidence = new ArrayList();

            // Get the first evidence object.
            IntPtr current = GetFirstEvidenceObject();

            // Iterate the remaining evidence objects, adding them each to the array.
            while (current != IntPtr.Zero)
            {
                evidence.Add(current);
                current = GetNextEvidenceObject(current);
            }

            return evidence;
        }

        /// <summary>
        /// Creates one or more evidence objects from one source (which can be a medium,
        /// disk/volume image, memory dump, or a directory/path). A case must already be 
        /// loaded. If more than 1 evidence object is created (for example for a physical 
        /// disk that contains partitions, which count as evidence objects themselves), 
        /// use XWF_GetNextEvObj to find them. Available in v16.5 and later. A helper 
        /// method for XWF_CreateEvObj().
        /// </summary>
        /// <param name="evidenceType">The evidence object type.</param>
        /// <param name="objectPath">Path in case of a file or directory, otherwise NULL.
        /// </param>
        /// <returns>Returns the first evidence object created, or NULL in case of an
        /// error.</returns>
        public static IntPtr CreateEvidenceObject(EvidenceObjectType evidenceType
            , string objectPath = null)
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
        /// older. A helper method for XWF_OpenEvObj().
        /// </summary>
        /// <param name="evidence">A pointer to the evidence object.</param>
        /// <param name="options">EvidenceOpenOptions open options.</param>
        /// <returns>Returns a handle to the volume that the evidence object represents 
        /// or returns 0 if unsuccessful.</returns>
        public static IntPtr OpenEvidenceObject(IntPtr evidence
            , EvidenceOpenOptions options)
        {
            return ImportedMethods.XWF_OpenEvObj(evidence, options);
        }

        /// <summary>
        /// Closes the specified evidence object if it is open currently and unloads the
        /// volume snapshot, otherwise does nothing. Available from v17.6. A helper 
        /// method for XWF_CloseEvObj().
        /// </summary>
        /// <param name="evidence">The evidence object to close.</param>
        public static void CloseEvidenceObject(IntPtr evidence)
        {
            ImportedMethods.XWF_CloseEvObj(evidence);
        }

        /// <summary>
        /// Retrieves information about the specified evidence object. Does not require
        /// that the evidence object be open. Available from v17.6. A helper method for
        /// XWF_GetEvObjProp().
        /// </summary>
        /// <param name="evidence">A pointer to the evidence object.</param>
        /// <returns>Returns a EvidenceObjectProperites struct.</returns>
        public static EvidenceObjectProperties GetEvidenceObjectProperties(
            IntPtr evidence)
        {
            // Fail if an evidence pointer wasn't provided.
            if (evidence == IntPtr.Zero) throw new ArgumentException(
                "Zero evidence pointer provided.");

            EvidenceObjectProperties Properties = new EvidenceObjectProperties();

            // Get the object number.
            Properties.objectNumber = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.EvidenceObjectNumber, IntPtr.Zero);

            // Get the object ID.
            Properties.objectID = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.EvidenceObjectID, IntPtr.Zero);

            // Get the parent object ID.
            Properties.parentObjectID = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.ParentEvidenceObjectID, IntPtr.Zero);

            // Get the title.
            long tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.Title, IntPtr.Zero);
            Properties.title = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Initialize the buffer for later use.
            int bufferSize = _eventObjectPropertiesLength;

            // Get the extended title.
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.ExtendedTitle, bufferPtr);
            Properties.extendedTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the abbreviated title.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.AbbreviatedTitle, bufferPtr);
            Properties.abbreviatedTitle = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the internal name.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.InternalName, IntPtr.Zero);
            Properties.internalName = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the description.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.Description, IntPtr.Zero);
            Properties.description = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the examiner comments.
            tmpPtr = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.ExaminerComments, IntPtr.Zero);
            Properties.examinerComments = Marshal.PtrToStringUni((IntPtr)tmpPtr);

            // Get the internally used directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.InternallyUsedDirectory, bufferPtr);
            Properties.internallyUsedDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the output directory.
            bufferPtr = Marshal.AllocHGlobal(bufferSize);
            ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.OutputDirectory, bufferPtr);
            Properties.outputDirectory = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            // Get the size in bytes.
            Properties.SizeInBytes = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.SizeInBytes, IntPtr.Zero);

            // Get the volume snapshot file count.
            Properties.VolumeSnapshotFileCount = ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.VolumeSnapshotFileCount, IntPtr.Zero);

            // Get the flags.
            Properties.Flags = (EvidenceProperties)ImportedMethods.XWF_GetEvObjProp(
                evidence, EvidencePropertyType.Flags, IntPtr.Zero);

            // Get the file system identifier.
            Properties.FileSystemIdentifier
                = (VolumeFileSystem)ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.FileSystemIdentifier, IntPtr.Zero);

            // Get the hash type.
            Properties.HashType = (HashType)ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.HashType, IntPtr.Zero);

            // Get the hash value.
            if (Properties.HashType == HashType.Undefined)
            {
                Properties.HashValue = null;
            }
            else
            {
                bufferPtr = Marshal.AllocHGlobal(bufferSize);
                int hashSize = (int)ImportedMethods.XWF_GetEvObjProp(evidence
                    , EvidencePropertyType.HashValue, bufferPtr);
                Byte[] hash1 = new Byte[hashSize];
                Marshal.Copy(bufferPtr, hash1, 0, hashSize);
                Properties.HashValue = hash1;
                Marshal.FreeHGlobal(bufferPtr);
            }

            // Get the creation time.
            Properties.CreationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.CreationTime, IntPtr.Zero));

            // Get the modification time.
            Properties.ModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetEvObjProp(evidence
                    , EvidencePropertyType.ModificationTime, IntPtr.Zero));

            // Get the hash 2 type.
            Properties.HashType2 = (HashType)ImportedMethods.XWF_GetEvObjProp(evidence
                , EvidencePropertyType.HashType2, IntPtr.Zero);

            // Get the hash 2 value.
            if (Properties.HashType2 == HashType.Undefined)
            {
                Properties.HashValue2 = null;
            }
            else
            {
                bufferPtr = Marshal.AllocHGlobal(bufferSize);
                int hashSize = (int)ImportedMethods.XWF_GetEvObjProp(evidence
                    , EvidencePropertyType.HashValue2, bufferPtr);
                Byte[] hash2 = new Byte[hashSize];
                Marshal.Copy(bufferPtr, hash2, 0, hashSize);
                Properties.HashValue2 = hash2;
                Marshal.FreeHGlobal(bufferPtr);
            }

            return Properties;
        }

        /// <summary>
        /// Retrieves a handle to the evidence object with the specified unique ID. The 
        /// unique ID of an evidence object remains the same after closing and re-opening 
        /// a case, whereas the handle will likely change. The evidence object number may 
        /// also change. That happens if the user re-orders the evidence objects in the 
        /// case. The unique ID, however, is guaranteed to never change and also 
        /// guaranteed to be unique within the case (actually likely unique even across 
        /// all the cases that the user will even deal with) and can be used to reliably 
        /// recognize a known evidence object. Available from v18.7. A helper method for 
        /// XWF_GetEvObj().
        /// </summary>
        /// <param name="evidenceObjectId"></param>
        /// <returns>Returns a pointer to the evidence object corresponding to the 
        /// specified evidence object Id or NULL if not found.</returns>
        private static IntPtr GetEvidenceObject(uint evidenceObjectId)
        {
            return ImportedMethods.XWF_GetEvObj(evidenceObjectId);
        }

        /// <summary>
        /// Gets the name of a report table, null if none, or the maximum number of
        /// report table names if reportTableId is set to -1. Valid report table IDs 
        /// range from 0 to (maximum number: -1).  Available in v17.7 and later. A helper
        /// method for XWF_GetReportTableInfo().
        /// </summary>
        /// <param name="reportTableId">An existing report table ID or -1 to get the 
        /// maximum number of report tables.</param>
        /// <param name="informationOptions">ReportTableInformationOptions options.
        /// Should be ReportTableInformationOptions.None before v18.1.</param>
        /// <returns>Returns the name of a given report table or null if none.</returns>
        /// <remarks>Should I change the name to GetReportTableName? Right now catching 
        /// exceptions; need to figure out what's happening. Need to test what happens 
        /// when -1 if supplied.</remarks>
        public static string GetReportTableInformation(int reportTableId
            , ReportTableInformationOptions informationOptions)
        {
            try
            {
                IntPtr Buffer = ImportedMethods.XWF_GetReportTableInfo(
                    IntPtr.Zero, reportTableId, informationOptions);
                string ReportName = Marshal.PtrToStringUni(Buffer);
                Marshal.FreeHGlobal(Buffer);

                return ReportName;
            }
            catch (System.AccessViolationException e)
            {
                OutputMessage("Exception: " + e);
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
        /// report table ID and 32-bit item ID pairs repeatedly, stored back to back. A
        /// helper method for XWF_GetEvObjReportTableAssocs().
        /// </summary>
        /// <param name="evidence">A pointer to the evidence object.</param>
        /// <param name="options">Optional. Return list flags.</param>
        /// <returns></returns>
        /// <remarks>Version 0.5 coding complete.
        /// - Todo: Need to test the output and build an array of items returned.
        /// - Todo: Need to figure out what the flags are used for.
        /// </remarks>
        public static IntPtr GetEvidenceObjectReportTableAssociations(IntPtr evidence
            , uint options = 0x01)
        {
            IntPtr Value;
            IntPtr associationList = ImportedMethods.XWF_GetEvObjReportTableAssocs(
                evidence, options, out Value);

            return Value;
        }

        /// <summary>
        /// Defines to which volume's volume snapshot subsequent calls of the below
        /// functions apply should you wish to change that. A helper method for 
        /// XWF_SelectVolumeSnapshot().
        /// </summary>
        /// <param name="volume">A pointer to the specified volume.</param>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static void SelectVolumeSnapshot(IntPtr volume)
        {
            // Fail if an evidence pointer wasn't provided.
            if (volume == IntPtr.Zero) throw new ArgumentException(
                "Zero volume pointer provided.");

            ImportedMethods.XWF_SelectVolumeSnapshot(volume);
        }

        /// <summary>
        /// Retrieves all available properties of the current volume snapshot.  Available
        /// in v17.4 and above. A helper method for XWF_GetVSProps() though this one gets
        /// all properties rather than just one.
        /// </summary>
        /// <returns>Returns the properties in a VolumeSnapshotProperties structure.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static VolumeSnapshotProperties GetVolumeSnapshotProperties()
        {
            VolumeSnapshotProperties Properties = new VolumeSnapshotProperties();

            // Get the root directory.
            Properties.rootDirectory = ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.RootDirectory);
            // Get the "Path Unknown" directory.
            Properties.pathUnknownDirectory = ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.PathUnknownDirectory);
            // Get the "Carved Files" directory.
            Properties.carvedFilesDirectory = ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.CarvedFilesDirectory);
            // Get the free space file.
            Properties.freeSpaceFile = ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.FreeSpaceFile);
            // Get the "System Volume Information" directory.
            Properties.systemVolumeInformationDirectory = ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.SystemVolumeInformationDirectory);
            // Get the Windows EDB file.
            Properties.windowsEDBFile = ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.SpecialItemID
                , SpecialItemType.WindowsEDBFile);
            // Get the first hash.
            Properties.hashType1 = (HashType)ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.HashType1
                , SpecialItemType.Ununsed);
            // Get the second hash.
            Properties.hashType2 = (HashType)ImportedMethods.XWF_GetVSProp(
                VolumeSnapshotPropertyType.HashType2
                , SpecialItemType.Ununsed);

            return Properties;
        }

        /// <summary>
        /// Retrieves the number of items in the current volume snapshot (both files and
        /// directories). Item IDs are consecutive 0-based, meaning the ID of the first
        /// item in the volume snapshot is 0 and the last item is GetItemCount() - 1. You
        /// address each and every item in that range, be it a file or directory, by 
        /// specifying its ID. A helper method for XWF_GetItemCount().
        /// </summary>
        /// <returns>Returns the number of files and directories in the current volume
        /// snapshot.</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static uint GetItemCount()
        {
            return ImportedMethods.XWF_GetItemCount(IntPtr.Zero);
        }

        /// <summary>
        /// Retrieves the accumulated number of files in the directory with the 
        /// specified ID and all its subdirectories. Also works for files that have
        /// child objects. Not currently supported for the root directory though you may
        /// specify -1 as the ID to get the total file count of the entire volume.
        /// Available from v17.7. A helper method for XWF_GetFileCount().
        /// </summary>
        /// <param name="directoryId">The directory ID.</param>
        /// <returns>Returns the accumulated number of all files under a provided
        /// directory or file and all it's subdirectories</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static uint GetFileCount(int directoryId)
        {
            // Fail if a directory Id less than -1 is provided.
            if (directoryId < -1) throw new ArgumentException(
                "Invalid directory Id provided.");

            return ImportedMethods.XWF_GetFileCount(directoryId);
        }

        /// <summary>
        /// Creates a new item (file or directory) in the volume snapshot. May be called
        /// when refining the volume snapshot. Should be followed by calls to 
        /// SetItemParent(), SetItemSize(), SetItemInformation(), and/or 
        /// SetItemOffsets(). If via SetItemParent(), you make the new file a child
        /// object of a file (not directory); you are responsible for setting the 
        /// parent's ItemInformationOptions.hasChildren flag. For example, if you are
        /// creating a file carved from the sectors of the evidence object, you can
        /// specify the file size using SetItemSize() and the start offset via the
        /// ItemOffsets.CarvedFileVolumeOffset parameter using SetItemOffsets(). A helper 
        /// method for XWF_CreateItem().
        /// </summary>
        /// <param name="itemName">The name of the item.</param>
        /// <param name="options">Creation option.</param>
        /// <returns>Returns the ID of the newly created item, or -1 if an error
        /// occurred (e.g. out of memory).</returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static int CreateItem(string itemName, CreateItemOptions options)
        {
            // Fail if no item name provided.
            if (itemName == null || itemName == "") throw new ArgumentException(
                "Must provide an item name.");

            return ImportedMethods.XWF_CreateItem(itemName, options);
        }

        /// <summary>
        /// Similar to CreateItem(), but also allows attachment of an external file to
        /// the volume snapshot or to define a file that is an excerpt of another file
        /// (its parent). Returns the Id of the newly created item, or -1 if an error 
        /// occurred (e.g. out of memory). Should be followed by a call to SetItemSize() 
        /// (not necessary if attaching an external file) or SetItemInformation() (not 
        /// necessary when carving a file in a file). Available from v16.7. A helper 
        /// method for XWF_CreateFile().
        /// </summary>
        /// <param name="fileName">The name that this file will have in the volume 
        /// snapshot, which may be different from its source file name if you are
        /// attaching an external file.</param>
        /// <param name="options">Creation flags (XWFCreateFileFlags).</param>
        /// <param name="parentItemId">The file's parent Id, if needed.</param>
        /// <param name="sourceInformation">More information about the source of the 
        /// file's data. The exact meaning depends on the options provided.</param>
        /// <returns>Returns the Id of the newly created item or -1 if an error occurred.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.
        /// Todo: Need to learn more about sourceInformation.</remarks>
        public static int CreateFile(string fileName, CreateFileOptions options, 
            int parentItemId, IntPtr sourceInformation)
        {
            // Fail if no item name provided.
            if (fileName == null || fileName == "") throw new ArgumentException(
                "Must provide a file name.");

            return ImportedMethods.XWF_CreateFile(fileName, options, parentItemId
                , sourceInformation);
        }

        /// <summary>
        /// Retrieves the name of the specified item (file or directory). You may call 
        /// GetItemName() and GetItemParent() repeatedly until you reach the root 
        /// directory and concatenate the results to get the full path of an item (See 
        /// GetFullPath()). A helper method for XWF_GetItemName().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns name of the item.</returns>
        /// <remarks>What happens if an incorrect item Id is provided?</remarks>
        public static string GetItemName(int itemId)
        {
            if (itemId < -1) throw new ArgumentException(
                "Invalid item Id provided.");

            string Result;

            IntPtr Buffer = ImportedMethods.XWF_GetItemName(itemId);
            Result = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            return Result;
        }

        /// <summary>
        /// Uses GetItemName() and GetItemParent() to recursively build a file path.
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>The full path of the specified item.</returns>
        /// <remarks>Needs testing.</remarks>
        public static string GetFullPath(int itemId)
        {
            // Fail if a item Id less than -1 is provided.
            if (itemId < -1) throw new ArgumentException(
                "Invalid item Id provided.");
            // Provide an empty string if the root is provided.
            else if (itemId == -1)
                return "";

            StringBuilder sb = new StringBuilder();

            // Add the lowest item of the path.
            sb.Insert(0, GetItemName(itemId));

            // Loop until we reach the root.
            while (true)
            {
                // Get the current item's parent Id.
                int parentItemId = GetItemParent(itemId);

                // Finished if the parent is the root.
                if (parentItemId < 0) return sb.ToString();

                // Prepend the parent name and the path separator and continue.
                sb.Insert(0, Path.DirectorySeparatorChar + GetItemName(itemId));
                itemId = parentItemId;
            }
        }

        /// <summary>
        /// Retrieves the size of the item (file or directory) in bytes, or -1 when the
        /// size is unknown. A helper method for XWF_GetItemSize().
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <returns>Returns the size of the item, or -1 if unknown.</returns>
        public static long GetItemSize(int itemId)
        {
            // Fail if a item Id less than -1 is provided.
            if (itemId < -1) throw new ArgumentException(
                "Invalid item Id provided.");

            return ImportedMethods.XWF_GetItemSize(itemId);
        }

        /// <summary>
        /// Sets the size of the item in bytes, using -1 when the size is unknown. A
        /// helper method for XWF_SetItemSize().
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="size">The size of the item, or -1 if unknown.</param>
        /// <remarks>Needs testing. Why set size as unknown?</remarks>
        public static void SetItemSize(int itemId, long size)
        {
            // Fail if a item Id less than -1 is provided.
            if (itemId < -1) throw new ArgumentException(
                "Invalid item Id provided.");

            ImportedMethods.XWF_SetItemSize(itemId, size);
        }

        /// <summary>
        /// Returns the data structure offset or carved file location of an item and the
        /// sector where it's data starts. The API method referenced retrieves the offset 
        /// of the file system data structure (e.g. NTFS FILE record) where the item is 
        /// defined. If negative, the absolute value is the offset where a carved file 
        /// starts on the volume. 0 if an error occurred. 0xFFFFFFFF if not available/not 
        /// applicable. Also retrieves the number of the sector from the point of the 
        /// volume in which the data of the item starts. A helper method for 
        /// XWF_GetItemOfs().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns ItemOffsets struct with the relative offsets.</returns>
        /// <remarks>Needs testing.</remarks>
        public static ItemOffsets GetItemOffsets(int itemId)
        {
            // Fail if a item Id less than -1 is provided.
            if (itemId < -1) throw new ArgumentException(
                "Invalid item Id provided.");

            long ItemOffset, StartSector;
            ItemOffsets ItemOffsets = new ItemOffsets();

            ImportedMethods.XWF_GetItemOfs(itemId, out ItemOffset, out StartSector);

            // If positive, ItemOffset is a file system offset.
            if (ItemOffset >= 0)
            {
                ItemOffsets.FileSystemDataStructureOffset = ItemOffset;
                ItemOffsets.CarvedFileVolumeOffset = -1;
            }
            // If negative, ItemOffset (the absolute value of) if a carved file offset.
            else
            {
                ItemOffsets.FileSystemDataStructureOffset = -1;
                ItemOffsets.CarvedFileVolumeOffset = Math.Abs(ItemOffset);
            }

            // The sector where the data for the item starts.
            ItemOffsets.DataStartSector = StartSector;

            return ItemOffsets;
        }

        /// <summary>
        /// Sets the offset and data sector start of a given item. A helper method for
        /// XWF_SetItemOfs().
        /// </summary>
        /// <param name="ItemId">The item ID.</param>
        /// <param name="itemOffsets">A ItemOffsets struct with the offsets to use.
        /// </param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        /// <remarks>Needs testing.</remarks>
        public static bool SetItemOffsets(int itemId, ItemOffsets itemOffsets)
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

            ImportedMethods.XWF_SetItemOfs(itemId, itemOffset
                , itemOffsets.DataStartSector);

            return true;
        }

        /// <summary>
        /// Returns information about an item (file or directory) as stored in the 
        /// volume snapshot, such as the original Id or attributes that the item had in 
        /// its defining file system. A helper method for XWF_GetItemInformation().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns ItemInformation struct with the given item's information.
        /// </returns>
        /// <remarks>Needs testing.</remarks>
        public static ItemInformation GetItemInformation(int itemId)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            ItemInformation Information = new ItemInformation();
            bool Status;

            // Get the original Id.
            Information.originalItemID = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.OriginalId, out Status);

            // Get the attributes.
            Information.attributes = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.Attributes, out Status);

            // Get the flags.
            Information.options = (ItemInformationOptions)
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.Options, out Status);

            // Get the deletion information.
            Information.deletionStatus = (ItemDeletionStatus)
                ImportedMethods.XWF_GetItemInformation(itemId
                    , ItemInformationType.DeletionStatus, out Status);

            // Get the classification.
            Information.classification = (ItemClassifiction)
                ImportedMethods.XWF_GetItemInformation(itemId
                    , ItemInformationType.Classification, out Status);

            // Get the link count.
            Information.linkCount = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.LinkCount, out Status);

            // Get the color analysis.
            Information.colorAnalysis = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.ColorAnalysis, out Status);

            // Get the file count.
            Information.fileCount = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.FileCount, out Status);

            // Get the embedded offset.
            Information.embeddedOffset = ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.EmbeddedOffset, out Status);

            // Get the creation time.
            Information.creationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.CreationTime, out Status));

            // Get the modification time.
            Information.modificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.ModificationTime, out Status));

            // Get the last access time.
            Information.lastAccessTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.LastAccessTime, out Status));

            // Get the entry modification time.
            Information.entryModificationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.EntryModificationTime
                , out Status));

            // Get the deletion time.
            Information.deletionTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.DeletionTime, out Status));

            // Get the internal creation time.
            Information.internalCreationTime = DateTime.FromFileTime(
                ImportedMethods.XWF_GetItemInformation(itemId
                , ItemInformationType.InternalCreationTime
                , out Status));

            return Information;
        }

        /// <summary>
        /// Sets information about an item (file or directory) in the volume snapshot. A
        /// helper method for XWF_SetItemInformation().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <param name="informationType">The information type.</param>
        /// <param name="informationValue">The information value.</param>
        /// <remarks>Needs testing.</remarks>
        public static void SetItemInformation(int itemId
            , ItemInformationType informationType, long informationValue)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            ImportedMethods.XWF_SetItemInformation(itemId, informationType,
                informationValue);
        }

        /// <summary>
        /// Retrieves a textual description of the type of the specified file and 
        /// returns information about the status of the type detection of the file: 
        /// 0 = not verified, 1 = too small, 2 = totally unknown, 3 = confirmed, 
        /// 4 = not confirmed, 5 = newly identified, 6 (v18.8 and later only) = mismatch
        /// detected. ­1 means error. A helper method for XWF_GetItemType().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns a ItemType structure with the file type and description.
        /// </returns>
        /// <remarks>Needs testing.</remarks>
        public static ItemType GetItemType(int itemId)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

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
        /// A helper method for XWF_SetItemType().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <param name="typeDescription">A type description.</param>
        /// <param name="itemType">The item type category.</param>
        /// <remarks>Needs testing.</remarks>
        public static void SetItemType(int itemId, string typeDescription
            , ItemTypeCategory itemType)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            ImportedMethods.XWF_SetItemType(itemId, typeDescription, itemType);
        }

        /// <summary>
        /// Returns the Id of the parent of the specified item, or -1 if the item is the 
        /// root directory or if for some strange reason no parent object is assigned. A
        /// helper method for XWF_GetItemParent().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns the parent Id of the given item, or -1 if there is none.
        /// </returns>
        /// <remarks>Needs testing.</remarks>
        public static int GetItemParent(int itemId)
        {
            // Fail if invalid child item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid child Id provided.");

            return ImportedMethods.XWF_GetItemParent(itemId);
        }

        /// <summary>
        /// Sets the parent of a given child item. A helper method for 
        /// XWF_SetItemParent().
        /// </summary>
        /// <param name="childItemId">The child ID.</param>
        /// <param name="parentItemId">The parent ID. Specify -1 for the virtual "Path 
        /// unknown" directory, or -2 for the "Carved files" directory.</param>
        /// <returns>Return true is successful, otherwise false.</returns>
        /// <remarks>Needs testing. The code that sets the "hasChildren option needs
        /// looked at closely.</remarks>
        public static void SetItemParent(int childItemId, int parentItemId)
        {
            // Fail if invalid child item Id provided.
            if (childItemId < 0)
                throw new ArgumentException("Invalid child Id provided.");

            // Fail if invalid parent item Id provided.
            if (parentItemId < -2)
                throw new ArgumentException("Invalid parent Id provided.");

            ImportedMethods.XWF_SetItemParent(childItemId, parentItemId);

            // If the parent doesn't have the "hasChildren" option set, do this.
            if (parentItemId >= 0 && (GetItemInformation(parentItemId).options & 
                ItemInformationOptions.hasChildren) == 0)
            {
                SetItemInformation(parentItemId, ItemInformationType.Options,
                    (long)(ItemInformationOptions.hasChildren));
            }
        }

        /// <summary>
        /// Retrieves the names of the report tables that the specified item is 
        /// associated with. A helper method for XWF_GetReportTableAssocs().
        /// </summary>
        /// <param name="itemId">The ID of the provided item.</param>
        /// associated with.</param>
        /// <returns>Returns an array of report tables names.</returns>
        /// <remarks>Needs testing.</remarks>
        public static string[] GetReportTableAssociations(int itemId)
        {
            const int BufferLengthStep = 128;
            string Associations;

            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            for (int bufferLength = BufferLengthStep; ; bufferLength += BufferLengthStep)
            {
                // Allocate a buffer to receive the associations.
                IntPtr Buffer = Marshal.AllocHGlobal(bufferLength);

                // Get the results from the API function, including the associations up
                // the specified buffer length.
                int AssociationsCount = ImportedMethods.XWF_GetReportTableAssocs(itemId
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

        /// <summary>
        /// Associates the specified file with the specified report table. If the report 
        /// table does not exist yet in the currently active case, it will be created. 
        /// A helper method for XWF_AddToReportTable().
        /// </summary>
        /// <param name="itemId">The Id of the item to association with the report table.
        /// </param>
        /// <param name="reportTableName">The report table name.</param>
        /// <param name="options">Options to use for the association.</param>
        /// <returns>Returns the result of the assocation.</returns>
        /// <remarks>Needs testing.</remarks>
        public static AddToReportTableResult AddToReportTable(int itemId
            , string reportTableName, AddToReportTableOptions options)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            // Fail if no metadata text provided.
            if (reportTableName == null || reportTableName == "")
                throw new ArgumentException("Report table name must be provided.");

            return ImportedMethods.XWF_AddToReportTable(itemId, reportTableName,
                options);
        }

        /// <summary>
        /// Gets the comment (if any) of the given item. A helper method for 
        /// XWF_GetComment().
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <returns>Returns the comment.</returns>
        /// <remarks>Needs testing.</remarks>
        public static string GetComment(int itemId)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            string Comment;

            IntPtr Buffer = ImportedMethods.XWF_GetComment(itemId);
            Comment = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            return Comment;
        }

        /// <summary>
        /// Sets the comment of the given item. A helper method for XWF_AddComment().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <param name="commentText">The comment.</param>
        /// <param name="mode">Indicates how the comment should be added.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        /// <remarks>Need testing.</remarks>
        public static bool AddComment(int itemId, string commentText, 
            AddCommentMode mode)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            // Fail if no metadata text provided.
            if (commentText == null || commentText == "")
                throw new ArgumentException("Comment text must be provided.");

            return ImportedMethods.XWF_AddComment(itemId, commentText, mode);
        }

        /// <summary>
        /// Get the previously extracted metadata of a given item.  Good to use this one 
        /// if metadata has already been extracted from items. Available in v17.7 and 
        /// later. A helper method for XWF_GetExtractedMetadata().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns the previously extracted metadata.</returns>
        /// <remarks>Needs testing.</remarks>
        public static string GetExtractedMetadata(int itemId)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            string Metadata;

            IntPtr Buffer = ImportedMethods.XWF_GetExtractedMetadata(itemId);
            Metadata = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);

            return Metadata;
        }

        /// <summary>
        /// Adds the specified text to the extracted metadata of the specified item. 
        /// Available in v17.7 and later. A helper method for XWF_AddExtractedMetadata().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <param name="metadataText">The text to add to the extracted metadata.</param>
        /// <param name="mode">Indicates how the metadata should be added.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        /// <remarks>Needs testing.</remarks>
        public static bool AddExtractedMetadata(int itemId, string metadataText
            , AddCommentMode mode)
        {
            // Fail if invalid item Id provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            // Fail if no metadata text provided.
            if (metadataText == null || metadataText == "")
                throw new ArgumentException("Metadata text must be provided.");

            return ImportedMethods.XWF_AddExtractedMetadata(itemId, metadataText, mode);
        }

        /// <summary>
        /// Retrieves the hash value of a a file if one has been computed, which can be 
        /// checked using GetItemInformation(). Available in v16.8 and later. A helper 
        /// method for XWF_GetHashValue().
        /// </summary>
        /// <param name="itemId">The Id of the file item.</param>
        /// <returns>Returns the file hash.</returns>
        /// <remarks>Needs testing. Check version. Define variable for the buffer length.
        /// </remarks>
        public static string GetHashValue(int itemId)
        {
            // Fail if item Id less than 0 provided.
            if (itemId < 0)
                throw new ArgumentException("Invalid item Id provided.");

            string Hash;
            IntPtr Buffer = Marshal.AllocHGlobal(_volumeNameBufferLength);
            ImportedMethods.XWF_GetHashValue(itemId, Buffer);
            Hash = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);
            return Hash;
        }

        /*
        /// <summary>
        /// DEPRECIATED IN V18.9
        /// Extracts internal metadata of a file to memory and returns a pointer to it if 
        /// successful, or NULL otherwise. The pointer is guaranteed to be valid only at 
        /// the time when you retrieve it. If you wish to do something with the text that 
        /// it points to after your X-Tension returns control to X-Ways Forensics, you 
        /// need to copy it to your own buffer. Unlike GetExtractedMetadata, the file 
        /// must have been opened with XWF_OpenItem because this function reads from the 
        /// file contents, not from data stored in the volume snapshot. The metadata is 
        /// taken from the very file that contains it, for example in the case of zip-
        /// style Office documents from the XML files. Available in v17.7 and later. A 
        /// helper method for XWF_GetMetadata().
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <returns>Returns the metadata if successful, or NULL otherwise.</returns>
        /// <remarks>Version 1.0 coding complete.
        /// - Todo: Needs some serious work and testing.
        /// - Question: What buffer length should be used?</remarks>
        public static string GetMetadata(int itemId)
        {
            string Metadata;
            IntPtr Buffer = Marshal.AllocHGlobal(_volumeNameBufferLength);
            ImportedMethods.XWF_GetMetadata(itemId, Buffer);
            Metadata = Marshal.PtrToStringUni(Buffer);
            Marshal.FreeHGlobal(Buffer);
            return Metadata;
        }
        */

        /// <summary>
        /// Can extract extensive internal metadata of files of various types, exactly as
        /// seen in Details mode in X-Ways Forensics, typically much more than the now
        /// depreciated GetMetadata(). Fills a memory buffer with either null-terminated 
        /// UTF-16 plain text or null-terminated ASCII HTML code, and returns a pointer 
        /// to it. You may parse the buffer to retrieve specific metadata that you need.
        /// The format may theoretically change from one version to the other. You must 
        /// release the allocated memory by passing that pointer to ReleaseMemory() when
        /// you do not need it any more. If no metadata is extracted, the return value is
        /// NULL instead.
        /// 
        /// Unlike the now depreciated GetMetadata(), this function is thread-safe.
        /// Unlike GetExtractedMetadata(), the file must have been opened with OpenItem()
        /// because this function reads from the file contents, not from data stored in 
        /// the volume snapshot. The metadata is taken from the very file that contains 
        /// it, for example in the case of zip-style Office documents, from the XML 
        /// files. Available in v18.9 and later.
        /// </summary>
        /// <param name="item">A pointer to the item.</param>
        /// <param name="options">Options for input and output. The only currently 
        /// defined input flag is 0x01. It tells X-Ways Forensics to extract only a 
        /// subset of the available metadata, as shown in the software in the Metadata column.</param>
        /// <returns></returns>
        /// <remarks>Needs testing.  Needs enum for options.</remarks>
        public static IntPtr GetMetadataEx(IntPtr item, uint options)
        {
            return ImportedMethods.XWF_GetMetadataEx(item, options);
        }

        /// <summary>
        /// Provides a standardized true-color RGB raster image representation for any 
        /// picture file type that is supported internally in X-Ways Forensics (e.g. 
        /// JPEG, GIF, PNG, etc.) with 24 bits per pixel. The result is a pointer to a 
        /// memory buffer, or NULL if not successful (e.g. if not a supported file type 
        /// variant or the file is too corrupt). The caller is responsible for releasing 
        /// the allocated memory buffer when no longer needed, by calling the Windows API 
        /// function VirtualFree(), with parameters dwSize = 0 and dwFreeType = 
        /// MEM_RELEASE. Available in v18.0 and later. A helper method for 
        /// XWF_GetRasterImage().
        /// </summary>
        /// <param name="ImageInformation">A structure of image information.</param>
        /// <returns>Returns a pointer to the raster image.</returns>
        /// <remarks>Needs testing. Also, should verify the provided 
        /// RasterImageInformation structure.</remarks>
        public static IntPtr GetRasterImage(ref RasterImageInformation imageInformation)
        {
            return ImportedMethods.XWF_GetRasterImage(ref imageInformation);
        }

        /// <summary>
        /// Runs a simultaneous search for multiple search terms in the specified volume. 
        /// The volume must be associated with an evidence object. Note that if this 
        /// function is called as part of volume snapshot refinement, it can be called 
        /// automatically for all selected evidence objects if the user applies the 
        /// X-Tension to all selected evidence objects. Must only be called from 
        /// XT_Prepare() or XT_Finalize(). Available in v16.5 and later. A wrapper method
        /// for XWF_Search().
        /// </summary>
        /// <param name="information">Information about the search.</param>
        /// <param name="codePages">The code pages to use.</param>
        /// <returns>Returns result status.</returns>
        /// <remarks>Needs testing.</remarks>
        public static int Search(ref SearchInformation information, 
            ref CodePages codePages)
        {
            return ImportedMethods.XWF_Search(ref information, ref codePages);
        }

        /// <summary>
        /// Used to build the search information for Search().
        /// </summary>
        /// <param name="searchTerms">New-line separated search terms.</param>
        /// <param name="searchOptions">Options for the search.</param>
        /// <returns>Returns search information structure.</returns>
        /// <remarks>Needs testing.</remarks>
        public static SearchInformation CreateSearchInfo(string searchTerms
            , SearchInformationOptions searchOptions)
        {
            SearchInformation Info = new SearchInformation();

            Info.volume = IntPtr.Zero; //the docs say that hVolume should be 0
            Info.searchTerms = searchTerms;
            Info.searchOptions = searchOptions;
            Info.searchWindowLength = 0;
            Info.packedRecordSize = Marshal.SizeOf(Info);
            return Info;
        }

        /// <summary>
        /// Creates a new search term and returns its ID or (if flag 0x01 is specified) 
        /// alternatively returns the ID of an existing search term with the same name, 
        /// if any. Returns -1 in case of an error. The maximum number of search terms in 
        /// a case is currently 8,191 (in v18.5). Use this function if you wish to 
        /// automatically categorize search hits (assign them to different search terms) 
        /// while responding to calls of ProcessSearchHit() or using SetSearchHit(). 
        /// Available in v18.5 and later. A helper method for XWF_AddSearchTerm().
        /// </summary>
        /// <param name="SearchTermName"></param>
        /// <returns>Returns the Id of the new or existing search term or -1 in the case
        /// of an error.</returns>
        /// <remarks>Needs testing.</remarks>
        public static int AddSearchTerm(string searchTermName, SearchTermOptions options
            = SearchTermOptions.None)
        {
            // Fail if no search term name provided.
            if (searchTermName == null || searchTermName == "")
                throw new ArgumentException("Search term name must be provided.");

            return ImportedMethods.XWFAddSearchTerm(searchTermName, options);
        }

        /// <summary>
        /// Retrieves the search term with the specified ID, or null if no search term 
        /// with that ID exists. All search terms have consecutive IDs starting with 0. 
        /// Available in v17.7 and later. A helper method for XWF_GetSearchTerm().
        /// </summary>
        /// <returns>Returns the search term or null if no search exists with the 
        /// specified Id.</returns>
        /// <remarks>Needs testing.</remarks>
        public static string GetSearchTerm(int searchTermId)
        {
            if (searchTermId < 0) return null;

            return ImportedMethods.XWF_GetSearchTerm(searchTermId, IntPtr.Zero);
        }

        /// <summary>
        /// Retrieves the total number of search terms. Available in v17.7 and later. A 
        /// helper method for XWF_GetSearchTerm().
        /// </summary>
        /// <returns>Returns the total number of search terms.</returns>
        /// <remarks>Needs testing.</remarks>
        public static int GetSearchTermCount()
        {
            return ImportedMethods.XWF_GetSearchTermCount(-1, IntPtr.Zero);
        }

        /// <summary>
        /// Allows adding events to the internal event hit list of an evidence object. 
        /// The internal event is loaded and accessible only if the evidence object is 
        /// open. Available in v17.6 and later. A helper method for XWF_AddEvent().
        /// </summary>
        /// <param name="information">The event information.</param>
        /// <returns>Returns 1 if the event was successfully added, 2 if deliberatedly 
        /// ignored, or 0 in case of failure to signal that the caller should stop adding
        /// more events.</returns>
        /// <remarks>This one needs a lot of work. Specifically, need to figure out this 
        /// EventInformation structure and how to make sure it is compatible with the 
        /// API. Also should make an enum for the return.</remarks>
        public static int AddEvent(EventInformation information)
        {
            return ImportedMethods.XWF_AddEvent(information);
        }

        /// <summary>
        /// Retrieves information about an event from the internal event hit list of an 
        /// evidence object. The internal event is loaded and accessible only if the 
        /// evidence object is open. The structure will be populated with values as 
        /// described above, except where noted. Available in v18.1 and later. A helper
        /// method for XWF_GetEvent().
        /// </summary>
        /// <param name="EventNumber">The event number.</param>
        /// <returns>Returns the event information.</returns>
        /// <remarks>This one needs a lot of work. Specifically, need to figure out this 
        /// EventInformation structure and how to make sure it is compatible with the 
        /// API.</remarks>
        public static EventInformation GetEvent(uint EventNumber)
        {
            EventInformation Information = new EventInformation();

            return Information;
        }

        /// <summary>
        /// Creates a new or opens an existing evidence file container. Currently only 1 
        /// container can be open at a time for filling. If a container is open already 
        /// when this function is called, it will be closed automatically. Available in 
        /// v16.5 and later. A helper method for XWF_CreateContainer().
        /// </summary>
        /// <param name="containerFileName">The file name to use for the container.
        /// </param>
        /// <param name="options">Container creation options.</param>
        /// <returns>Returns a pointer to the container.</returns>
        /// <remarks>Needs testing.</remarks>
        public static IntPtr CreateContainer(string containerFileName
            , ContainerCreationOptions options)
        {
            // Fail if no container file name is provided.
            if (containerFileName == null || containerFileName == "")
                throw new ArgumentException("Container file name must be provided.");

            return ImportedMethods.XWF_CreateContainer(containerFileName, options
                , IntPtr.Zero);
        }

        /// <summary>
        /// Copies a file to an evidence file container. Available in v16.5 and later. A
        /// helper method for XWF_CopyToContainer().
        /// </summary>
        /// <param name="container">A pointer to the container to copy to.</param>
        /// <param name="item">A pointer to the item that is being copied.</param>
        /// <param name="options">The copy options.</param>
        /// <param name="mode">The copy mode.</param>
        /// <param name="startOffset">For modes that require it, the starting offset. 
        /// Otherwise should be -1.</param>
        /// <param name="endOffset">For modes that require it, the ending offset; 
        /// otherwise should be -1.</param>
        /// <returns>Returns 0 if successful, otherwise an error code. If the error code 
        /// is negative, you should not try to fill the container further.</returns>
        /// <remarks>Needs testing.</remarks>
        public static int CopyToContainer(IntPtr container, IntPtr item, 
            CopyToContainerOptions options, CopyToContainerMode mode, 
            long startOffset = -1, long endOffset = -1)
        {
            // Fail if a zero container provided.
            if (container == IntPtr.Zero) throw new ArgumentException(
                "Zero container provided");

            // Fail if a item container provided.
            if (item == IntPtr.Zero) throw new ArgumentException(
                "Zero item provided");

            return ImportedMethods.XWF_CopyToContainer(container, item, options, mode
                , startOffset, endOffset, IntPtr.Zero);
        }

        /// <summary>
        /// Closes a container. Available in v16.5 and later. A helper method for 
        /// CloseContainer().
        /// </summary>
        /// <param name="container">A pointer to the container to close.</param>
        /// <returns>Returns true if succesful, otherwise false.</returns>
        /// <remarks>Needs testing.</remarks>
        public static bool CloseContainer(IntPtr container)
        {
            // Fail if a zero container provided.
            if (container == IntPtr.Zero) throw new ArgumentException(
                "Zero container provided");

            // Return true if the API call yields 1.
            if (ImportedMethods.XWF_CloseContainer(container, IntPtr.Zero) == 1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Outputs the specified message in the Messages window. You may use this 
        /// function for example to alert the user of errors or to output debug 
        /// information. A helper method for XWF_OutputMessage().
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="options">Output message options.</param>
        /// <remarks>Needs testing.</remarks>
        public static void OutputMessage(string message, OutputMessageOptions options = 
            OutputMessageOptions.None)
        {
            // Fail if a null string message is provided.
            if (message == null) throw new ArgumentException(
                "Null string provided");

            string tab;

            if ((options & OutputMessageOptions.Level4) != 0)
            {
                tab = new string(' ', 12);
            }
            else if ((options & OutputMessageOptions.Level3) != 0)
            {
                tab = new string(' ', 8);
            }
            else if ((options & OutputMessageOptions.Level2) != 0)
            {
                tab = new string(' ', 4);
            }
            else
            {
                tab = "";
            }

            // Tab out the message.
            message = tab + message;

            // Build the API-specific options.
            OutputMessageOptions_XWF xwf_options = OutputMessageOptions_XWF.None;

            if ((options & OutputMessageOptions.NoLineBreak) != 0)
                xwf_options |= OutputMessageOptions_XWF.NoLineBreak;

            if ((options & OutputMessageOptions.DoNotLogError) != 0)
                xwf_options |= OutputMessageOptions_XWF.DoNotLogError;

            if ((options & OutputMessageOptions.Ansi) != 0)
                xwf_options |= OutputMessageOptions_XWF.Ansi;

            ImportedMethods.OutputMessage(message, xwf_options);

            // Output a blank line after the message for headers.
            if ((options & OutputMessageOptions.Header) != 0)
            {
                ImportedMethods.OutputMessage("");
            }
        }

        /// <summary>
        /// Requests user input through a dialog window provided by X-Ways Forensics. 
        /// This input can be text (e.g. a password) or can be directly interpreted as an 
        /// integer number. Available in v18.5 and later. A helper method for
        /// XWF_GetUserInput().
        /// </summary>
        /// <param name="message">Presented to the user (truncated if too long) as an 
        /// explanation for what is required.</param>
        /// <param name="suggestedInput">Displayed in the input box as a suggestion.
        /// </param>
        /// <param name="options">User input options.</param>
        /// <returns></returns>
        /// <remarks>Needs testing.</remarks>
        public static string GetUserInput(string message, string suggestedInput = null,
            UserInputOptions options = UserInputOptions.Unused)
        {
            // Fail if a null string message is provided.
            if (message == null) throw new ArgumentException(
                "Null string provided");

            // Allocate a buffer to receive the user input
            IntPtr Buffer = Marshal.AllocHGlobal((int)_userInputLength);

            // If caller has provided suggest input, put it in the buffer.
            if (suggestedInput != null)
            {
                Buffer = Marshal.StringToHGlobalUni(suggestedInput);
            }

            // Call the API function and get the user input, and clean up.
            long InputLength = ImportedMethods.XWF_GetUserInput(message, Buffer, 
                _userInputLength, options);
            string UserInput = Marshal.PtrToStringUni(Buffer, (int) InputLength);
            Marshal.FreeHGlobal(Buffer);

            return UserInput;
        }

        /// <summary>
        /// Creates a progress indicator window with the specified caption. You should 
        /// call PeekMessage, TranslateMessage, DispatchMessage occasionally to allow for 
        /// the main window to remain responsive. You must not use any of the progress 
        /// indicator methods when implementing XT_ProcessItem or XT_ProcessItemEx or 
        /// when calling methods that create a progress bar themselves. A helper method
        /// for XWF_ShowProgress().
        /// </summary>
        /// <param name="progressCaption">Caption to display in the progress window.
        /// </param>
        /// <param name="options">Progress window options.</param>
        /// <remarks>Needs testing.</remarks>
        public static void ShowProgress(string progressCaption, ProgressIndicatorOptions
            options = ProgressIndicatorOptions.None)
        {
            // Fail if a null string is provided.
            if (progressCaption == null) throw new ArgumentException(
                "Null string provided");

            ImportedMethods.XWF_ShowProgress(progressCaption, options);
        }

        /// <summary>
        /// Set the progress percentage. A helper method for XWF_SetProgressPrecentage().
        /// </summary>
        /// <param name="percent">The percent to display.</param>
        /// <remarks>Needs testing.</remarks>
        public static void SetProgressPercentage(uint progressPercentage)
        {
            ImportedMethods.XWF_SetProgressPercentage(progressPercentage);
        }

        /// <summary>
        /// Displays descriptive text about the progress. A helper method for 
        /// XWF_SetProgressDescription().
        /// </summary>
        /// <param name="description">The description to display.</param>
        /// <remarks>Needs testing.</remarks>
        public static void SetProgressDescription(string description)
        {
            // Fail if a null string is provided.
            if (description == null) throw new ArgumentException(
                "Null string provided");

            ImportedMethods.XWF_SetProgressDescription(description);
        }

        /// <summary>
        /// When a progress indicator window is on the screen and you call PeekMessage 
        /// etc. regularly, you can use this function to check whether the user wants to 
        /// abort the operation. A helper method for XWF_ShouldStop().
        /// </summary>
        /// <returns>Returns true if the user wants to stop; otherwise false.</returns>
        /// <remarks>Needs testing.</remarks>
        public static bool ShouldStop()
        {
            return ImportedMethods.XWF_ShouldStop();
        }

        /// <summary>
        /// Closes the progress indicator window. A helper method for XWF_HideProgress().
        /// </summary>
        /// <remarks>Needs testing.</remarks>
        public static void HideProgress()
        {
            ImportedMethods.XWF_HideProgress();
        }

        /// <summary>
        /// Call this function to release a buffer allocated by X-Ways Forensics if 
        /// instructed to do so in the description of another method.
        /// </summary>
        /// <param name="buffer">The buffer to release.</param>
        /// <returns>Returns true if successful; otherwise false.</returns>
        /// <remarks>Needs testing.</remarks>
        public static bool ReleaseMemory(IntPtr buffer)
        {
            // Fail if a zero buffer provided.
            if (buffer == IntPtr.Zero) throw new ArgumentException(
                "Zero buffer provided");

            return ImportedMethods.XWF_ReleaseMem(buffer);
        }

        /// <summary>
        /// Converts a byte array to a hex string representation. For example, 
        /// \x00\x01\x02\x03 would become "00010203".
        /// </summary>
        /// <param name="data">The byte array to convert.</param>
        /// <returns>Returns a hex string representation of the provided byte array.
        /// </returns>
        /// <remarks>Version 1.0 coding complete.</remarks>
        public static string Hexlify(byte[] data)
        {
            // String should be double length of the byte array since each byte will be
            // represented by two characters.
            StringBuilder hex = new StringBuilder(data.Length * 2);

            // Append each hex byte representation to the string.
            foreach (byte b in data)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="externalFilename"></param>
        /// <param name="parentItemId"></param>
        /// <param name="keepExternalFile"></param>
        /// <returns></returns>
        public static int CreateFileFromExternalFile(string name, string externalFilename
            , int parentItemId, bool keepExternalFile = false)
        {
            IntPtr extFilenamePtr = Marshal.StringToHGlobalUni(externalFilename);

            int itemId = CreateFile(name
                , CreateFileOptions.AttachExternalFile
                    | (keepExternalFile ? CreateFileOptions.KeepExternalFile : 0)
                , parentItemId
                , extFilenamePtr);

            Marshal.FreeHGlobal(extFilenamePtr);
            return itemId;
        }
    }
}

