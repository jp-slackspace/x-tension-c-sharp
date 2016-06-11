using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace XTensions.Testing
{
    public static class Testing
    {
        /// <summary>
        /// Run a test for the current category methods.
        /// </summary>
        /// <param name="hVolume">The current volume pointer.</param>
        /// <param name="nOpType">The current operation type.</param>
        /// <returns></returns>
        public static void RunTest(IntPtr hVolume, XTensionActionSource nOpType)
        {
            // Get the action type description.
            string ActionDescription;
            switch (nOpType)
            {
                case XTensionActionSource.MainMenu:
                    ActionDescription = "Called from the main menu for no particular "
                        + "volume.";
                    break;
                case XTensionActionSource.VolumeSnapshotRefinement:
                    ActionDescription = "Called from volume snapshot refinement.";
                    break;
                case XTensionActionSource.LogicalSimultaneousSearch:
                    ActionDescription = "Called from a logical simultaneous search.";
                    break;
                case XTensionActionSource.PhysicalSimultaneousSearch:
                    ActionDescription = "Called from a physical simultaneous search.";
                    break;
                case XTensionActionSource.DirectoryBrowserContextMenu:
                    ActionDescription = "Called from the directory browser context "
                        + "menu.";
                    break;
                case XTensionActionSource.SearchHitContextMenu:
                    ActionDescription = "Called from the search hit context menu.";
                    break;
                default:
                    ActionDescription = "ALERT: Called from an unknown area!";
                    break;
            }

            HelperMethods.OutputMessage("Volume Handle = " + hVolume);
            HelperMethods.OutputMessage("Action Type = " + nOpType);
            HelperMethods.OutputMessage("Action Description = " 
                + ActionDescription);

            // XWF_OpenItem test.
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_OpenItem Test:");

            // Item 1 test.
            long ItemID1 = 0;
            IntPtr hItem1 = HelperMethods.OpenItem(hVolume, ItemID1
                , ItemOpenModes.LogicalContents);
            if (hItem1 != IntPtr.Zero)
            {
                HelperMethods.OutputMessage("Item ID = " + ItemID1);
                HelperMethods.OutputMessage("    Item Handle = " + hItem1);

                Int64 itemPhysicalSize = HelperMethods.GetSize(hItem1
                    , ItemSizeType.PhysicalSize);
                HelperMethods.OutputMessage("    Item Physical Size: "
                    + itemPhysicalSize);

                Int64 itemLogicalSize = HelperMethods.GetSize(hItem1
                    , ItemSizeType.LogicalSize);
                HelperMethods.OutputMessage("    Item Logical Size: "
                    + itemLogicalSize);

                Int64 itemValidDateLength = HelperMethods.GetSize(hItem1
                    , ItemSizeType.ValidDataLength);
                HelperMethods.OutputMessage("    Item Valid Data Length: "
                    + itemValidDateLength);

                //HelperMethods.XWF_SelectVolumeSnapshot(hVolume);
                //string itemName = HelperMethods.XWF_GetItemName(ItemID1);
                //HelperMethods.OutputMessage("    Item Name = " + itemName);
            }
            else
            {
                HelperMethods.OutputMessage("Item ID " + ItemID1 + " couldn't be opened." );
            }

            // Item 2 test.
            Int64 itemID2 = 3;
            IntPtr hItem2 = HelperMethods.OpenItem(hVolume, itemID2
                , ItemOpenModes.LogicalContents);
            if (hItem1 != IntPtr.Zero)
            {
                HelperMethods.OutputMessage("Item ID = " + itemID2);
                HelperMethods.OutputMessage("    Item Handle = " + hItem2);

                Int64 itemPhysicalSize = HelperMethods.GetSize(hItem2
                    , ItemSizeType.PhysicalSize);
                HelperMethods.OutputMessage("    Item Physical Size: "
                    + itemPhysicalSize);

                Int64 itemLogicalSize = HelperMethods.GetSize(hItem2
                    , ItemSizeType.LogicalSize);
                HelperMethods.OutputMessage("    Item Logical Size: "
                    + itemLogicalSize);

                Int64 itemValidDateLength = HelperMethods.GetSize(hItem2
                    , ItemSizeType.ValidDataLength);
                HelperMethods.OutputMessage("    Item Valid Data Length: "
                    + itemValidDateLength);

                HelperMethods.SelectVolumeSnapshot(hVolume);
                //string itemName = HelperMethods.XWF_GetItemName(itemID2);
                //HelperMethods.OutputMessage("    Item Name = " + itemName);
            }
            else
            {
                HelperMethods.OutputMessage("Item ID " + itemID2 + " couldn't be opened.");
            }

            // XWF_Read test.
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_Read Test:");

            byte[] itemContent = HelperMethods.Read(hItem2, 0x4E, 0x27);

            if (itemContent != null)
            {
                HelperMethods.OutputMessage("Item 2 Content from Offset 0x4E: "
                    + System.Text.Encoding.Default.GetString(itemContent));
            } else
            {
                HelperMethods.OutputMessage("Item read failed.");
            }

            // XWF_Close test.
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_Close Test:");

            if (HelperMethods.CloseItem(hItem1))
            {
                HelperMethods.OutputMessage("Item 1 closed");
            } else
            {
                HelperMethods.OutputMessage("Item 1 doesn't exist to close.");
            }

            if (HelperMethods.CloseItem(hItem2))
            {
                HelperMethods.OutputMessage("Item 2 closed");
            } else
            {
                HelperMethods.OutputMessage("Item 2 doesn't exist to close.");
            }

            // XWF_GetFirstEvObj, XWF_GetNextEvObj, and GetCaseEvidence, XWF_OpenEvObj,
            // XWF_CloseEvObj, XWF_GetEvObjProp test.
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_GetFirstEvObj, XWF_GetNextEvObj, " 
                + "GetCaseEvidence, XWF_OpenEvObj, XWF_CloseEvObj, XWF_GetEvObjProp " 
                + "Test:");

            int nEvidenceNumber = 0;

            foreach (IntPtr currentEvidence in HelperMethods.GetCaseEvidence())
            {
                nEvidenceNumber += 1;
                HelperMethods.OutputMessage(String.Format(
                    "Evidence {0} Object Handle: {1}",
                    nEvidenceNumber, currentEvidence));

                IntPtr hCurrentEvidence = HelperMethods.OpenEvidenceObject(
                    currentEvidence, EvidenceOpenOptions.None);
                HelperMethods.OutputMessage(String.Format(
                    "    Opened Evidence {0} Object Handle: {1}",
                    nEvidenceNumber, hCurrentEvidence));

                HelperMethods.CloseEvidenceObject(currentEvidence);
                HelperMethods.OutputMessage(String.Format(
                    "    Closed Open Evidence {0} Object Handle",
                    nEvidenceNumber));

                EvidenceObjectProperties evidenceObjectProps 
                    = HelperMethods.GetEvidenceObjectProperties(currentEvidence);

                HelperMethods.OutputMessage("    Object Number: "
                    + evidenceObjectProps.objectNumber);
                HelperMethods.OutputMessage("    Object ID: "
                    + evidenceObjectProps.objectID);
                HelperMethods.OutputMessage("    Parent Object ID: "
                    + evidenceObjectProps.parentObjectID);
                HelperMethods.OutputMessage("    Title: "
                    + evidenceObjectProps.title);
                HelperMethods.OutputMessage("    Extended Title: "
                    + evidenceObjectProps.extendedTitle);
                HelperMethods.OutputMessage("    Abbreviated Title: "
                    + evidenceObjectProps.abbreviatedTitle);
                HelperMethods.OutputMessage("    Internal Name: "
                    + evidenceObjectProps.internalName);
                // Commenting out since it can be fairly wordy
                //HelperMethods.OutputMessage("    Description: "
                //    + evidenceObjectProps.description);
                HelperMethods.OutputMessage("    Examiner Comments: "
                    + evidenceObjectProps.examinerComments);
                HelperMethods.OutputMessage("    Internally Used Directory: "
                    + evidenceObjectProps.internallyUsedDirectory);
                HelperMethods.OutputMessage("    Output Directory: "
                    + evidenceObjectProps.outputDirectory);
                HelperMethods.OutputMessage("    Size in Bytes: "
                    + evidenceObjectProps.SizeInBytes);
                HelperMethods.OutputMessage("    Volume Snapshot File Count: "
                    + evidenceObjectProps.VolumeSnapshotFileCount);
                HelperMethods.OutputMessage("    Flags: "
                    + evidenceObjectProps.Flags);
                HelperMethods.OutputMessage("    File System Identifier: "
                    + evidenceObjectProps.FileSystemIdentifier);
                HelperMethods.OutputMessage("    Creation Time: "
                    + evidenceObjectProps.CreationTime);
                HelperMethods.OutputMessage("    Modification Time: "
                    + evidenceObjectProps.ModificationTime);
                HelperMethods.OutputMessage("    Hash 1 Type: "
                    + evidenceObjectProps.HashType);
                if (evidenceObjectProps.HashType != HashType.Undefined)
                {
                    HelperMethods.OutputMessage("    Hash 1 Value: "
                        + HelperMethods.Hexlify(evidenceObjectProps.HashValue).ToUpper());
                }
                HelperMethods.OutputMessage("    Hash 2 Type: "
                    + evidenceObjectProps.HashType2);
                if (evidenceObjectProps.HashType2 != HashType.Undefined)
                {
                    HelperMethods.OutputMessage("    Hash 2 Value: "
                        + HelperMethods.Hexlify(evidenceObjectProps.HashValue).ToUpper());
                }
            }

            // XWF_CreateEvObj test.
            // Commenting this method out for now. The following exception occurs when
            // using this function, though it did successfully add the evidence object:
            //
            // Lost internal information about parent disk of partition!
            // Lost internal information about parent disk of partition!
            // Lost internal information about parent disk of partition!
            // An exception of type 216 (page protection fault) occurred at offset
            // 1:001F52E2.
            /*
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_CreateEvObj Test:");

            string evidencePath = Path.GetFullPath(Path.Combine(caseProps.caseDirectory
                , @"..\Media\test1.img"));
            HelperMethods.OutputMessage("Evidence Object to be created: "
                + evidencePath);

            HelperMethods.XWF_CreateEvObj(
                XWFEvidenceObjTypeID.DiskImage, evidencePath);
            */

            // XWF_GetReportTableInfo test.
            // Exception from running this. Needs more investigation.
            /*
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_GetReportTableInfo Test:");

            string sReportName = HelperMethods.XWF_GetReportTableInfo(0
                , XWFGetReportTableInfoFlags.UserCreated 
                & XWFGetReportTableInfoFlags.ApplicationCreated
                & XWFGetReportTableInfoFlags.RepresentsSearchTerm);

            HelperMethods.OutputMessage("Report Table Name:" + sReportName);
            */

            // XWF_SelectVolumeSnapshot test.
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_SelectVolumeSnapshot Test:");

            HelperMethods.SelectVolumeSnapshot(hVolume);
            HelperMethods.OutputMessage("Volume Snapshots for volume " + hVolume 
                + " will be processed with subsquent function calls.");

            // XWF_GetVSProp test.
            HelperMethods.OutputMessage("");
            HelperMethods.OutputMessage("XWF_GetVSProp Test:");

            VolumeSnapshotProperties vsProps 
                = HelperMethods.GetVolumeSnapshotProperties();

            /*
            IntPtr hEvidence1 = HelperMethods.XWF_GetFirstEvObj();
            HelperMethods.OutputMessage("Evidence Object 1: " + hEvidence1);
            IntPtr hEvidence2 = HelperMethods.XWF_GetNextEvObj(hEvidence1);
            HelperMethods.OutputMessage("Evidence Object 2: " + hEvidence2);
            IntPtr hEvidence3 = HelperMethods.XWF_GetNextEvObj(hEvidence2);
            HelperMethods.OutputMessage("Evidence Object 3: " + hEvidence3);
            IntPtr hEvidence4 = HelperMethods.XWF_GetNextEvObj(hEvidence3);
            HelperMethods.OutputMessage("Evidence Object 4: " + hEvidence4);
            IntPtr hEvidence5 = HelperMethods.XWF_GetNextEvObj(hEvidence4);
            HelperMethods.OutputMessage("Evidence Object 5: " + hEvidence5);
            IntPtr hEvidence6 = HelperMethods.XWF_GetNextEvObj(hEvidence5);
            HelperMethods.OutputMessage("Evidence Object 6: " + hEvidence6);
            */

            /*
            // XT_Prepare may get called with a zero handle, which means there is no 
            // volume to prepare for. Before calling any function on hVolume passed from 
            // XT_Prepare, you have to check that its not zero: hVolume != IntPtr.Zero
            if (hVolume != IntPtr.Zero)
            {
                // Get the case properties.
                var props = HelperMethods.XWF_GetCaseProps();

                HelperMethods.OutputMessage(string.Format(
                    "Case Title: {0}, Case Examiner: {1}, File Path: {2}"
                  + ", Case Directory: {3}"
                  , props.caseTitle, props.caseExaminer, props.caseFilePath
                  , props.caseDirectory));

                // Get the volume information.
                var volumeInfo = HelperMethods.XWF_GetVolumeInformation(hVolume);

                HelperMethods.OutputMessage(string.Format(
                    "File System: {0}, Bytes per Sector: {1}, Sectors per Cluster: {2}"
                  + ", Cluster Count: {3}, First Cluster Sector Number: {4}"
                  , volumeInfo.FileSystem, volumeInfo.BytesPerSector
                  , volumeInfo.SectorsPerCluster, volumeInfo.ClusterCount
                  , volumeInfo.FirstClusterSectorNo));

                // Get the first cluster sector.
                string sectorDesc;
                Int32 sectorItemId;

                bool sectorIsUsed = HelperMethods.XWF_GetSectorContents(hVolume
                    , volumeInfo.FirstClusterSectorNo, out sectorDesc
                    , out sectorItemId);

                HelperMethods.OutputMessage(string.Format(
                    "First Cluster Sector Description: {0}, Sector Item Id: {1}, "
                  + "Sector Is Used: {2}"
                  , sectorDesc, sectorItemId, sectorIsUsed));

                // Get the volume name.
                string volumeName = HelperMethods.XWF_GetVolumeName(hVolume
                    , XWFVolumeNameType.Type1);

                HelperMethods.OutputMessage("Volume Name: " + volumeName);

                // Enumerate evidence objects
                HelperMethods.OutputMessage("ENUMERATE EVIDENCE OBJECTS");
                var currentObject = HelperMethods.XWF_GetFirstEvObj();
                var objectProps = HelperMethods.XWF_GetEvObjProp(currentObject);
                HelperMethods.OutputMessage(string.Format(
                    "Object Number: {0}, Object ID: {1}, Parent Object ID: {2}"
                  + ", Object Title: {3}, Extended Object Title: {4}"
                  + ", Abber. Object Title: {5}, Internal Name: {6}, Description: {7}"
                  + ", Size in Bytes: {8}, Creation Time: {9}, Modification Time: {10}"
                  , objectProps.objectNumber, objectProps.objectID
                  , objectProps.parentObjectID, objectProps.title
                  , objectProps.extendedTitle, objectProps.abbreviatedTitle
                  , objectProps.internalName, objectProps.description
                  , objectProps.SizeInBytes, objectProps.CreationTime
                  , objectProps.ModificationTime));

                currentObject = HelperMethods.XWF_GetNextEvObj(currentObject);
                objectProps = HelperMethods.XWF_GetEvObjProp(currentObject);
                HelperMethods.OutputMessage(string.Format(
                    "Object Number: {0}, Object ID: {1}, Parent Object ID: {2}"
                  + ", Object Title: {3}, Extended Object Title: {4}"
                  + ", Abber. Object Title: {5}, Internal Name: {6}, Description: {7}"
                  + ", Size in Bytes: {8}, Creation Time: {9}, Modification Time: {10}"
                  , objectProps.objectNumber, objectProps.objectID
                  , objectProps.parentObjectID, objectProps.title
                  , objectProps.extendedTitle, objectProps.abbreviatedTitle
                  , objectProps.internalName, objectProps.description
                  , objectProps.SizeInBytes, objectProps.CreationTime
                  , objectProps.ModificationTime));

            }
            */
        }
    }
}
