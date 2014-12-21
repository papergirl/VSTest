/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System.Reflection;
using KittyAltruistic.CPlusPlusTestRunner;
using Microsoft.VsSDK.UnitTestLibrary;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject.MyToolWindowTest
{
    using TestPackage_UnitTestProject.Mocks;

    [TestClass()]
    public class ShowToolWindowTest
    {
        [TestMethod()]
        public void ValidateToolWindowShown()
        {
            IVsPackage package = new TestPackage() as IVsPackage;

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            //Add uishell service that knows how to create a toolwindow
            BaseMock uiShellService = UIShellServiceMock.GetUiShellInstanceCreateToolWin();
            serviceProvider.AddService(typeof(SVsUIShell), uiShellService, false);
            BaseMock vsShellService = VSShellMock.GetVsShellInstance0();
            serviceProvider.AddService(typeof(SVsShell),vsShellService,false);
            // Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK");

            MethodInfo method = typeof(TestPackage).GetMethod("ShowTestList", BindingFlags.NonPublic | BindingFlags.Instance);

            object result = method.Invoke(package, new object[] { null, null });
        }

        [TestMethod()]
        [ExpectedException(typeof(TargetInvocationException), "Did not throw expected exption when windowframe object was null")]
        public void ShowToolwindowNegativeTest()
        {
            IVsPackage package = new TestPackage() as IVsPackage;

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            //Add uishell service that knows how to create a toolwindow
            BaseMock uiShellService = UIShellServiceMock.GetUiShellInstanceCreateToolWinReturnsNull();
            serviceProvider.AddService(typeof(SVsUIShell), uiShellService, false);
            BaseMock vsShellService = VSShellMock.GetVsShellInstance0();
            serviceProvider.AddService(typeof(SVsShell), vsShellService, false);
            // Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK");

            MethodInfo method = typeof(TestPackage).GetMethod("ShowTestList", BindingFlags.NonPublic | BindingFlags.Instance);

            //ShowToolWindow throw NotSupportException but the Exception is converted during the
            //call of invoke method
            object result = method.Invoke(package, new object[] { null, null });
        }
    }
}
