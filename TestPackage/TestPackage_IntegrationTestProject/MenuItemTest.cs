using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;
using TestPackage_IntegrationTestProject.IntegrationTest_Library;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace TestPackage_IntegrationTestProject
{
    [TestClass()]
    public class MenuItemTest
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

        [TestInitialize]
        public void Init()
        {
            MessageFilter.Register();
        }

        /// <summary>
        ///A test for lauching the command and closing the associated dialogbox
        ///</summary>
        [TestMethod()]
        [Ignore]
        [HostType("VS IDE")]
        public void LaunchCommand()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                CommandID menuItemCmd = new CommandID(KittyAltruistic.CPlusPlusTestRunner.GuidList.GUIDTestPackageCmdSet, (int)KittyAltruistic.CPlusPlusTestRunner.PkgCmdIDList.cmdTestSettings);

                // Create the DialogBoxListener Thread.
                string expectedDialogBoxText = string.Format(CultureInfo.CurrentCulture, "{0}\n\nInside {1}.MenuItemCallback()", "GTest", "KittyAltruistic.CPlusPlusGTest.TestPackage");
                DialogBoxPurger purger = new DialogBoxPurger(NativeMethods.IDOK, expectedDialogBoxText);

                try
                {
                    purger.Start();

                    TestUtils testUtils = new TestUtils();
                    testUtils.ExecuteCommand(menuItemCmd);
                }
                finally
                {
                    Assert.IsTrue(purger.WaitForDialogThreadToTerminate(), "The dialog box has not shown");
                }
            });
        }

        [TestCleanup]
        public void cleanup()
        {
            MessageFilter.Revoke();
        }
    }
}
