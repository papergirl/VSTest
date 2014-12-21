using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.VCProjectEngine;
using Debugger = System.Diagnostics.Debugger;
using IOLEServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace KittyAltruistic.CPlusPlusTestRunner
{

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the registration utility (regpkg.exe) that this class needs
    // to be registered as package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // A Visual Studio component can be registered under different regitry roots; for instance
    // when you debug your package you want to register it in the experimental hive. This
    // attribute specifies the registry root to use if no one is provided to regpkg.exe with
    // the /root switch.
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\9.0")]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration(false,"#110", "#112", "1.2", IconResourceID = 400)]
    // In order be loaded inside Visual Studio in a machine that has not the VS SDK installed,
    // package needs to have a valid load key (it can be requested at
    // http://msdn.microsoft.com/vstudio/extend/). This attributes tells the shell that this
    // package has a load key embedded in its resources.
    [ProvideLoadKey("Standard", "1.0", "TestPackage", "KittyAltruistic", 1)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource(1000, 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(TestList), Orientation = ToolWindowOrientation.Bottom, Style = VsDockStyle.Tabbed, MultiInstances = false)]
    [ProvideToolWindow(typeof(TestFailure), Orientation = ToolWindowOrientation.Left, Style = VsDockStyle.Linked, MultiInstances = false)]

    //This attribute registers the test markers exposed
    [ProvideService(typeof(TestMarkerProvider), ServiceName = "Test Marker Service")]
    [ProvideTextMarker("GTestMarker", GuidList.GUIDTestMarkerString, GuidList.GUIDTestMarkerProviderString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [Guid(GuidList.GUIDTestPackagePkgString)]
    public sealed class TestPackage : Package, IVsShellPropertyEvents
    {
        const string DebugTestOutputWindow = "Test Output";
        private static OutputWindowPane _debugOut;
        private static TestFailure _testFailureWindow;
        private DTE2 _dte;
        private BuildEvents _buildEvents;
        private SolutionEvents _solutionEvents;
        private Solution _currentSolution;
        private CPlusPlusTestConfig _config;
        private IVsTextManager _txtMgr;
        private DocumentEvents _documentEvents;
        private readonly Dictionary<string, ConfiguredProject> _projectsWithTests = new Dictionary<string, ConfiguredProject>();
        private readonly Dictionary<string, IVsTextLines> _referencesToMarkers = new Dictionary<string, IVsTextLines>();
        private IOLEServiceProvider _oleServiceProvider;
        private TestMarkerProvider _provider = new TestMarkerProvider();
        private readonly List<IVsTextLineMarker[]> _markers = new List<IVsTextLineMarker[]>();
        private static OleMenuCommandService _mcs;
        private static TestList _testListWindow;
        private uint _cookie;
        public TestPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            //Debugger.Launch();
            base.Initialize();
            if (false == this.AttemptInit())
            {
                IVsShell shellService = GetService(typeof(SVsShell)) as IVsShell;

                if (shellService != null)
                    ErrorHandler.ThrowOnFailure(shellService.AdviseShellPropertyChanges(this, out _cookie));
                else
                {
                    Debug.WriteLine("shellService == null Unable to set initializing routine exiting...");
                    return;
                }
            }
            //   _markers = null;
            // Add our command handlers for menu (commands must exist in the .vsct file)
            _mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != _mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.GUIDTestPackageCmdSet, (int)PkgCmdIDList.cmdTestSettings);
                OleMenuCommand menuItem = new OleMenuCommand(MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += TestSettingsVisiblityCallback;
                _mcs.AddCommand(menuItem);
                // Create the command for the tool window
                CommandID toolwndCommandID = new CommandID(GuidList.GUIDTestPackageCmdSet, (int)PkgCmdIDList.cmdTestList);
                MenuCommand menuToolWin = new MenuCommand(ShowTestList, toolwndCommandID);
                _mcs.AddCommand(menuToolWin);
             //   CommandID ctxRunTestsID = new CommandID(GuidList.GUIDTestMarkerCmdSet,(int)PkgCmdIDList.cmdRunTest);
              //  _mcs.AddCommand(new MenuCommand(test,ctxRunTestsID));
            }

            _oleServiceProvider = (IOLEServiceProvider)GetService(typeof(IOLEServiceProvider));
            IServiceContainer container = (IServiceContainer)this;
            container.AddService(typeof(TestMarkerProvider), _provider, true);


            ToolWindowPane failure = this.FindToolWindow(typeof(TestFailure), 0, true);
            if ((null == failure) || (null == failure.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            _testFailureWindow = (TestFailure)failure;
            ToolWindowPane testlist = this.FindToolWindow(typeof(TestList), 0, true);
            if ((null == testlist) || (null == testlist.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            _testListWindow = (TestList) testlist;

        }

        private void DocumentEvents_DocumentOpened(Document document)
        {
            //check if the item opened is a text document
            TextDocument txtDoc = document.Object("") as TextDocument;
            if (txtDoc == null)
                return;
            //is the text document a member of one of our projects with tests?
            if(!_projectsWithTests.ContainsKey(document.ProjectItem.ContainingProject.Name))
                return;
            ConfiguredProject project = _projectsWithTests[document.ProjectItem.ContainingProject.Name];
            if (project == null)
                return;
            IVsUIHierarchy uiHierarchy;
            uint itemID;
            IVsTextLines lines;
            IVsWindowFrame windowFrame;
            var serviceProvider = project.BelongsTo.DTE as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
            var x = new ServiceProvider(serviceProvider);
            IVsTextView view;
            //IVsTextManager2 txtManager2 = (IVsTextManager2) GetService(typeof (SVsTextManager));
            //txtManager2.GetActiveView2(0,null,(uint)_VIEWFRAMETYPE.vftAny,out view);
            
            if (VsShellUtilities.IsDocumentOpen(x, document.FullName, Guid.Empty,
                                   out uiHierarchy, out itemID, out windowFrame))
            {
                view = VsShellUtilities.GetTextView(windowFrame);  
                view.GetBuffer(out lines);
            }
            else
                throw new Exception("COM happened");
            if (!_referencesToMarkers.ContainsKey(document.FullName))
                _referencesToMarkers.Add(document.FullName, lines);
            MarkTests(project, lines);

        }

        #endregion Package Members

        private void GetTestsForProject(Project p)
        {
            Contract.Requires(p != null);
            if (p.ConfigurationManager != null)
                GetTestsForProject(p, p.ConfigurationManager.ActiveConfiguration.ConfigurationName);
        }

        private void GetTestsForProject(Project p, string configName)
        {
            Contract.Requires(p != null);
            Contract.Requires(!String.IsNullOrEmpty(configName));

            if (p.Kind != GuidList.CPPProject)
                return;
            // get build configuration for project, name and path of the output
            // to feed gtest.
            VCProject cppProject = (VCProject)p.Object;
            VCConfiguration vcC =
                (VCConfiguration)
                (((IVCCollection)cppProject.Configurations).Item(configName));
            if (!vcC.PrimaryOutput.EndsWith("exe", true, CultureInfo.CurrentCulture))
                return;
            ConfiguredProject project = _config.GetConfiguration(p);

            project.Name = p.Name;
            project.TestExe = vcC.PrimaryOutput;
            //try to get tests if a build exists)
            if (File.Exists(vcC.PrimaryOutput))
            {
                TestList window = GetTestList();
                if (window.DisplayAvaliableTests(project))
                {
                    if (!_projectsWithTests.ContainsKey(project.Name))
                        _projectsWithTests.Add(project.Name, project);
                }
            }
        }

        private void MarkTests(ConfiguredProject configuredCPPProject, IVsTextLines lines)
        {

            int lineNumber;
            lines.GetLineCount(out lineNumber);
            for (int i = 0;i < lineNumber;i++)
            {
                int linelength;
                lines.GetLengthOfLine(i, out linelength);
                if (linelength <= 0)
                       continue;
                string codeLine;
                lines.GetLineText(i, 0, i, linelength, out codeLine);

                //TODO improve this so it deals with if the class declaration isn't the first thing on the line
                if (!codeLine.StartsWith("TEST") && !codeLine.StartsWith("GTEST"))
                    continue;

                //extract the test group name and test name and combine them for the gtest filter string.
                int endOfBrackets = codeLine.IndexOf(')');
                //TODO validate the next characters after the ) are a newline, curly bracket or a comment.
                int startOfBrackets = codeLine.IndexOf('(') + 1;
                //inside of brackets
                string name = codeLine.Substring(startOfBrackets, endOfBrackets - startOfBrackets); 
                name = name.Replace(',', '.').Replace(" ", "");
                IVsTextLineMarker[] marker = new IVsTextLineMarker[1];
              
                int err = lines.CreateLineMarker(TestMarkerType.ID, i, 0, i,
                                       linelength, new GTestMarker(configuredCPPProject, name),marker);

                if (err != VSConstants.S_OK)
                    throw new Exception("Could not create marker");
                _markers.Add(marker);
            }
        }

        #region "Windows and tools accessors"

        private IVsTextManager GetTextManager()
        {
            Contract.Requires(_dte != null);
            if (_txtMgr != null)
                return _txtMgr;
            _txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            return _txtMgr;

        }

        public static OutputWindowPane GetOutputWindow()
        {
            return _debugOut;
        }


        public static TestFailure GetTestFailureWindow()
        {
            return _testFailureWindow;
        }

        public static TestList GetTestList()
        {
            return _testListWindow;
        }
        #endregion

        #region "Event handlers"

        private void TestSettingsVisiblityCallback(object caller, EventArgs args)
        {
            OleMenuCommand command = caller as OleMenuCommand;
            if (command == null) return;
            if (command.CommandID.Guid != GuidList.GUIDTestPackageCmdSet) return;
            Project p = _dte.SelectedItems.Item(1).Project;

            //only used with c++ projects! Unsure why this should ever be null... I think this called when it shouldn't be some time!
            if (p == null || p.Kind != GuidList.CPPProject)
                command.Visible = false;
            else
            {
                //tests can only appear on executables.
                VCProject vcProject = (VCProject)p.Object;
                VCConfiguration vcC =
                (VCConfiguration)
                (((IVCCollection)vcProject.Configurations).Item(p.ConfigurationManager.ActiveConfiguration.ConfigurationName));

                command.Visible = vcC.PrimaryOutput.EndsWith("exe", true, CultureInfo.CurrentCulture);

            }
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            //our code wasn't loaded to catch the solution opened event?!!
            if (_config == null)
            {
                //ok we can deal lets just fire the event manually to do the initialization
                SolutionEvents_Opened();
            }
            if (_config == null)
                throw new NullReferenceException("Unable to Open Configuration");
            ConfiguredProject project = _config.GetConfiguration(_dte.SelectedItems.Item(1).Project);
            GSettings settings = new GSettings(project);
            settings.Closed += Settings_Closed;
            settings.ShowDialog();
            settings.Focus();
        }


        private void Settings_Closed(object sender, EventArgs e)
        {
            Contract.Requires(sender.GetType() == typeof(GSettings));
            GSettings settings = (GSettings)sender;
            if (!settings.Configuration.Dirty)
                return;
            _config.Save();

            TestList window = GetTestList();
            window.DisplayAvaliableTests(settings.Configuration);
        }

        /// <summary>
        /// Opens The Test List
        /// </summary>
        private void ShowTestList(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = GetTestList();
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        private void SolutionEvents_AfterClosing()
        {
            _currentSolution = null;
            TestList window = GetTestList();
            window.ClearTests();
        }

        private void SolutionEvents_Opened()
        {

            _currentSolution = _dte.Solution;
            _config = CPlusPlusTestConfig.Open(_currentSolution);
            foreach (Project p in _currentSolution.Projects)
                GetTestsForProject(p);

        }

        private void BuildEvents_OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {

            if (!success)
                return;
            foreach (Project p in _currentSolution.Projects)
            {
                if (p.UniqueName != project)
                    continue;
                GetTestsForProject(p, projectConfig);
            }
        }

        #endregion "Event handlers"

        public static OleMenuCommandService GetMenuService()
        {
            return _mcs;
        }

        /// <summary>
        /// Required to initalize DTE when visual studio stops 
        /// eating brains. 
        /// </summary>
        /// <param name="propid"></param>
        /// <param name="var"></param>
        /// <returns></returns>
        public int OnShellPropertyChange(int propid, object var)
        {
            // when zombie state changes to false, finish package initialization

            if (((int)__VSSPROPID4.VSSPROPID_ShellInitialized == propid && (bool)var) || 
                ((int)__VSSPROPID.VSSPROPID_Zombie == propid && (bool)var == false))
            {

                if (AttemptInit())
                {
                    this._cookie = 0;

                    // eventlistener no longer needed
                    IVsShell shellService = GetService(typeof(SVsShell)) as IVsShell;

                    if (null != shellService)
                        ErrorHandler.ThrowOnFailure(shellService.UnadviseShellPropertyChanges(this._cookie));
                }
            }

            return VSConstants.S_OK;
        }
        /// <summary>
        /// Sets up the DTE and other code which depends on 
        /// visual studios initalization being complete
        /// </summary>
        /// <returns>true id initialization was completed successfully</returns>
        private bool AttemptInit(){

            
            // zombie state dependent code
            this._dte = (DTE2)GetService(typeof(DTE));
            if (this._dte == null)
                return false;
            //setup all our windows and panes here
            OutputWindowPanes panes =
            _dte.ToolWindows.OutputWindow.OutputWindowPanes;
            // Create a new pane.
            try
            {
                _debugOut = panes.Item(DebugTestOutputWindow);
            }
            catch
            {
                _debugOut = panes.Add(DebugTestOutputWindow);
            }

            //events we're intrested in.
            _buildEvents = _dte.Events.BuildEvents;
            _solutionEvents = _dte.Events.SolutionEvents;
            _documentEvents = _dte.Events.get_DocumentEvents(null);
            _solutionEvents.Opened += SolutionEvents_Opened;
            _solutionEvents.AfterClosing += SolutionEvents_AfterClosing;
            _buildEvents.OnBuildProjConfigDone += BuildEvents_OnBuildProjConfigDone;
            _documentEvents.DocumentOpened += DocumentEvents_DocumentOpened;

            TestMarkerProvider.InitializeMarkerIds(GetTextManager());

            
            return true;
        }
    }
}