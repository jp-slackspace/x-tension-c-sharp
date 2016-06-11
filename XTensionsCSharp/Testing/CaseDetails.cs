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
            HelperMethods.OutputHeader("CASE DETAILS TESTING MODULE"
                , OutputMessageLevel.Level1);

            // XWF_GetCaseProp() test.
            HelperMethods.OutputHeader("XWF_GetCaseProp() Test"
                , OutputMessageLevel.Level2);

            CaseProperties caseProps = HelperMethods.GetCaseProperties();
            HelperMethods.OutputMessage("Case Title: " + caseProps.CaseTitle
                , OutputMessageLevel.Level3);
            HelperMethods.OutputMessage("Case Examiner: " + caseProps.CaseExaminer
                , OutputMessageLevel.Level3);
            HelperMethods.OutputMessage("Case File Path: " 
                + caseProps.CaseFilePath, OutputMessageLevel.Level3);
            HelperMethods.OutputMessage("Case Directory: " 
                + caseProps.CaseDirectory, OutputMessageLevel.Level3);
            HelperMethods.OutputEmptyLine();
        }
    }
}
