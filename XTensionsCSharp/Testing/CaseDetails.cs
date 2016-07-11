using System;

namespace XTensions.Testing
{
    public static class CaseDetails
    {
        /// <summary>
        /// Run a test for the current category methods.
        /// </summary>
        /// <param name="hVolume">The current volume pointer.</param>
        /// <param name="nOpType">The current operation type.</param>
        /// <returns></returns>
        public static void RunTest(IntPtr hVolume, XTensionActionSource nOpType)
        {
            HelperMethods.OutputMessage("CASE DETAILS TESTING MODULE", 
                OutputMessageOptions.Level1 | OutputMessageOptions.Header);

            // XWF_GetCaseProp() test.
            HelperMethods.OutputMessage("XWF_GetCaseProp() Test", 
                OutputMessageOptions.Level2 | OutputMessageOptions.Header);

            CaseProperties caseProps = HelperMethods.GetCaseProperties();

            HelperMethods.OutputMessage("Case Title: " + caseProps.CaseTitle
                , OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("Case Examiner: " + caseProps.CaseExaminer
                , OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("Case File Path: " 
                + caseProps.CaseFilePath, OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("Case Directory: " 
                + caseProps.CaseDirectory, OutputMessageOptions.Level3);
            HelperMethods.OutputMessage("");
        }
    }
}
