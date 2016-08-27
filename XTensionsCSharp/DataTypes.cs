using System;
using System.Runtime.InteropServices;

namespace XTensions
{
    /// <summary>
    /// How to add a comment with XWF_AddComment.
    /// </summary>
    public enum AddCommentMode : uint
    {
        /// <summary>Replace any existing comment.</summary>
        Replace = 0u,
        /// <summary>Add to existing comments</summary>
        Append = 1u,
        /// <summary>Add to existing comments using a line break to delimit.</summary>
        AppendLine = 2u
    }

    /// <summary>
    /// Flags for XWF_AddToReportTable.
    /// </summary>
    [Flags]
    public enum AddToReportTableOptions : uint
    {
        /// <summary>Show as created by the application, not the examiner.</summary>
        CreatedByApplication = 0x01u,
        /// <summary>Select for inclusion in the report.</summary>
        IncludeInReport = 0x02u,
        /// <summary>Select for filtering.</summary>
        Filtering = 0x04u,
        /// <summary>Select for future manual report table associations.</summary>
        FutureManualReportTableAssociations = 0x08u,
    }

    /// <summary>
    /// Indicates the result of associating a file with a report table.
    /// </summary>
    public enum AddToReportTableResult : uint
    {
        /// <summary>There was an error in the assocation.</summary>
        Failure = 0,
        /// <summary>File was successfully and newly associated with the report table.
        /// </summary>
        NewAssociation = 1,
        /// <summary>The file to report table association already exists</summary>
        ExistingAssociation = 2
    }

    /// <summary>
    /// Capabilities of the calling program.
    /// </summary>
    [Flags]
    public enum CallingProgramCapabilities : uint
    {
        /// <summary>X-Ways Forensics</summary>
        XWaysForensics = 0x00000001u,
        /// <summary>WinHex</summary>
        WinHex = 0x00000002u,
        /// <summary>X-Ways Investigator</summary>
        XWaysInvestigator = 0x00000004u,
        /// <summary>Beta Version</summary>
        BetaVersion = 0x00000008u,
        /// <summary>Check if the API accepts calling application (v16.5+).</summary>
        APICompatibilityQuickCheck = 0x00000020u,
        /// <summary>Just prepare for XT_About (v16.5+).</summary>
        AboutOnly = 0x00000040u
    }

    /// <summary>
    /// Case property types.
    /// </summary>
    public enum CasePropertyType : uint
    {
        /// <summary>Case title.</summary>
        CaseTitle = 1u,
        /// <summary>Case examiner.</summary>
        CaseExaminer = 3u,
        /// <summary>Case file path.</summary>
        CaseFilePath = 5u,
        /// <summary>Case directory.</summary>
        CaseDirectory = 6u
    }

