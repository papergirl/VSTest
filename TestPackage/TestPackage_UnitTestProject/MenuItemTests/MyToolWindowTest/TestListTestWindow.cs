/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System.Reflection;
using System.Threading;
using KittyAltruistic.CPlusPlusTestRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestPackage_UnitTestProject.Mocks;
using TestPackage_UnitTestProject.Properties;

namespace UnitTestProject.TestListTest
{
    using System;
    using System.IO;

    /// <summary>
    ///This is a test class for MyToolWindowTest and is intended
    ///to contain all MyToolWindowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TestListTest
    {

        /// <summary>
        ///MyToolWindow Constructor test
        ///</summary>
        [TestMethod()]
        public void TestListConstructorTest()
        {
            TestList target = new TestList();
            Assert.IsNotNull(target, "Failed to create an instance of TestList");

            FieldInfo field = target.GetType().GetField("Control", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field.GetValue(target), "TestList object was not instantiated");
        }

        public void WindowPropertyTest()
        {
            TestList target = new TestList();
            Assert.IsNotNull(target.Window, "Window property was null");
        }

        [TestMethod]
        public void ListTestsTest()
        {
            ManualResetEvent testFinishedSuccessfully = new ManualResetEvent(false);
            bool testFinishedEventFired = false;

            GTestRunner ctrl = new GTestRunner();
            Assert.IsNotNull(ctrl, "Failed to create an instance of TestListCtrl");
            ConfiguredProject configuredProject = GetTestProject();
            ctrl.OnTestsUpdated += delegate(ConfiguredProject project, GTestResultCollection tests)
            {
                Assert.IsNotNull(project);
                VerfyTestListData(tests);
                testFinishedSuccessfully.Set();
                testFinishedEventFired = true;
            };
            ctrl.ListTests(configuredProject);

            testFinishedSuccessfully.WaitOne(2000, false);
            Assert.IsTrue(testFinishedEventFired, "Test Finished Event Fired");
        }

        private void VerfyTestListData(GTestResultCollection tests)
        {

            Assert.IsNotNull(tests);
            Assert.AreEqual(6,tests.TotalNumberOfTests, "Number Of tests = " + tests.TotalNumberOfTests.ToString());
            Assert.AreEqual(0, tests.TotalNumberOfErrors, "Number of errors = " + tests.TotalNumberOfErrors.ToString());
        }

        [TestMethod]
        public void RunTest()
        {
            ManualResetEvent testFinishedSuccessfully = new ManualResetEvent(false);
            bool testFinishedEventFired = false;
            GTestRunner ctrl = new GTestRunner();
            Assert.IsNotNull(ctrl, "Failed to create an instance of GTestRunner");
            ConfiguredProject configuredProject = GetTestProject();
            //check we have a filter string
            ctrl.OnTestsUpdated += delegate(ConfiguredProject project, GTestResultCollection tests)
            {
                Assert.IsNotNull(project);
                VerfyTestRunData(tests);
                testFinishedSuccessfully.Set();
                testFinishedEventFired = true;
            };
            ctrl.RunTests(configuredProject, "Test*",false);

            testFinishedSuccessfully.WaitOne(8000, false);
            Assert.IsTrue(testFinishedEventFired, "Test Finished Event Fired");
        }

        private void VerfyTestRunData(GTestResultCollection tests)
        {
            int sumOfNumberOfTests = 0;
            int sumOfErrors = 0;
            int sumOfFails = 0;
            int individualSumOfFails = 0;
            Assert.IsNotNull(tests);

            foreach (GTestSuite testSuite in tests.GetList())
            {
                sumOfNumberOfTests += testSuite.NumberOfTests;
                sumOfErrors += testSuite.NumberOfErrors;
                sumOfFails += testSuite.NumberOfFailures;
                foreach (GTestResult test in testSuite.GetList())
                {
                    if (!test.HasPassed()) individualSumOfFails += 1;
                    Assert.IsFalse(test.Name.Contains("DISABLED_"), "Test name still contains disabled flag");
                    Assert.IsNotNull(test.GetRelatedImage, "Problem getting test image");
                }
            }
            //check the totals reflect the actual results, (checks if parsing was successful)
            Assert.AreEqual(sumOfNumberOfTests, tests.TotalNumberOfTests);
            Assert.AreEqual(sumOfErrors, tests.TotalNumberOfErrors);
            Assert.AreEqual(sumOfFails, tests.TotalNumberOfFailures);
            Assert.AreEqual(individualSumOfFails, tests.TotalNumberOfFailures);
            //check expected results
            Assert.AreEqual(6, tests.TotalNumberOfTests);
            Assert.AreEqual(0, tests.TotalNumberOfErrors);
            Assert.AreEqual(2, tests.TotalNumberOfFailures);
        }

        [TestMethod]
        public void TestCanAttachVSDebugger()
        {
            ManualResetEvent testFinishedSuccessfully = new ManualResetEvent(false);
            bool testFinishedEventFired = false;
            GTestRunner ctrl = new GTestRunner();
            Assert.IsNotNull(ctrl, "Failed to create an instance of GTestRunner");
            ConfiguredProject configuredProject = GetTestProject();
            ctrl.OnDebuggerAttached += delegate()
                                           {
                                               testFinishedSuccessfully.Set();
                                               testFinishedEventFired = true;
                                           };
            try
            {
                ctrl.RunTests(configuredProject, "UnendingTest*", true);
                testFinishedSuccessfully.WaitOne(5000, false);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            finally
            {
                ctrl.ForceTestStop();
            }
            Assert.IsTrue(testFinishedEventFired);
            
        }


        private ConfiguredProject GetTestProject()
        {
            string currentDir = Path.GetFullPath(Settings.Default.TestDataPath);
            
            Assert.IsTrue(Directory.Exists(currentDir),"Test data directory("+ currentDir +") doesn't exist");
            return new ConfiguredProject
            {
                TestExe = currentDir + "TestData.exe",
                Name = "TestData.exe",
                BelongsTo = new ProjectMock()
            };
        }

    }
}