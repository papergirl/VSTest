/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.IO;
using System.Reflection;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VsSDK.UnitTestLibrary;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Shell;
using KittyAltruistic.CPlusPlusTestRunner;
using TestPackage_UnitTestProject.MenuItemTests;

namespace UnitTestProject.MenuItemTests
{
    using TestPackage_UnitTestProject.Mocks;

    [TestClass()]
    public class ConfigureTest
    {

        /// <summary>
        /// Verify that a new menu command object gets added to the OleMenuCommandService. 
        /// This action takes place In the Initialize method of the Package object
        /// </summary>
        [TestMethod]
        public void InitializeConfigMenuCommands()
        {
            // Create the package
            IVsPackage package = new TestPackage();
            Assert.IsNotNull(package, "The object does not implement IVsPackage");

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            BaseMock uiShellService = MyToolWindowTest.UIShellServiceMock.GetUiShellInstanceCreateToolWin();
            serviceProvider.AddService(typeof(SVsUIShell), uiShellService, false);
            
            BaseMock vsShellService = VSShellMock.GetVsShellInstance0();
            serviceProvider.AddService(typeof(SVsShell), vsShellService, false);

            // Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK");

            //Verify that the menu command can be found
            CommandID menuCommandID = new CommandID(GuidList.GUIDTestPackageCmdSet, (int)PkgCmdIDList.cmdTestSettings);
            MethodInfo info = typeof(Package).GetMethod("GetService", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(info);
            OleMenuCommandService mcs = info.Invoke(package, new object[] { (typeof(IMenuCommandService)) }) as OleMenuCommandService;
            Assert.IsNotNull(mcs);
            Assert.IsNotNull(mcs.FindCommand(menuCommandID));
        }

        [TestMethod]
        public void ConfigMenuItemCallback()
        {
            // Create the package
            IVsPackage package = new TestPackage();
            Assert.IsNotNull(package, "The object does not implement IVsPackage");

            // Create a basic service provider
            OleServiceProvider serviceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            // Create a UIShell service mock and proffer the service so that it can called from the MenuItemCallback method
            BaseMock uishellMock = UIShellServiceMock.GetUiShellInstance();
            serviceProvider.AddService(typeof(SVsUIShell), uishellMock, true);
            
            // Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK");

            //Invoke private method on package class and observe that the method does not throw
            MethodInfo info = package.GetType().GetMethod("MenuItemCallback", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(info, "Failed to get the private method MenuItemCallback throug refplection");
            //we can't invoke this without mocking the dte which isn't done yet
            //info.Invoke(package, new object[] { null, null });

            //Clean up services
            serviceProvider.RemoveService(typeof(SVsUIShell));
        }
        /// <summary>
        /// Checks if we can successfully save and load a project's configuration
        /// </summary>
        [TestMethod]
        public void ConfigurationSaveAndRestorationTest()
        {
             Solution mockSolution = new SolutionMock();
             CPlusPlusTestConfig testConfiguration = CPlusPlusTestConfig.Open(mockSolution);
             Assert.IsFalse(String.IsNullOrEmpty(testConfiguration.FilePath),"File Path is null");
             testConfiguration.Projects.Add(new ConfiguredProject {Name ="Mock Project",TestExe = "mock.exe"});
             testConfiguration.Save();
             Assert.IsTrue(File.Exists(testConfiguration.FilePath),"Failed to create configuration file");
             testConfiguration = CPlusPlusTestConfig.Open(mockSolution);
             ConfiguredProject project = testConfiguration.Projects[0];
             Assert.IsNotNull(project,"Failed to restore project for config");
             Assert.AreEqual(project.ListTestCommand,Resources.DefaultGTestList,"List Test's Command Failed to save correctly");
             Assert.AreEqual(project.Name, "Mock Project", "Project name Failed to save correctly");
             Assert.AreEqual(project.RunTestCommand, Resources.DefaultGTestRun, "Run Test's Command Failed to save correctly");
             Assert.AreEqual(project.TestExe, "mock.exe", "Test Exe failed to save correctly");
             File.Delete(testConfiguration.FilePath);
        }
        
       
    }
}