    /// <summary>
    /// Container creation flags; used with XWF_CreateContainer.
    /// </summary>
    [Flags]
    public enum ContainerCreationOptions : uint
    {
        /// <summary>Opens an existing container; all other flags are ignored.</summary>
        XWF_CTR_OPEN = 0x00000001u
        /// <summary>Use new XWFS2 file system</summary>
        , XWF_CTR_XWFS2 = 0x00000002u //
        /// <summary></summary>
        , XWF_CTR_SECURE = 0x00000004u //mark this container as to be filled indirectly/secure
        /// <summary></summary>
        , XWF_CTR_TOPLEVEL = 0x00000008u //include evidence object names as top directory level
        /// <summary></summary>
        , XWF_CTR_INCLDIRDATA = 0x00000010u //include directory data
        /// <summary></summary>
        , XWF_CTR_FILEPARENTS = 0x00000020u //allow files as parents of files
        /// <summary></summary>
        , XWF_CTR_USERREPORTTABLES = 0x00000100u //export associations with user-created report table
        /// <summary></summary>
        , XWF_CTR_SYSTEMREPORTTABLES = 0x00000200u //export associations with system-created report tables (currently requires 0x100)
        /// <summary></summary>
        , XWF_CTR_ALLCOMMENTS = 0x00000800u //pass on comments
        /// <summary></summary>
        , XWF_CTR_OPTIMIZE1 = 0x00001000u //optimize for > 1,000 items
        /// <summary></summary>
        , XWF_CTR_OPTIMIZE2 = 0x00002000u //optimize for > 50,000 items
        /// <summary></summary>
        , XWF_CTR_OPTIMIZE3 = 0x00004000u //optimize for > 250,000 items
        /// <summary></summary>
        , XWF_CTR_OPTIMIZE4 = 0x00008000u //optimize for > 1 million items
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CopyToContainerMode : uint
    {
        /// <summary></summary>
        LogicalContents = 0u //copy logical file contents only
        /// <summary></summary>
        , PhysicalContents = 1u //copy physical file contents (not supported)
        /// <summary></summary>
        , LogicalContentsAndSlack = 2u //logical contents and file slack separately
        /// <summary></summary>
        , Slack = 3u //copy slack only
        /// <summary></summary>
        , Range = 4u //copy range only (the last 2 parameters, which are otherwise ignored)
        /// <summary></summary>
        , Metadata = 5u //copy metadata only
    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum CopyToContainerOptions : uint
    {
        /// <summary></summary>
        RecreateOriginalPath = 0x00000001u //recreate full original path
        /// <summary></summary>
        , IncludeParentItemData = 0x00000002u //include parent item data (requires flag 0x1)
        /// <summary></summary>
        , StoreHash = 0x00000004u //store hash value in container
    }

    /// <summary>
    /// File creation flags. Flags 2 and 4 are mutually exclusive, and 8 can only be 
    /// combined with 4. If neither of the flags 2, 4, or 8 are specified, pSourceInfo
    /// is ignored and XWFCreateFile works exactly like XWFCreateItem.</summary>
    [Flags]
    public enum CreateFileOptions : uint
    {
        /// <summary>For performance reasons, set if many more items are expected to be 
        /// created.</summary>
        MoreItemsExpected = 0x00000001u,
        /// <summary>Create a file that is defined as an excerpt from its parent, where 
        /// pExtraInfo points to a 64-bit start offset in the parent file.</summary>
        ExcerptFromParent = 0x00000002u,
        /// <summary>Attach an external file, and pExtraInfo is an LPWSTR pointer to the 
        /// path of that file. Can only be used with the volume snapshot of an evidence
        /// object.</summary>
        AttachExternalFile = 0x00000004u,
        /// <summary>Keep the external file designated if its still needed after calling 
        /// this function.</summary>
        KeepExternalFile = 0x00000008u,
    }

    [Flags]
    public enum CreateItemOptions : uint
    {
        /// <summary>For performance reasons, set if many more items are expected to be 
        /// created.</summary>
        MoreItemsExpected = 0x00000001u
    }

    /// <summary>
    /// Evidence object type.
    /// </summary>
    public enum EvidenceObjectCategory : uint
    {
        /// <summary>File object.</summary>
        File = 0u,
        /// <summary>Disk image object.</summary>
        Image = 1u,
        /// <summary>Memory dump object.</summary>
        MemoryDump = 2u,
        /// <summary>Directory object.</summary>
        Directory = 3u,
        /// <summary>Physical disk object.</summary>
        Disk = 4u
    }

    [Flags]
    public enum EvidenceObjectReportTableAssociationsOptions : uint
    {
        SortedByItemID = 0x000000001u
    }

    /// <summary>
    /// Evidence object type IDs.
    /// </summary>
    public enum EvidenceObjectType : int
    {
        /// <summary>File based type.</summary>
        FileBased = 0,
        /// <summary>Disk image.</summary>
        DiskImage = 1000,
        /// <summary>Memory dump.</summary>
        MemoryDump = 1001,
        /// <summary>Directory.</summary>
        Directory = 1002,
        /// <summary>File.</summary>
        File = 1003,
        /// <summary>Drive letter A:</summary>
        DriveLetterA = 1,
        /// <summary>Drive letter B:</summary>
        DriveLetterB = 2,
        /// <summary>Drive letter C:</summary>
        DriveLetterC = 3,
        /// <summary>Drive letter D:</summary>
        DriveLetterD = 4,
        /// <summary>Drive letter E:</summary>
        DriveLetterE = 5,
        /// <summary>Drive letter F:</summary>
        DriveLetterF = 6,
        /// <summary>Drive letter G:</summary>
        DriveLetterG = 7,
        /// <summary>Drive letter H:</summary>
        DriveLetterH = 8,
        /// <summary>Drive letter I:</summary>
        DriveLetterI = 9,
        /// <summary>Drive letter J:</summary>
        DriveLetterJ = 10,
        /// <summary>Drive letter K:</summary>
        DriveLetterK = 11,
        /// <summary>Drive letter L:</summary>
        DriveLetterL = 12,
        /// <summary>Drive letter M:</summary>
        DriveLetterM = 13,
        /// <summary>Drive letter N:</summary>
        DriveLetterN = 14,
        /// <summary>Drive letter O:</summary>
        DriveLetterO = 15,
        /// <summary>Drive letter P:</summary>
        DriveLetterP = 16,
        /// <summary>Drive letter Q:</summary>
        DriveLetterQ = 17,
        /// <summary>Drive letter R:</summary>
        DriveLetterR = 18,
        /// <summary>Drive letter S:</summary>
        DriveLetterS = 19,
        /// <summary>Drive letter T:</summary>
        DriveLetterT = 20,
        /// <summary>Drive letter U:</summary>
        DriveLetterU = 21,
        /// <summary>Drive letter V:</summary>
        DriveLetterV = 22,
        /// <summary>Drive letter W:</summary>
        DriveLetterW = 23,
        /// <summary>Drive letter X:</summary>
        DriveLetterX = 24,
        /// <summary>Drive letter Y:</summary>
        DriveLetterY = 25,
        /// <summary>Drive letter Z:</summary>
        DriveLetterZ = 26,
        /// <summary>Physical hard disk 1.</summary>
        PhysicalHD1 = -3,
        /// <summary>Physical hard disk 2.</summary>
        PhysicalHD2 = -4,
        /// <summary>Physical hard disk 3.</summary>
        PhysicalHD3 = -5,
        /// <summary>Physical hard disk 4.</summary>
        PhysicalHD4 = -6,
        /// <summary>Physical hard disk 5.</summary>
        PhysicalHD5 = -7,
        /// <summary>Physical hard disk 6.</summary>
        PhysicalHD6 = -8,
        /// <summary>Physical hard disk 7.</summary>
        PhysicalHD7 = -9,
        /// <summary>Physical hard disk 8.</summary>
        PhysicalHD8 = -10,
        /// <summary>Physical hard disk 9.</summary>
        PhysicalHD9 = -11,
        /// <summary>Physical hard disk 10.</summary>
        PhysicalHD10 = -12
    }

    [Flags]
    public enum EvidenceOpenOptions : uint
    {
        None = 0x00000000u,
        OpenWithoutCheckingSource = 0x00000001u,
        OpenVolumeSnapshotReadOnly = 0x00000002u
    }

    [Flags]
    public enum EvidenceProperties : uint
    {
        /// <summary>Data window active.</summary>
        DataWindowActive = 0x00000001u,
        /// <summary>Data window open.</summary>
        DataWindowOpen = 0x00000002u,
        /// <summary>Flagged.</summary>
        Flagged = 0x00000004u,
        /// <summary>Selected for operations.</summary>
        SelectedForOperations = 0x00000008u,
        /// <summary>Selected for recursive view.</summary>
        SelectedForRecursiveView = 0x00000010u,
        /// <summary>Expanded in case tree.</summary>
        ExpandedInCaseTree = 0x00000020u,
        /// <summary>Has no children.</summary>
        HasNoChildren = 0x00000040u,
        /// <summary>Is an evidence file container.</summary>
        IsEvidenceFileContainer = 0x00000100u,
        /// <summary>Is a deleted partition.</summary>
        IsDeletedPartition = 0x00000200u,
        /// <summary>Has optical disc icon.</summary>
        OpticalDiscIcon = 0x00000400u,
        /// <summary>Has RAM icon.</summary>
        RAMIcon = 0x00000800u,
        /// <summary>Is a dynamic disk.</summary>
        IsDynamicDisk = 0x00001000u,
        /// <summary>Evidence object is just a single file in a directory.</summary>
        IsJustSingleFileInADirectory = 0x00002000u,
        /// <summary>Index is available.</summary>
        IndexAvailable = 0x00010000u,
        /// <summary>Logging is enabled.</summary>
        LoggingEnabled = 0x00020000u,
        /// <summary>Annotations are highlighted.</summary>
        AnnotationsHighlighted = 0x00040000u,
        /// <summary>Warned of weird image file size already.</summary>
        WarnedOfWeirdImageFileSize = 0x00080000u,
        /// <summary>Suppress "size of evidence object has changed".</summary>
        SuppressSizeOfEvidenceObjectHasChanged = 0x00100000u,
    }

    public enum EvidencePropertyType : uint
    {
        /// <summary>Evidence object number.</summary>
        EvidenceObjectNumber = 0u,
        /// <summary>Evidence object ID.</summary>
        EvidenceObjectID = 1u,
        /// <summary>Parent evidence object ID.</summary>
        ParentEvidenceObjectID = 2u,
        /// <summary>Title.</summary>
        Title = 6u,
        /// <summary>Extended title.</summary>
        ExtendedTitle = 7u,
        /// <summary>Abbreviated title.</summary>
        AbbreviatedTitle = 8u,
        /// <summary>Internal name.</summary>
        InternalName = 9u,
        /// <summary>Description.</summary>
        Description = 10u,
        /// <summary>Examiner comments.</summary>
        ExaminerComments = 11u,
        /// <summary>Internally used directory.</summary>
        InternallyUsedDirectory = 12u,
        /// <summary>Output directory.</summary>
        OutputDirectory = 13u,
        /// <summary>Size (in bytes).</summary>
        SizeInBytes = 16u,
        /// <summary>Volume snapshot file count.</summary>
        VolumeSnapshotFileCount = 17u,
        /// <summary>Flags.</summary>
        Flags = 18u,
        /// <summary>File system identifier.</summary>
        FileSystemIdentifier = 19u,
        /// <summary>Hash #1 type.</summary>
        HashType = 20u,
        /// <summary>Hash #1 value.</summary>
        HashValue = 21u,
        /// <summary>Creation time (when the evidence object was added to the case).
        /// </summary>
        CreationTime = 32u,
        /// <summary>Modification time.</summary>
        ModificationTime = 33u,
        /// <summary>Hash #2 type.</summary>
        HashType2 = 40u,
        /// <summary>Hash #2 value.</summary>
        HashValue2 = 41u
    }

    public enum HashType : int
    {
        Undefined = 0,
        CS8 = 1,
        CS16 = 2,
        CS32 = 3,
        CS64 = 4,
        CRC16 = 5,
        CRC32 = 6,
        MD5 = 7,
        SHA1 = 8,
        SHA256 = 9,
        RIPEMD128 = 11,
        RIPEMD160 = 12,
        MD4 = 13,
        ED2K = 14,
        Adler32 = 15,
        TigerTreeHash = 16,
        Tiger128 = 17,
        Tiger160 = 18,
        Tiger192 = 19
    }

    public enum ItemClassifiction : uint
    {
        NormalFile = 0u,
        HFSResourceFork = 4u,
        NTFSAlternateDataStream = 8u,
        NTFSNonDirectoryIndex = 0x0Au,
        NTFSBitmapAttribute = 0x0Bu,
        NTFSGeneralLoggedUtilityStream = 0x10u,
        NTFSEFSLoggedUtilityStream = 0x11u,
        EmailRelated = 0xF5,
        Excerpt = 0xF6,
        ManuallyAttached = 0xF7,
        VideoStill = 0xF8,
        EmailAttachment = 0xF9,
        EmailMessage = 0xFA,
        INDXRecordRemnant = 0xFD,
        SessionRootDirectoryInCDFSOrUDF = 0xFEu
    }

    public enum ItemDeletionStatus : uint
    {
        Existing = 0u,
        PreviouslyExistingPossiblyRecoverable = 1u,
        PreviouslyExistingFirstClusterOverwrittenOrUnknown = 2u,
        RenamedOrMovedPossiblyRecoverable = 3u,
        RenamedOrMovedFirstClusterOverwrittenOrUknown = 4u
    }

    [Flags]
    public enum ItemInformationOptions : ulong
    {
        IsDirectory = 0x00000001u,
        hasChildren = 0x00000002u,
        hasSubDirectories = 0x00000004u,
        IsVirtualItem = 0x00000008u,
        HiddenByExaminer = 0x00000010u,
        Tagged = 0x00000020u,
        TaggedPartially = 0x00000040u,
        ViewedByExaminer = 0x00000080u,
        FileSystemTimestampsNotInUTC = 0x00000100u,
        InternalCreationTimestampNotInUTC = 0x00000200u,
        FATTimestamps = 0x00000400u,
        OriginatesFromNTFS = 0x00000800u,
        UNIXWorldAttributes = 0x00001000u,
        HasExaminerComment = 0x00002000u,
        HasExtractedMetadata = 0x00004000u,
        FileContentsTotallyUnknown = 0x00008000u,
        FileContentsPartiallyUnknown = 0x00010000u,
        OriginalFileExcerpt = 0x00020000u,
        Hash1AlreadyComputed = 0x00040000u,
        HasDuplicates = 0x00080000u,
        Hash2AlreadyComputed = 0x00100000u,
        KnownGoodHashCategory = 0x00200000u,
        KnownBadHashCategory = 0x00400000u,
        FoundInVolumeShadowCopy = 0x00800000u,
        DeletedFilesWithKnownOriginalContents = 0x01000000u,
        FileFormatConsistencyOK = 0x02000000u,
        FileFormatConsistencyNotOK = 0x04000000u,
        FileArchiveAlreadyExplored = 0x10000000u,
        EmailArchiveOrVideoAlreadyUncovered = 0x20000000u,
        EmbeddedDataAlreadyUncovered = 0x40000000u,
        MetadataExtractionAlreadyApplied = 0x80000000u,
        FileEmbeddedInOtherFileLinearly = 0x100000000u,
        FileContentsStoredExternally = 0x200000000,
        AlternativeDataAvailable = 0x400000000
    }

    /// <summary>
    /// Item information type.
    /// </summary>
    public enum ItemInformationType : int
    {
        /// <summary>Original ID.</summary>
        OriginalId = 1,
        /// <summary>Attributes.</summary>
        Attributes = 2,
        /// <summary>Flags.</summary>
        Options = 3,
        /// <summary>Deletion.</summary>
        DeletionStatus = 4,
        /// <summary>e.g. extracted e-mail, alternate data stream, etc.</summary>
        Classification = 5,
        /// <summary>Hard-link count.</summary>
        LinkCount = 6,
        ColorAnalysis = 7,
        /// <summary>Count of existing recursive child objects that are files.</summary>
        FileCount = 11,
        EmbeddedOffset = 16,
        /// <summary>Creation timestamp.</summary>
        CreationTime = 32,
        /// <summary>Modification timestamp.</summary>
        ModificationTime = 33,
        /// <summary>Last access timestamp.</summary>
        LastAccessTime = 34,
        /// <summary>Entry modification timestamp.</summary>
        EntryModificationTime = 35,
        /// <summary>Deleation timestamp.</summary>
        DeletionTime = 36,
        /// <summary>Internal creation timestamp.</summary>
        InternalCreationTime = 37,
        /// <summary>Indicates only options that should be set; others should remain 
        /// unchanged.</summary>
        SetOptions = 64,
        /// <summary>Indicates options that should be removed; others should remain 
        /// unchanged.</summary>
        RemoveOptions = 65,
    }

    /// <summary>
    /// Open mode for items.
    /// </summary>
    [Flags]
    public enum ItemOpenModes : uint
    {
        /// <summary>Open item only.</summary>
        LogicalContents = 0x00000000u,
        /// <summary>Open item and slack space.</summary>
        LogicalContentsAndSlack = 0x00000001u,
        /// <summary>Suppress error messages.</summary>
        SuppressErrors = 0x00000002u,
        /// <summary>Prefer alternative data (such as thumbnail) if available.</summary>
        PreferAlternative = 0x00000008u,
        /// <summary>Open alternative data, failing if not available.</summary>
        OpenAlternative = 0x00000010u,
    }

    /// <summary>
    /// The type of size to get for a volume or item.
    /// </summary>
    public enum ItemSizeType : uint
    {
        /// <summary>Retrieve the physical size of the volume or item.</summary>
        PhysicalSize = 0u,
        /// <summary>Retrieve the logical size of the volume or item, which may be
        /// different from the size that was known in the volume snapshot before the
        /// file was opened.</summary>
        LogicalSize = 1u,
        /// <summary>Retrieve the valid data length (a.k.a. initialized size of the data
        /// stream, which may be available from NTFS, exFAT, XWFS, XWFS2).</summary>
        ValidDataLength = 2u
    }

    public enum ItemTypeCategory : int
    {
        Error = -1,
        NotVerified = 0,
        TooSmall = 1,
        TotallyUnknown = 2,
        Confirmed = 3,
        NotConfirmed = 4,
        NewlyIdentified = 5
    }

    public enum OutputMessageLevel : int
    {
        Level1 = 0,
        Level2 = 1,
        Level3 = 2,
        Level4 = 3
    }

    [Flags]
    public enum OutputMessageOptions : uint
    {
        None = 0,
        NoLineBreak = 0x01u,
        DoNotLogError = 0x02u,
        Ansi = 0x04u,
        Header = 0x08u,
        Level1 = 0x010u,
        Level2 = 0x020u,
        Level3 = 0x040u,
        Level4 = 0x080u,
    }
    
    /// <summary>
    /// API-specific output message options.
    /// </summary>
    [Flags]
    public enum OutputMessageOptions_XWF : uint
    {
        None = 0x00000000u,
        /// <summary>Append without a line break.</summary>
        NoLineBreak = 0x00000001u,
        /// <summary>Don't log this error message.</summary>
        DoNotLogError = 0x00000002u,
        /// <summary>Message points to an ANSI (not UNICODE) string (v16.5+).</summary>
        Ansi = 0x00000004u
    }

    /// <summary>
    /// Progress indicator flags; used with ShowProgress().
    /// </summary>
    [Flags]
    public enum ProgressIndicatorOptions : uint
    {
        None = 0,
        /// <summary>Show just the window, no actual progress bar.</summary>
        WindowOnly = 0x01u,
        /// <summary>Do not allow the user to interrupt the operation.</summary>
        DisallowInterrupting = 0x02u,
        /// <summary>Show window immediately.</summary>
        ShowImmediately = 0x04u,
        /// <summary>Double-confirm abort.</summary>
        DoubleConfirmAbort = 0x08u,
        /// <summary>Prevent logging.</summary>
        PreventLogging = 0x010u
    }

    /// <summary>
    /// Raster image creation options.
    /// </summary>
    [Flags]
    public enum RasterImageOptions : uint
    {
        None = 0,
        /// <summary>Get a memory buffer that starts with an appropriate Windows Bitmap 
        /// header.</summary>
        GetWindowsBitmapBuffer = 0x01u,
        /// <summary>Align line offsets at 4-byte boundaries.</summary>
        AlignOnLONGBoundaries = 0x02u,
        /// <summary>Vertically flip image, physically (reverse the order of pixel lines 
        /// in the memory buffer).</summary>
        VerticallyFlipImagePhysically = 0x04u,
        /// <summary>Create a standard Windows BMP image (suitable combination of flags 
        /// 0x01, 0x02, and 0x04).</summary>
        CreateStandardWindowsBMP = 0x07u,
        /// <summary>Vertically flip image, logically (only in conjunction with 0x01, 
        /// using a negative height in the BMP header)</summary>
        VerticallyFlipImageLogically = 0x08u,
        /// <summary>Horizontally flip image, physically (reverse the order of the pixels
        /// in each line in the memory buffer)</summary>
        HorizontallyFlipImagePhysically = 0x10u,
    }

    /// <summary>
    /// Report table information flags.
    /// </summary>
    [Flags]
    public enum ReportTableInformationOptions : long
    {
        /// <summary>Used for versions below v18.1.</summary>
        None = 0x00000000u,
        /// <summary>Created internally by the application.</summary>
        ApplicationCreated = 0x00000001u,
        /// <summary>Created by the user.</summary>
        UserCreated = 0x00000002u,
        /// <summary>Represents a search term.</summary>
        RepresentsSearchTerm = 0x00000080u
    }

    /// <summary>
    /// Search hit information flags.
    /// </summary>
    [Flags]
    public enum SearchHitInformationProperties : ushort
    {
        /// <summary>Resides in the text that was extracted from the file.</summary>
        ResidesInTheExtractedText = 0x0001,
        /// <summary>A notable search hit.</summary>
        Notable = 0x0002,
        /// <summary>Search hit marked as deleted; set to discard the hit.</summary>
        Deleted = 0x0008,
        /// <summary>An index search hit.</summary>
        IndexSearchHit = 0x0040,
        /// <summary>Search hit within slack space.</summary>
        InSlackSpace = 0x0080
    }

    /// <summary>
    /// Search options.
    /// </summary>
    [Flags]
    public enum SearchInformationOptions : uint
    {
        /// <summary>Logical search instead of physical search (only logical search 
        /// currently available)</summary>
        LogicalSearch = 0x00000001u,
        /// <summary>Tagged objects in volume snapshot only</summary>
        TaggedObjectsOnly = 0x00000004u,
        /// <summary>Case sensitive, i.e. match the case.</summary>
        CaseSensitive = 0x00000010u,
        /// <summary>Match whole words only.</summary>
        WholeWordsOnly = 0x00000020u,
        /// <summary>Use GREP syntax.</summary>
        GREP = 0x00000040u,
        /// <summary>Allow overlapping hits.</summary>
        AllowOverlappingHits = 0x00000080u,
        /// <summary>Cover slack space.</summary>
        CoverSlackSpace = 0x00000100u,
        /// <summary>Cover slack/free space transition</summary>
        CoverSlackSpaceFreeSpaceTransition = 0x00000200u,
        /// <summary>Decode text in standard file types</summary>
        DecodeTextInStandardFileTypes = 0x00000400u,
        /// <summary>Decode text in specified file types (not yet supported)</summary>
        DecodeTextInSpecifiedFileTypes = 0x00000800u,
        /// <summary>Only one hit per file needed.</summary>
        OnlyOneHitPerFile = 0x00001000u,
        /// <summary>Omit files classified as irrelevant.</summary>
        OmitIrrelevantFiles = 0x00010000u,
        /// <summary>Omit hidden files.</summary>
        OmitHiddenFiles = 0x00020000u,
        /// <summary>Omit files that are filtered out.</summary>
        OmitFilteredFiles = 0x00040000u,
        /// <summary>Recommendable data reduction.</summary>
        RecommendableDataReduction = 0x00080000u,
        /// <summary>Omit directories.</summary>
        OmitDirectories = 0x00100000u,
        /// <summary>XT_ProcessSearchHit (if exported) will process each hit.</summary>
        ProcessEachSearchHit = 0x01000000u,
        /// <summary>Display search hit list when the search completes.</summary>
        DisplaySearchHitListOnCompletion = 0x04000000u
    }

    /// <summary>
    /// Search options.
    /// </summary>
    [Flags]
    public enum SearchOptions : uint
    {
        /// <summary>Case sensitive; i.e. match the case.</summary>
        CaseSensitive = 0x00000010u,
        /// <summary>Match whole words only.</summary>
        MatchWholeWordsOnly = 0x00000020u,
        /// <summary>Use GREP syntax.</summary>
        GREPSyntax = 0x00000040u,
        /// <summary>Match whole words only for search terms that are specially marked.
        /// </summary>
        SpecificMatchWholeWordsOnly = 0x00004000u,
        /// <summary>Use GREP syntax only for search terms starting with "grep:".
        /// </summary>
        SpecificGREPSyntax = 0x00008000u
    }

    [Flags]
    public enum SearchTermOptions : uint
    {
        None = 0,
        /// <summary>Allow re-use of existing search term of the same name.</summary>
        ReUseExisting = 0x01u,
        /// <summary>Mark search term as a search term for user search hits.</summary>
        UserSearchTerm = 0x02u
    }
    
    [Flags]
    public enum SectorIOOptions : uint
    {
        None = 0,
        /// <summary>NOT YET IMPLEMENTED. If not yet set, read is assumed.</summary>
        Write = 0x01,
        /// <summary>Stop on I/O error and return the number of successfully read sectors
        /// (if not set, X­Ways Forensics will try to continue and fill unreadable sectors
        /// with an ASCII pattern and return the total number of sectors tried).
        /// </summary>
        StopOnError = 0x02,
        /// <summary>Output error messages in the GUI in case of I/O errors 0x08: do not 
        /// trigger any pending skeleton image acquisition through a read operation.
        /// </summary>
        OutputErrorMessages = 0x04,
        /// <summary>Check whether the entire range of sectors is defined as sparse in a 
        /// lower abstraction layer, for performance benefits 0x20 (returned): the entire 
        /// range of sectors targeted is sparse, so you may ignore it, and the buffer was 
        /// not filled</summary>
        CheckSparse = 0x10    
    }

    /// <summary>
    /// Special item types.
    /// </summary>
    public enum SpecialItemType : int
    {
        Ununsed = 0,
        /// <summary>Root directory.</summary>
        RootDirectory = 1,
        /// <summary>Path unknown directory.</summary>
        PathUnknownDirectory = 2,
        /// <summary>Carved files directory.</summary>
        CarvedFilesDirectory = 4,
        /// <summary>Free space file.</summary>
        FreeSpaceFile = 5,
        /// <summary>System volume information directory.</summary>
        SystemVolumeInformationDirectory = 11,
        /// <summary>Windows.edb file.</summary>
        WindowsEDBFile = 12
    }

    /// <summary>
    /// User input options.
    /// </summary>
    [Flags]
    public enum UserInputOptions : uint
    {
        Unused = 0,
        /// <summary>Requires the user to enter a positive integer number. That integer 
        /// number is returned by this function. lpBuffer and nBufferLen must be 
        /// NULL/zero. </summary>
        RequirePositiveNumber = 1,
        /// <summary>Empty user input allowed. Mutually exclusive with the previous flag.
        /// </summary>
        EmptyInputAllowed = 2,
        /// <summary>Gives X-Ways Forensics a hint that the X-Tension is requesting a 
        /// password, so that for example no screenshot of the dialog window is taken for 
        /// the log.</summary>
        PasswordRequested = 10
    }

    /// <summary>
    /// File Systems
    /// </summary>
    public enum VolumeFileSystem : int
    {
        MainMemory = 9,
        CDFS = 8,
        ViaOS = 7,
        XWFS = 6,
        UDF = 5,
        exFAT = 4,
        FAT32 = 3,
        FAT16 = 2,
        FAT12 = 1,
        Unknown = 0,
        NTFS = -1,
        HPFS = -2,
        Ext2 = -3,
        Ext3 = -4,
        ReiserFS = -5,
        Reiser4 = -6,
        Ext4 = -7,
        JFS = -9,
        XFS = -10,
        UFS = -11,
        HFS = -12,
        HFSPlus = -13,
        NTFSBitlocker = -15,
        PartitionedDisks = -16
    }

    /// <summary>
    /// Volume name type.
    /// </summary>
    public enum VolumeNameType : uint
    {
        /// <summary>Type 1</summary>
        Type1 = 1u,
        /// <summary>Type 2</summary>
        Type2 = 2u,
        /// <summary>Type 3</summary>
        Type3 = 3u
    }

    /// <summary>
    /// 
    /// </summary>
    public enum VolumeSnapshotPropertyType : int
    {
        /// <summary>Returns the ID of a special item in the volume snapshot, or -1 if
        /// the requested special item is not present in the volume snapshot. Useful for
        /// example when adding more items to the volume snapshot that need to have a 
        /// parent. pBuffer must point to a byte with a SpecialItemType value.
        /// </summary>
        SpecialItemID = 10,
        /// <summary>Retrieves the type of the primary hash values that the files in the
        /// volume snapshot have. Requires v17.8 SR-17, v17.9 SR-10, v18.0 SR-4 or 
        /// later.</summary>
        HashType1 = 20,
        /// <summary>Retrieves the type of the secondary hash values that the files in
        /// the volume snapshot have. Requires v17.8 SR-17, v17.9 SR-10, v18.0 SR-4 and 
        /// later.</summary>
        HashType2 = 21
    }

    /// <summary>
    /// Where the X-Tension was called from.
    /// </summary>
    [Flags]
    public enum XTensionActionSource : uint
    {
        /// <summary>Called from the main menu, not for any particular volume.</summary>
        MainMenu = 0u,
        /// <summary>Called from volume snapshot refinement.</summary>
        VolumeSnapshotRefinement = 1u,
        /// <summary>Called from logical simultaneous search.</summary>
        LogicalSimultaneousSearch = 2u,
        /// <summary>Called from physical simultaneous search.</summary>
        PhysicalSimultaneousSearch = 3u,
        /// <summary>Called from the directory browser context menu.</summary>
        DirectoryBrowserContextMenu = 4u,
        /// <summary>Called from the search hit context menu.</summary>
        SearchHitContextMenu = 5u
    }

    /// <summary>
    /// A struct to store block boundaries. 
    /// </summary>
    public struct BlockBoundaries
    {
        /// <summary>Block starting offset.</summary>
        public long StartOffset;
        /// <summary>Block ending offset.</summary>
        public long EndOffset;
    }

    /// <summary>
    /// Calling program's version information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CallerInformation
    {
        /// <summary>User interface language of the calling program.</summary>
        public byte Language;
        /// <summary>The service release number of the callilng program.</summary>
        public byte ServiceRelease;
        /// <summary>The version of the calling program.</summary>
        public short Version;
    }

    /// <summary>
    /// Case properties.
    /// </summary>
    public struct CaseProperties
    {
        /// <summary>The case title.</summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CaseTitle;
        /// <summary>The case examiner.</summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CaseExaminer;
        /// <summary>The .xfc case file path.</summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CaseFilePath;
        /// <summary>The case directory path.</summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CaseDirectory;
    }

    /// <summary>
    /// Code Pages. Must specify 0 for unused code page numbers.
    /// </summary>
    /// <remarks>Need enum for the code pages.</remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct CodePages
    {
        /// <summary>Size of the packed record.</summary>
        public int packedRecordSize;
        /// <summary>Code page 1.</summary>
        public ushort codePage1;
        /// <summary>Code page 2.</summary>
        public ushort codePage2;
        /// <summary>Code page 3.</summary>
        public ushort codePage3;
        /// <summary>Code page 4.</summary>
        public ushort codePage4;
        /// <summary>Code page 5.</summary>
        public ushort codePage5;
    };

    /// <summary>
    /// Event Information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct EventInformation
    {
        /// <summary>Size of the structure.</summary>
        public uint Size;
        /// <summary>Pointer to the related evidence.</summary>
        public IntPtr Evidence;
        /// <summary>Event type.</summary>
        public uint Type; // need enum for this
        public uint Options; // need enum for this
        /// <summary>Event timestamp.</summary>
        public System.Runtime.InteropServices.ComTypes.FILETIME TimeStamp;
        /// <summary>Item ID of item related to the event, otherwise -1.</summary>
        public int ItemID;
        /// <summary>Offset where the timestamp was found in the volume or (if nItemID 
        /// unequal to -1) within the object in the volume snapshot. -1 if unknown.
        /// </summary>
        public long Offset;
        /// <summary>Optional null-terminated textual description of the event, 
        /// preferably in 7-bit ASCII or else in UTF-8. Will be truncated internally 
        /// after 255 bytes. NULL if not provided.</summary>
        public string Description;
    };

    public struct EvidenceObjectProperties
    {
        public long objectNumber;
        public long objectID;
        public long parentObjectID;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string title;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string extendedTitle;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string abbreviatedTitle;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string internalName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string description;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string examinerComments;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string internallyUsedDirectory;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string outputDirectory;
        public long SizeInBytes;
        public long VolumeSnapshotFileCount;
        public EvidenceProperties Flags;
        public VolumeFileSystem FileSystemIdentifier;
        public HashType HashType;
        public byte[] HashValue;
        public DateTime CreationTime;
        public DateTime ModificationTime;
        public HashType HashType2;
        public byte[] HashValue2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct ItemInformation
    {
        public long originalItemID;
        public long attributes;
        public ItemInformationOptions options;
        public ItemDeletionStatus deletionStatus;
        public ItemClassifiction classification;
        public long linkCount;
        public long colorAnalysis;
        public long fileCount;
        public long embeddedOffset;
        public DateTime creationTime;
        public DateTime modificationTime;
        public DateTime lastAccessTime;
        public DateTime entryModificationTime;
        public DateTime deletionTime;
        public DateTime internalCreationTime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct ItemOffsets
    {
        public long FileSystemDataStructureOffset;
        public long CarvedFileVolumeOffset;
        public long DataStartSector;
    }

    public struct ItemType
    {
        public ItemTypeCategory Type;
        public string Description;
    }

    /// <summary>
    /// Raster Image Information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct RasterImageInformation
    {
        /// <summary>Size of the structure.</summary>
        public int packedStructureSize;
        /// <summary>The item Id.</summary>
        public int itemId;
        /// <summary>Pointer to the item.</summary>
        public IntPtr item;
        /// <summary></summary>
        public RasterImageOptions options;
        /// <summary>Width of the image.</summary>
        public uint imageWidth;
        /// <summary>Height of the image.</summary>
        public uint imageHeight;
        /// <summary>Size of the image in bytes.</summary>
        public uint imageSizeInBytes;
    };

    /// <summary>
    /// Search hit information.
    /// </summary>
    public struct SearchHitInformation
    {
        /// <summary>Size of the packed record.</summary>
        public int iSize;
        /// <summary>The ID of the current item.</summary>
        public int nItemID;
        /// <summary>Relative offset of the search hit in its respective file, if any, 
        /// otherwise -1.</summary>
        public long nRelOfs;
        /// <summary>Absolute offset of the search hit with respect to the volume, if 
        /// available, otherwise -1. This offset may be changed if it helps improve the 
        /// quality of the search hit.</summary>
        public long nAbsOfs;
        /// <summary>Pointer to the search hit in memory. Provided only if 
        /// XT_ProcessSearchHit is called during a search, not when later applied to an 
        /// existing seach hit, and only for search hits of the simultaneous search, not 
        /// index searches.</summary>
        public IntPtr lpOptionalHitPtr;
        /// <summary>The ID of the search term matched; can be changed.</summary>
        public ushort lpSearchTermID;
        /// <summary>Size of the search hit in bytes. Can be changed if it helps improve
        /// the quality of the search hit.</summary>
        public ushort nLength;
        /// <summary>The code page of the search hit.</summary>
        public ushort nCodePage;
        /// <summary>Search hit information flags.</summary>
        public SearchHitInformationProperties nFlags;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct SearchInformation
    {
        /// <summary>Size of the packed record.</summary>
        public int packedRecordSize;
        /// <summary>Currently must be 0; function is always applied at the active
        /// volume.</summary>
        public IntPtr volume;
        /// <summary>The search terms delimited by line breaks; added to the case if not
        /// currently existing.</summary>
        public string searchTerms;
        /// <summary>Search options. Can only be combined as known from the user 
        /// interface. For example, GREP and whole words are mutually exclusive. 
        /// Otherwise, the results are undefined.</summary>
        public SearchInformationOptions searchOptions;
        /// <summary>Search window length. 0 for standard length.</summary>
        public uint searchWindowLength;
    };

    /// <summary>
    /// Search preparation information.
    /// </summary>
    public struct SearchPreparationInformation
    {
        /// <summary>Size of the packed record.</summary>
        public int iSize;
        /// <summary>The currently entered search terms; null-terminated and delimited 
        /// by line breaks. Can be manipulated, replaced or completed.</summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpSearchTerms;
        /// <summary>Size in Unicode charactes of the buffer pointed to by 
        /// lpSearchTerms.</summary>
        public int nBufLen;
        /// <summary>Search options.</summary>
        public SearchOptions nFlags;
    };

    /// <summary>
    /// A struct to store sector information.
    /// </summary>
    public struct SectorInformation
    {
        /// <summary>True if the sector is being used, otherwise False.</summary>
        public bool IsAllocated;
        /// <summary>A textual description of what the sector is used for. Can be the
        /// name and path of a file or something like "FAT 1".</summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Description;
        /// <summary>The ID of the item in the volume snapshot that the sector is
        /// allocated to, if any, otherwise -1.</summary>
        public long OwnerItemID;
    }

    /// <summary>
    /// A struct to store volume information.
    /// </summary>
    public struct VolumeInformation
    {
        /// <summary>The file system used.</summary>
        public VolumeFileSystem FileSystem;
        /// <summary>The number of bytes per sector.</summary>
        public uint BytesPerSector;
        /// <summary>The number of sectors per cluster.</summary>
        public uint SectorsPerCluster;
        /// <summary>The cluster count.</summary>
        public long ClusterCount;
        /// <summary>the first cluster's sector number.</summary>
        public long FirstClusterSectorNumber;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct VolumeSnapshotProperties
    {
        public long rootDirectory;
        public long pathUnknownDirectory;
        public long carvedFilesDirectory;
        public long freeSpaceFile;
        public long systemVolumeInformationDirectory;
        public long windowsEDBFile;
        public HashType hashType1;
        public HashType hashType2;
    }
}