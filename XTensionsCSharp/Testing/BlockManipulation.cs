using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace XTensions.Testing
{
    public static class BlockManipulation
    {
        /// <summary>
        /// Run a test for the block manipulation methods.
        /// </summary>
        /// <param name="hVolume">The current volume pointer.</param>
        /// <param name="nOpType">The current operation type.</param>
        /// <returns></returns>
        public static void RunTest(IntPtr hVolume, XTensionActionSource nOpType)
        {
            HelperMethods.OutputHeader("BLOCK MANIPULATION TESTING MODULE"
                , OutputMessageLevel.Level1);

            // XWF_GetBlock test.
            HelperMethods.OutputHeader("XWF_GetBlock() Test"
                , OutputMessageLevel.Level2);

            BlockBoundaries blockBoundaries = HelperMethods.GetBlock(hVolume);

            if (blockBoundaries.EndOffset == -1)
            {
                HelperMethods.OutputMessage("No block defined."
                    , OutputMessageLevel.Level3);
            }
            else
            {
                HelperMethods.OutputMessage(String.Format(
                    "Block defined from offsets {0} to {1}", 
                    blockBoundaries.StartOffset, blockBoundaries.EndOffset)
                    , OutputMessageLevel.Level3);
            }

            HelperMethods.OutputEmptyLine();

            // XWF_SetBlock test.
            HelperMethods.OutputHeader("XWF_SetBlock() Test"
                , OutputMessageLevel.Level2);

            BlockBoundaries newBoundaries;
            newBoundaries.StartOffset = 4;
            newBoundaries.EndOffset = 8;

            bool setBlockResult = HelperMethods.SetBlock(hVolume, newBoundaries);
            blockBoundaries = HelperMethods.GetBlock(hVolume);

            if (blockBoundaries.Equals(newBoundaries))
            {
                HelperMethods.OutputMessage("Block successfully created."
                    , OutputMessageLevel.Level3);
            }
            else
            {
                HelperMethods.OutputMessage("Block creation failed."
                    , OutputMessageLevel.Level3);
            }

            newBoundaries.StartOffset = 0;
            newBoundaries.EndOffset = -1;
            setBlockResult = HelperMethods.SetBlock(hVolume, newBoundaries);
            blockBoundaries = HelperMethods.GetBlock(hVolume);

            if (blockBoundaries.Equals(newBoundaries))
            {
                HelperMethods.OutputMessage("Block successfully cleared."
                    , OutputMessageLevel.Level3);
            }
            else
            {
                HelperMethods.OutputMessage("Block clearing failed."
                    , OutputMessageLevel.Level3);
            }

            HelperMethods.OutputEmptyLine();
        }
    }
}
