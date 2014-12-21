using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPackage_IntegrationTestProject
{
    using Microsoft.VSSDK.Tools.VsIdeTesting;
    using Microsoft.VsSDK.IntegrationTestLibrary;

    [TestClass()]
    public class ToolWindowTest
    {
        private delegate void ThreadInvoker();

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for showing the toolwindow
        ///</summary>
        [TestMethod()]
        [HostType("VS IDE")]
        [Ignore]
        public void ShowToolWindow()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                CommandID toolWindowCmd = new CommandID(KittyAltruistic.CPlusPlusTestRunner.GuidList.GUIDTestPackageCmdSet, (int)KittyAltruistic.CPlusPlusTestRunner.PkgCmdIDList.cmdTestList);

                TestUtils testUtils = new TestUtils();
                testUtils.ExecuteCommand(toolWindowCmd);

                Assert.IsTrue(testUtils.CanFindToolwindow(new Guid(KittyAltruistic.CPlusPlusTestRunner.GuidList.GUIDTestListID)));

            });
        }

    }
}
