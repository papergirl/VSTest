using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace IntegrationTests
{
    [TestClass]
    public class SolutionTests
    {
        #region fields

        private delegate void ThreadInvoker();
        private TestContext _testContext;

        #endregion fields

        #region properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return _testContext; }
            set { _testContext = value; }
        }

        #endregion properties

        #region ctors

        public SolutionTests()
        {
        }

        #endregion ctors

        [TestMethod]
        [HostType("VS IDE")]
        [Ignore]
        public void CreateEmptySolution()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                TestUtils testUtils = new TestUtils();
                testUtils.CloseCurrentSolution(__VSSLNSAVEOPTIONS.SLNSAVEOPT_NoSave);
                testUtils.CreateEmptySolution(TestContext.TestDir, "CreateEmptySolution");
            });
        }

    }
}
