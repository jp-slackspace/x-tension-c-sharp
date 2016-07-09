using System;

namespace XTensions.Testing
{
    public static class ItemInteraction
    {
        private static void ProcessItem(IntPtr volume, int itemID)
        {
            HelperMethods.OutputHeader("XWF_OpenItem(), XWF_GetItemName(), "
                + "XWF_GetSize(), XWF_GetItemSize(), XWF_GetItemInformation(), " 
                + "XWF_GetItemType(), and XWF_GetItemOfs() Test:"
                , OutputMessageLevel.Level2);

            HelperMethods.OutputMessage("Item ID: " + itemID
                , OutputMessageLevel.Level3);

            // Get item's name and output it.
            string itemName = HelperMethods.GetItemName(itemID);
            HelperMethods.OutputMessage("Item Name: "
                + itemName, OutputMessageLevel.Level4);

            // Logically open the item.
            IntPtr LogicalItem = HelperMethods.OpenItem(volume, itemID
                , ItemOpenModes.LogicalContents);

            // Logically open the item with slack.
            IntPtr LogicalSlackItem = HelperMethods.OpenItem(volume
                , itemID, ItemOpenModes.LogicalContentsAndSlack);

            if (LogicalItem == IntPtr.Zero)
            {
                HelperMethods.OutputMessage("Item ID " + itemID
                    + " couldn't be opened.", OutputMessageLevel.Level3);
                return;
            }

            // Output the open results.
            HelperMethods.OutputMessage("Logical Handle: " + LogicalItem
                , OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Logical (w/ Slack) Handle: "
                + LogicalSlackItem, OutputMessageLevel.Level4);

            // Compare results of XWF_GetSize() and XWF_GetItemSize().
            Int64 itemSize = HelperMethods.GetSize(LogicalItem
                , ItemSizeType.PhysicalSize);
            Int64 itemSize2 = HelperMethods.GetItemSize(itemID);

            if (itemSize == itemSize2)
            {
                HelperMethods.OutputMessage("Logical Size: " + itemSize
                    , OutputMessageLevel.Level4);
            }
            else
            {
                HelperMethods.OutputMessage(String.Format("XWF_GetSize() results " 
                    + "({1}) differs from XWF_GetItemSize() results ({2}).", itemSize
                    , itemSize2), OutputMessageLevel.Level4);
            }

            // Get the size of the logical item with slack.
            itemSize = HelperMethods.GetSize(LogicalSlackItem
                , ItemSizeType.PhysicalSize);
            HelperMethods.OutputMessage("Logical (w/ Slack) Size: "
                + itemSize, OutputMessageLevel.Level4);

            // Close the open items.
            HelperMethods.CloseItem(LogicalItem);
            HelperMethods.OutputMessage("Logical item closed."
                , OutputMessageLevel.Level4);

            HelperMethods.CloseItem(LogicalSlackItem);
            HelperMethods.OutputMessage("Logical item with slack closed."
                , OutputMessageLevel.Level4);

            // Not sure we need this.
            //HelperMethods.XWF_SelectVolumeSnapshot(volume);

            // Get the item's information.
            ItemInformation itemInfo = HelperMethods.GetItemInformation(itemID);

            HelperMethods.OutputMessage("Original ID: "
                + itemInfo.OriginalItemID, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Attributes: "
                + itemInfo.Attributes, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Flags: "
                + itemInfo.Flags, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Deletion Status: "
                + itemInfo.Deletion, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Hard Link Count: "
                + itemInfo.LinkCount, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Color Analysis: "
                + itemInfo.ColorAnalysis, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Recursive Child File Count: "
                + itemInfo.FileCount, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Embedded Offset: "
                + itemInfo.EmbeddedOffset, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Creation Time: "
                + itemInfo.CreationTime, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Modification Time: "
                + itemInfo.ModificationTime, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Last Access Time: "
                + itemInfo.LastAccessTime, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Entry Modification Time: "
                + itemInfo.EntryModificationTime, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Deletion Time: "
                + itemInfo.DeletionTime, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Internal Creation Time: "
                + itemInfo.InternalCreationTime, OutputMessageLevel.Level4);

            ItemType CurrentItemType = HelperMethods.GetItemType(itemID);
            HelperMethods.OutputMessage("Item Type: "
                + CurrentItemType.Type, OutputMessageLevel.Level4);
            HelperMethods.OutputMessage("Item Description: "
                + CurrentItemType.Description, OutputMessageLevel.Level4);

            ItemOffsets Offsets = HelperMethods.GetItemOffsets(itemID);
            if (Offsets.FileSystemDataStructureOffset != -1)
            {
                HelperMethods.OutputMessage("File System Data Structure Offset: "
                    + Offsets.FileSystemDataStructureOffset
                    , OutputMessageLevel.Level4);
            }

            if (Offsets.CarvedFileVolumeOffset != -1)
            {
                HelperMethods.OutputMessage("Carved File Volume Offset: "
                    + Offsets.CarvedFileVolumeOffset
                    , OutputMessageLevel.Level4);
            }

            HelperMethods.OutputMessage("Data Sector Start: "
                + Offsets.DataStartSector
                , OutputMessageLevel.Level4);

            Int32 nParentItemID = HelperMethods.GetItemParent(itemID);
            HelperMethods.OutputMessage("Parent ID: "
                + nParentItemID
                , OutputMessageLevel.Level4);

            string[] Associations = HelperMethods.GetReportTableAssociations(itemID);

            if (Associations.Length > 0)
            {
                int ReportNumber = 1;
                foreach (string sReportName in Associations)
                {
                    HelperMethods.OutputMessage(String.Format("Report {0} Name: {1}"
                        , ReportNumber, sReportName), OutputMessageLevel.Level4);
                    ReportNumber += 1;
                }
            }
            else
            {
                HelperMethods.OutputMessage("No report table associations."
                    , OutputMessageLevel.Level4);
            }

            // Add and get item comment.
            string itemCommentToAdd = "This is a comment.";
            HelperMethods.AddComment(itemID, itemCommentToAdd
                , AddCommentMode.Replace);

            string itemComment = HelperMethods.GetComment(itemID);
            if (itemComment != null)
            {
                HelperMethods.OutputMessage("Comment: " + itemComment
                    , OutputMessageLevel.Level4);
            }

            string itemMetadata = HelperMethods.GetExtractedMetadata(itemID);
            if (itemMetadata != null)
            {
                HelperMethods.OutputMessage("Metadata: " + itemMetadata
                    , OutputMessageLevel.Level4);
            }

            HelperMethods.OutputEmptyLine();
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
            HelperMethods.OutputHeader("ITEM INTERACTION TESTING MODULE"
                , OutputMessageLevel.Level1);

            ProcessItem(hVolume, 7554);
            ProcessItem(hVolume, 2680);
        }
    }
}
