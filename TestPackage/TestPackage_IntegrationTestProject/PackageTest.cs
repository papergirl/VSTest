using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestPackage_IntegrationTestProject.IntegrationTest_Library;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace IntegrationTestProject
{
    /// <summary>
    /// Integration test for package validation
    /// </summary>
    [TestClass]
    public class PackageTest
    {
        private delegate void ThreadInvoker();

        private TestContext testContextInstance;

        [TestInitialize()]
        public void Initialize()
        {
            MessageFilter.Register();
        }

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

        [TestMethod]
        [HostType("VS IDE")]
        [Ignore]
        public void PackageLoadTest()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {

                //Get the Shell Service
                IVsShell shellService = VsIdeTestHostContext.ServiceProvider.GetService(typeof(SVsShell)) as IVsShell;
                Assert.IsNotNull(shellService);

                //Validate package load
                IVsPackage package;
                Guid packageGuid = new Guid(KittyAltruistic.CPlusPlusTestRunner.GuidList.GUIDTestPackagePkgString);
                Assert.IsTrue(0 == shellService.LoadPackage(ref packageGuid, out package));
                Assert.IsNotNull(package, "Package failed to load");

            });
        }

        [TestCleanup()]
        public void Cleanup()
        {
            MessageFilter.Revoke();
        }
    }
}
