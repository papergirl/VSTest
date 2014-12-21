using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using EnvDTE;
using EnvDTE80;
using Debugger = EnvDTE.Debugger;
using Process = System.Diagnostics.Process;
using Thread = System.Threading.Thread;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    public class GTestRunner : ITestListener
    {
        private ConfiguredProject _currentTestsFor;
        private bool _testsRunning;
        private Process _testProcess;
        private string _testExeArgs;
        private const string TestFileName = "vsgtestrun.xml";
        private readonly Dictionary<string, GTestResultCollection> TestSets = new Dictionary<string, GTestResultCollection>();

        public delegate void DebuggerAttached();
        public event DebuggerAttached OnDebuggerAttached;
        
        public delegate void TestsUpdated(ConfiguredProject project, GTestResultCollection tests);
        public event TestsUpdated OnTestsUpdated;
        public const string RUN_ALL = "*";

        public Dictionary<string, GTestResultCollection> TestsInRunner
        {
            get { return TestSets; }
        }

        public void ListTests(ConfiguredProject project)
        {
            GTestResultCollection testlist;
            OutputWindowPane debugOut = TestPackage.GetOutputWindow();
            Process testProcess = new Process();
            // first try and vaildate as a vaild gtest executable
            _testExeArgs = "--help";
            FileInfo info = new FileInfo(project.TestExe);
            testProcess.StartInfo = new ProcessStartInfo(info.FullName)
                                        {
                                            UseShellExecute = false,
                                            WindowStyle = ProcessWindowStyle.Minimized,
                                            Arguments = _testExeArgs,
                                            CreateNoWindow = true,
                                            RedirectStandardError = true,
                                            RedirectStandardOutput = true,
                                            WorkingDirectory = info.DirectoryName ?? Directory.GetCurrentDirectory()
                                        };
            //   testProcess.OutputDataReceived += ReadIntoBuffer;
            testProcess.Start();
            string list = testProcess.StandardOutput.ReadLine();
            if (list != Resources.GTestHelpString)
            {
                testProcess.WaitForExit(400);
                if (!testProcess.HasExited)
                    testProcess.Kill();
                if (testProcess.ExitCode != 0 && debugOut != null)
                    debugOut.OutputString("An error occured while attempting to run " + info.Name +
                                          " process exited with " + testProcess.ExitCode);
                return;
            }

            if (debugOut != null)
                debugOut.OutputString("Tests found for " + info.Name + "..loading tests now...");

            _testExeArgs = project.ListTestCommand;
            testProcess.StartInfo.Arguments = _testExeArgs;
            testProcess.Start();
            list = testProcess.StandardOutput.ReadToEnd();
            testProcess.WaitForExit(200);

            if (!TestSets.ContainsKey(project.TestExe))
            {
                testlist = new GTestResultCollection();
                TestSets.Add(project.TestExe, testlist);
            }
            else
            {
                testlist = TestSets[project.TestExe];
            }

            string[] splitList = list.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            GTestSuite suite = null;

            foreach (string t1 in splitList)
            {
                string actualName = t1.Trim(new[] { ' ', '\n', '\r' });
                if (actualName.Contains("."))
                {
                    actualName = actualName.TrimEnd('.');
                    int suiteIndex = testlist.TestResults.FindIndex(t => t.Name == actualName);
                    if (suiteIndex == -1)
                    {
                        suite = new GTestSuite { Name = actualName, NumberOfTests = 0 };
                        testlist.TestResults.Add(suite);
                    }
                    else
                        suite = testlist.TestResults[suiteIndex];
                    continue;
                }
                if (suite == null)
                    throw new NullReferenceException("ACK THESE BLACKHOLES ARE EVERYWHERE");

                int disabled = 0;
                if (actualName.StartsWith("DISABLED_"))
                {
                    disabled = 1;
                    actualName = actualName.Replace("DISABLED_", string.Empty);
                }
                GTestResult testResult = suite.Results.Find(t => t.Name == actualName);
                if (testResult == null)
                {
                    testResult = new GTestResult { Name = actualName, Disabled = disabled };
                    suite.Results.Add(testResult);
                    testlist.TotalNumberOfTests++;
                    suite.NumberOfTests++;

                }

            }
            if (OnTestsUpdated != null) OnTestsUpdated.Invoke(project, testlist);
        }

        public void RunTests(ConfiguredProject project, string filterString,bool debugProcess)
        {
            if (!File.Exists(project.TestExe))
            {
                throw new TestRunnerException("The project must be built b4 running the tests");
            }
            lock (this)
            {
                if (_testsRunning)
                    throw new TestRunnerException("Tests are already running please wait and try again");
                _testsRunning = true;
            }

            Thread testFinishChecker = new Thread(CheckIfTestsHaveFinished);
            _currentTestsFor = project;
            _testProcess = new Process();
            _testExeArgs = project.RunTestCommand.Replace("$outputFile$", TestFileName).Replace("$tests$", filterString);
            _testProcess.StartInfo = new ProcessStartInfo(project.TestExe, _testExeArgs)
            {
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };
            
            _testProcess.Start();
            testFinishChecker.Start();
            //there is probably a race condition here if the test process will run b4 we manage to attach then 
            //a user break point will probably be missed.
            if (debugProcess)
            {
                foreach (EnvDTE.Process process in project.BelongsTo.DTE.Debugger.LocalProcesses)
                {
                    if (process.ProcessID == _testProcess.Id)
                    {
                        process.Attach();
                        if (OnDebuggerAttached != null) OnDebuggerAttached.Invoke();
                        
                    }
                }
            }
        }

        private void CheckIfTestsHaveFinished()
        {
            OutputWindowPane debugOut = TestPackage.GetOutputWindow();
            if (debugOut != null)
                debugOut.OutputString(_testProcess.StandardOutput.ReadToEnd());
            _testProcess.WaitForExit(5000);
            if(!_testProcess.HasExited)
                _testProcess.Kill();
            
            if (_testProcess.ExitCode > 1 || !File.Exists(TestFileName))
            {
                MessageBox.Show(Resources.CommandLineErrorMessage + TestFileName + @" " + _testExeArgs);
                _testProcess.Dispose();
                return;
            }
            _testProcess.Dispose();
            FileStream testFile = File.OpenRead(TestFileName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GTestResultCollection));

            GTestResultCollection newTestData = (GTestResultCollection)xmlSerializer.Deserialize(testFile);
            GTestResultCollection testDataToUpdate;
            if (!TestSets.ContainsKey(_testProcess.StartInfo.FileName))
            {
                testDataToUpdate = newTestData;
                TestSets.Add(_testProcess.StartInfo.FileName, testDataToUpdate);
            }
            else
            {
                testDataToUpdate = TestSets[_testProcess.StartInfo.FileName];
                testDataToUpdate.TotalNumberOfTests = newTestData.TotalNumberOfTests;
                testDataToUpdate.TotalNumberOfFailures = newTestData.TotalNumberOfFailures;
                testDataToUpdate.TotalNumberOfErrors = newTestData.TotalNumberOfErrors;
                testDataToUpdate.TotalTime = newTestData.TotalTime;
            }

            foreach (GTestSuite testSuite in newTestData.TestResults)
            {
                // check if this suite exists in current data.
                GTestSuite suite = testSuite;
                int currentTestSuiteIndex = testDataToUpdate.TestResults.FindIndex(t => t.Name == suite.Name);
                if (currentTestSuiteIndex == -1)
                {
                    testDataToUpdate.TestResults.Add(testSuite);
                    currentTestSuiteIndex = testDataToUpdate.TestResults.Count - 1;
                }
                else // update suite data
                {
                    GTestSuite currentsuite = testDataToUpdate.TestResults[currentTestSuiteIndex];
                    currentsuite.Time = testSuite.Time;
                    currentsuite.NumberOfTests = testSuite.NumberOfTests;
                    currentsuite.NumberOfFailures = testSuite.NumberOfFailures;
                    currentsuite.NumberOfErrors = testSuite.NumberOfErrors;
                } 
                for (int i = 0; i < testSuite.Results.Count;i++ )
                {
                    GTestResult test = testSuite.Results[i];
                    //check to see if the test exist in current data
                    string testName = test.Name;
                    bool disabled = testName.StartsWith("DISABLED_");
                    testName = test.Name.Replace("DISABLED_", "");
                    int currentTestIndex = testDataToUpdate.TestResults[currentTestSuiteIndex].Results.FindIndex(
                                   t => t.Name == testName);
                    GTestResult current = null;

                    if (currentTestIndex == -1)
                    {
                        testDataToUpdate.TestResults[currentTestSuiteIndex].Results.Add(test);
                        current = test;
                    }
                    else if (test.TestRan)
                    {
                        current =
                            testDataToUpdate.TestResults[currentTestSuiteIndex].Results[currentTestIndex];
                        current.Errors = test.Errors;
                        current.Status = test.Status;
                        current.Time = test.Time;
                    }
                    else
                    {
                        current = testDataToUpdate.TestResults[currentTestSuiteIndex].Results[currentTestIndex];
                        current.TestRan = false;
                    }
                    current.Disabled = disabled ? 1 : 0;
                }
            }

            testFile.Close();
            File.Delete(TestFileName);
            _testsRunning = false;
            if (OnTestsUpdated != null) OnTestsUpdated.Invoke(_currentTestsFor, testDataToUpdate);
        }
        public void ForceTestStop()
        {
            if (_testsRunning && !_testProcess.HasExited)
                _testProcess.Kill();
            
        }
    }
}
