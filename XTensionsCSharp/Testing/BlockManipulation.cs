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
            HelperMethods.OutputMessage("BLOCK MANIPULATION TESTING MODULE", 
                OutputMessageOptions.Level1 | OutputMessageOptions.Header);

            // XWF_GetBlock test.
            HelperMethods.OutputMessage("XWF_GetBlock() Test", 
                OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            BlockBoundaries blockBoundaries = HelperMethods.GetBlock(hVolume);

            if (blockBoundaries.EndOffset == -1)
            {
                HelperMethods.OutputMessage("No block defined.", 
                    OutputMessageOptions.Level3);
            }
            else
            {
                HelperMethods.OutputMessage(String.Format(
                    "Block defined from offsets {0} to {1}", blockBoundaries.StartOffset, 
                    blockBoundaries.EndOffset), OutputMessageOptions.Level3);
            }

            HelperMethods.OutputMessage("");

            // XWF_SetBlock test.
            HelperMethods.OutputMessage("XWF_SetBlock() Test", 
                OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            BlockBoundaries newBoundaries;
            newBoundaries.StartOffset = 4;
            newBoundaries.EndOffset = 8;

            bool setBlockResult = HelperMethods.SetBlock(hVolume, newBoundaries);
            blockBoundaries = HelperMethods.GetBlock(hVolume);

            if (blockBoundaries.Equals(newBoundaries))
            {
                HelperMethods.OutputMessage("Block successfully created.", 
                    OutputMessageOptions.Level3);
            }
            else
            {
                HelperMethods.OutputMessage("Block creation failed.", 
                    OutputMessageOptions.Level3);
            }

            newBoundaries.StartOffset = 0;
            newBoundaries.EndOffset = -1;
            setBlockResult = HelperMethods.SetBlock(hVolume, newBoundaries);
            blockBoundaries = HelperMethods.GetBlock(hVolume);

            if (blockBoundaries.Equals(newBoundaries))
            {
                HelperMethods.OutputMessage("Block successfully cleared.", 
                    OutputMessageOptions.Level3);
            }
            else
            {
                HelperMethods.OutputMessage("Block clearing failed.", 
                    OutputMessageOptions.Level3);
            }

            HelperMethods.OutputMessage("");
        }
    }
}
