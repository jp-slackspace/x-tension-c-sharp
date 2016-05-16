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
            HelperMethods.XWF_OutputHeader("BLOCK MANIPULATION TESTING MODULE"
                , OutputMessageLevel.Level1);

            // XWF_GetBlock test.
            HelperMethods.XWF_OutputHeader("XWF_GetBlock() Test"
                , OutputMessageLevel.Level2);

            BlockBoundaries blockBoundaries = HelperMethods.XWF_GetBlock(hVolume);

            if (blockBoundaries.EndOffset == -1)
            {
                HelperMethods.XWF_OutputMessage("No block defined."
                    , OutputMessageLevel.Level3);
            }
            else
            {
                HelperMethods.XWF_OutputMessage(String.Format(
                    "Block defined from offsets {0} to {1}", 
                    blockBoundaries.StartOffset, blockBoundaries.EndOffset)
                    , OutputMessageLevel.Level3);
            }

            HelperMethods.XWF_OutputEmptyLine();

            // XWF_SetBlock test.
            HelperMethods.XWF_OutputHeader("XWF_SetBlock() Test"
                , OutputMessageLevel.Level2);

            BlockBoundaries newBoundaries;
            newBoundaries.StartOffset = 4;
            newBoundaries.EndOffset = 8;

            bool setBlockResult = HelperMethods.XWF_SetBlock(hVolume, newBoundaries);
            blockBoundaries = HelperMethods.XWF_GetBlock(hVolume);

            if (blockBoundaries.Equals(newBoundaries))
            {
                HelperMethods.XWF_OutputMessage("Block successfully created."
                    , OutputMessageLevel.Level3);
            }
            else
            {
                HelperMethods.XWF_OutputMessage("Block creation failed."
                    , OutputMessageLevel.Level3);
            }

            newBoundaries.StartOffset = 0;
            newBoundaries.EndOffset = -1;
            setBlockResult = HelperMethods.XWF_SetBlock(hVolume, newBoundaries);
            blockBoundaries = HelperMethods.XWF_GetBlock(hVolume);

            if (blockBoundaries.Equals(newBoundaries))
            {
                HelperMethods.XWF_OutputMessage("Block successfully cleared."
                    , OutputMessageLevel.Level3);
            }
            else
            {
                HelperMethods.XWF_OutputMessage("Block clearing failed."
                    , OutputMessageLevel.Level3);
            }

            HelperMethods.XWF_OutputEmptyLine();
        }
    }
}
