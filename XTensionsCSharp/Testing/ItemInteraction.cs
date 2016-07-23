using System;

namespace XTensions.Testing
{
    public static class ItemInteraction
    {
        private static void ProcessItem(IntPtr volume, int itemID)
        {
            HelperMethods.OutputMessage("OpenItem(), GetItemName(), GetSize(), " +
                "GetItemSize(), GetItemInformation(), GetItemType(), and " +
                "GetItemOffsets() Test:", OutputMessageOptions.Level2 | 
                OutputMessageOptions.Header);

            HelperMethods.OutputMessage("Item ID: " + itemID
                , OutputMessageOptions.Level3);

            // Get item's name and output it.
            string itemName = HelperMethods.GetItemName(itemID);
            HelperMethods.OutputMessage("Item Name: "
                + itemName, OutputMessageOptions.Level4);

            // Logically open the item.
            IntPtr LogicalItem = HelperMethods.OpenItem(volume, itemID
                , ItemOpenModes.LogicalContents);

            // Logically open the item with slack.
            IntPtr LogicalSlackItem = HelperMethods.OpenItem(volume
                , itemID, ItemOpenModes.LogicalContentsAndSlack);

            if (LogicalItem == IntPtr.Zero)
            {
                HelperMethods.OutputMessage("Item ID " + itemID
                    + " couldn't be opened.", OutputMessageOptions.Level3);
                return;
            }

            // Output the open results.
            HelperMethods.OutputMessage("Logical Handle: " + LogicalItem
                , OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Logical (w/ Slack) Handle: "
                + LogicalSlackItem, OutputMessageOptions.Level4);

            // Compare results of XWF_GetSize() and XWF_GetItemSize().
            Int64 itemSize = HelperMethods.GetSize(LogicalItem
                , ItemSizeType.PhysicalSize);
            Int64 itemSize2 = HelperMethods.GetItemSize(itemID);

            if (itemSize == itemSize2)
            {
                HelperMethods.OutputMessage("Logical Size: " + itemSize
                    , OutputMessageOptions.Level4);
            }
            else
            {
                HelperMethods.OutputMessage(String.Format("XWF_GetSize() results " 
                    + "({1}) differs from XWF_GetItemSize() results ({2}).", itemSize
                    , itemSize2), OutputMessageOptions.Level4);
            }

            // Get the size of the logical item with slack.
            itemSize = HelperMethods.GetSize(LogicalSlackItem
                , ItemSizeType.PhysicalSize);
            HelperMethods.OutputMessage("Logical (w/ Slack) Size: "
                + itemSize, OutputMessageOptions.Level4);

            // Close the open items.
            HelperMethods.CloseItem(LogicalItem);
            HelperMethods.OutputMessage("Logical item closed."
                , OutputMessageOptions.Level4);

            HelperMethods.CloseItem(LogicalSlackItem);
            HelperMethods.OutputMessage("Logical item with slack closed."
                , OutputMessageOptions.Level4);

            // Not sure we need this.
            //HelperMethods.XWF_SelectVolumeSnapshot(volume);

            // Get the item's information.
            ItemInformation itemInfo = HelperMethods.GetItemInformation(itemID);

            HelperMethods.OutputMessage("Original ID: "
                + itemInfo.originalItemID, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Attributes: "
                + itemInfo.attributes, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Flags: "
                + itemInfo.options, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Deletion Status: "
                + itemInfo.deletionStatus, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Hard Link Count: "
                + itemInfo.linkCount, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Color Analysis: "
                + itemInfo.colorAnalysis, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Recursive Child File Count: "
                + itemInfo.fileCount, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Embedded Offset: "
                + itemInfo.embeddedOffset, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Creation Time: "
                + itemInfo.creationTime, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Modification Time: "
                + itemInfo.modificationTime, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Last Access Time: "
                + itemInfo.lastAccessTime, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Entry Modification Time: "
                + itemInfo.entryModificationTime, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Deletion Time: "
                + itemInfo.deletionTime, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Internal Creation Time: "
                + itemInfo.internalCreationTime, OutputMessageOptions.Level4);

            ItemType CurrentItemType = HelperMethods.GetItemType(itemID);
            HelperMethods.OutputMessage("Item Type: "
                + CurrentItemType.Type, OutputMessageOptions.Level4);
            HelperMethods.OutputMessage("Item Description: "
                + CurrentItemType.Description, OutputMessageOptions.Level4);

            ItemOffsets Offsets = HelperMethods.GetItemOffsets(itemID);
            if (Offsets.FileSystemDataStructureOffset != -1)
            {
                HelperMethods.OutputMessage("File System Data Structure Offset: "
                    + Offsets.FileSystemDataStructureOffset
                    , OutputMessageOptions.Level4);
            }

            if (Offsets.CarvedFileVolumeOffset != -1)
            {
                HelperMethods.OutputMessage("Carved File Volume Offset: "
                    + Offsets.CarvedFileVolumeOffset
                    , OutputMessageOptions.Level4);
            }

            HelperMethods.OutputMessage("Data Sector Start: "
                + Offsets.DataStartSector
                , OutputMessageOptions.Level4);

            Int32 nParentItemID = HelperMethods.GetItemParent(itemID);
            HelperMethods.OutputMessage("Parent ID: "
                + nParentItemID
                , OutputMessageOptions.Level4);

            string[] Associations = HelperMethods.GetReportTableAssociations(itemID);

            if (Associations.Length > 0)
            {
                int ReportNumber = 1;
                foreach (string sReportName in Associations)
                {
                    HelperMethods.OutputMessage(String.Format("Report {0} Name: {1}"
                        , ReportNumber, sReportName), OutputMessageOptions.Level4);
                    ReportNumber += 1;
                }
            }
            else
            {
                HelperMethods.OutputMessage("No report table associations."
                    , OutputMessageOptions.Level4);
            }

            // Add and get item comment.
            string itemCommentToAdd = "This is a comment.";
            HelperMethods.AddComment(itemID, itemCommentToAdd
                , AddCommentMode.Replace);

            string itemComment = HelperMethods.GetComment(itemID);
            if (itemComment != null)
            {
                HelperMethods.OutputMessage("Comment: " + itemComment
                    , OutputMessageOptions.Level4);
            }

            string itemMetadata = HelperMethods.GetExtractedMetadata(itemID);
            if (itemMetadata != null)
            {
                HelperMethods.OutputMessage("Metadata: " + itemMetadata
                    , OutputMessageOptions.Level4);
            }

            HelperMethods.OutputMessage("");
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
            HelperMethods.OutputMessage("ITEM INTERACTION TESTING MODULE", 
                OutputMessageOptions.Level1 | OutputMessageOptions.Header);

            ProcessItem(hVolume, 7554);
            ProcessItem(hVolume, 2680);
        }
    }
}
