using System;

namespace XTensions.Testing
{
    public static class ItemInteraction
    {
        private static void ProcessItem(IntPtr volume, int itemID)
        {
            HelperMethods.XWF_OutputHeader("XWF_OpenItem(), XWF_GetItemName(), "
                + "XWF_GetSize(), XWF_GetItemSize(), XWF_GetItemInformation(), " 
                + "XWF_GetItemType(), and XWF_GetItemOfs() Test:"
                , OutputMessageLevel.Level2);

            HelperMethods.XWF_OutputMessage("Item ID: " + itemID
                , OutputMessageLevel.Level3);

            // Get item's name and output it.
            string itemName = HelperMethods.XWF_GetItemName(itemID);
            HelperMethods.XWF_OutputMessage("Item Name: "
                + itemName, OutputMessageLevel.Level4);

            // Logically open the item.
            IntPtr LogicalItem = HelperMethods.XWF_OpenItem(volume, itemID
                , ItemOpenModes.LogicalContents);

            // Logically open the item with slack.
            IntPtr LogicalSlackItem = HelperMethods.XWF_OpenItem(volume
                , itemID, ItemOpenModes.LogicalContentsAndSlack);

            if (LogicalItem == IntPtr.Zero)
            {
                HelperMethods.XWF_OutputMessage("Item ID " + itemID
                    + " couldn't be opened.", OutputMessageLevel.Level3);
                return;
            }

            // Output the open results.
            HelperMethods.XWF_OutputMessage("Logical Handle: " + LogicalItem
                , OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Logical (w/ Slack) Handle: "
                + LogicalSlackItem, OutputMessageLevel.Level4);

            // Compare results of XWF_GetSize() and XWF_GetItemSize().
            Int64 itemSize = HelperMethods.XWF_GetSize(LogicalItem
                , ItemSizeType.PhysicalSize);
            Int64 itemSize2 = HelperMethods.XWF_GetItemSize(itemID);

            if (itemSize == itemSize2)
            {
                HelperMethods.XWF_OutputMessage("Logical Size: " + itemSize
                    , OutputMessageLevel.Level4);
            }
            else
            {
                HelperMethods.XWF_OutputMessage(String.Format("XWF_GetSize() results " 
                    + "({1}) differs from XWF_GetItemSize() results ({2}).", itemSize
                    , itemSize2), OutputMessageLevel.Level4);
            }

            // Get the size of the logical item with slack.
            itemSize = HelperMethods.XWF_GetSize(LogicalSlackItem
                , ItemSizeType.PhysicalSize);
            HelperMethods.XWF_OutputMessage("Logical (w/ Slack) Size: "
                + itemSize, OutputMessageLevel.Level4);

            // Close the open items.
            if (HelperMethods.XWF_Close(LogicalItem))
            {
                HelperMethods.XWF_OutputMessage("Logical item closed."
                    , OutputMessageLevel.Level4);
            }
            else
            {
                HelperMethods.XWF_OutputMessage("Logical item couldn't be closed."
                    , OutputMessageLevel.Level4);
            }
            if (HelperMethods.XWF_Close(LogicalSlackItem))
            {
                HelperMethods.XWF_OutputMessage("Logical item with slack closed."
                    , OutputMessageLevel.Level4);
            }
            else
            {
                HelperMethods.XWF_OutputMessage(
                    "Logical item with slack couldn't be closed."
                    , OutputMessageLevel.Level4);
            }

            // Not sure we need this.
            //HelperMethods.XWF_SelectVolumeSnapshot(volume);

            // Get the item's information.
            ItemInformation itemInfo = HelperMethods.XWF_GetItemInformation(itemID);

            HelperMethods.XWF_OutputMessage("Original ID: "
                + itemInfo.OriginalItemID, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Attributes: "
                + itemInfo.Attributes, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Flags: "
                + itemInfo.Flags, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Deletion Status: "
                + itemInfo.Deletion, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Hard Link Count: "
                + itemInfo.LinkCount, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Color Analysis: "
                + itemInfo.ColorAnalysis, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Recursive Child File Count: "
                + itemInfo.FileCount, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Embedded Offset: "
                + itemInfo.EmbeddedOffset, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Creation Time: "
                + itemInfo.CreationTime, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Modification Time: "
                + itemInfo.ModificationTime, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Last Access Time: "
                + itemInfo.LastAccessTime, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Entry Modification Time: "
                + itemInfo.EntryModificationTime, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Deletion Time: "
                + itemInfo.DeletionTime, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Internal Creation Time: "
                + itemInfo.InternalCreationTime, OutputMessageLevel.Level4);

            ItemType CurrentItemType = HelperMethods.XWF_GetItemType(itemID);
            HelperMethods.XWF_OutputMessage("Item Type: "
                + CurrentItemType.Type, OutputMessageLevel.Level4);
            HelperMethods.XWF_OutputMessage("Item Description: "
                + CurrentItemType.Description, OutputMessageLevel.Level4);

            ItemOffsets Offsets = HelperMethods.XWF_GetItemOfs(itemID);
            if (Offsets.FileSystemDataStructureOffset != -1)
            {
                HelperMethods.XWF_OutputMessage("File System Data Structure Offset: "
                    + Offsets.FileSystemDataStructureOffset
                    , OutputMessageLevel.Level4);
            }

            if (Offsets.CarvedFileVolumeOffset != -1)
            {
                HelperMethods.XWF_OutputMessage("Carved File Volume Offset: "
                    + Offsets.CarvedFileVolumeOffset
                    , OutputMessageLevel.Level4);
            }

            HelperMethods.XWF_OutputMessage("Data Sector Start: "
                + Offsets.DataStartSector
                , OutputMessageLevel.Level4);

            Int32 nParentItemID = HelperMethods.XWF_GetItemParent(itemID);
            HelperMethods.XWF_OutputMessage("Parent ID: "
                + nParentItemID
                , OutputMessageLevel.Level4);

            string[] Associations = HelperMethods.XWF_GetReportTableAssocs(itemID);

            if (Associations.Length > 0)
            {
                int ReportNumber = 1;
                foreach (string sReportName in Associations)
                {
                    HelperMethods.XWF_OutputMessage(String.Format("Report {0} Name: {1}"
                        , ReportNumber, sReportName), OutputMessageLevel.Level4);
                    ReportNumber += 1;
                }
            }
            else
            {
                HelperMethods.XWF_OutputMessage("No report table associations."
                    , OutputMessageLevel.Level4);
            }

            // Add and get item comment.
            string itemCommentToAdd = "This is a comment.";
            HelperMethods.XWF_AddComment(itemID, itemCommentToAdd
                , AddCommentMode.Replace);

            string itemComment = HelperMethods.XWF_GetComment(itemID);
            if (itemComment != null)
            {
                HelperMethods.XWF_OutputMessage("Comment: " + itemComment
                    , OutputMessageLevel.Level4);
            }

            string itemMetadata = HelperMethods.XWF_GetExtractedMetadata(itemID);
            if (itemMetadata != null)
            {
                HelperMethods.XWF_OutputMessage("Metadata: " + itemMetadata
                    , OutputMessageLevel.Level4);
            }

            HelperMethods.XWF_OutputEmptyLine();
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
            HelperMethods.XWF_OutputHeader("ITEM INTERACTION TESTING MODULE"
                , OutputMessageLevel.Level1);

            ProcessItem(hVolume, 7554);
            ProcessItem(hVolume, 2680);
        }
    }
}
