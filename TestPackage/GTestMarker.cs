using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    internal class GTestMarker : IVsTextMarkerClient,IOleCommandTarget
    {
        private readonly ConfiguredProject _projectMarkerBelongsTo;
        private readonly string _testName;


        public GTestMarker(ConfiguredProject projectMarkerBelongsTo, string testName)
        {
            _projectMarkerBelongsTo = projectMarkerBelongsTo;
            _testName = testName;
        }

        public void MarkerInvalidated()
        {

        }

        public int GetTipText(IVsTextMarker pMarker, string[] pbstrText)
        {
            pbstrText[0] = "Run Test";
            return VSConstants.S_OK;
        }

        public void OnBufferSave(string pszFileName)
        {
           
        }

        public void OnBeforeBufferClose()
        {
          
        }

        public int GetMarkerCommandInfo(IVsTextMarker pMarker, int iItem, string[] pbstrText, uint[] pcmdf)
        {
            //this is passed in as null first returning S_OK tells vs the marker supports this command
            if (pcmdf == null && pcmdf == null)
                return VSConstants.S_OK;
            // ReSharper disable BitwiseOperatorOnEnumWihtoutFlags
            const uint menuItemFlags = (uint)(
                  OLECMDF.OLECMDF_SUPPORTED
                | OLECMDF.OLECMDF_ENABLED);
            // ReSharper restore BitwiseOperatorOnEnumWihtoutFlags
            pcmdf[0] = menuItemFlags;
            switch (iItem)
            {
                case 0:
                    pbstrText[0] = Resources.MarkerRun;
                    break;
                case 1:
                    pbstrText[0] = Resources.MarkerDebug;
                    break;
                case (int)MarkerCommandValues2.mcvRightClickCommand:
                    break;
                default:
                    pcmdf[0] = 0;
                    return VSConstants.S_FALSE;
            }

            return VSConstants.S_OK;
        }

        public int ExecMarkerCommand(IVsTextMarker pMarker, int iItem)
        {
            var tester = new GTestRunner();
            TestList testList = TestPackage.GetTestList();
            tester.OnTestsUpdated += testList.UpdateTestResult;
            switch (iItem)
            {
                case (int)MarkerCommandValues2.mcvRightClickCommand:
                    IVsUIShell uiShell = (IVsUIShell)TestPackage.GetGlobalService(typeof (SVsUIShell));
                    Guid context =  GuidList.GUIDTestMarkerCmdSet;
                    POINTS[] menuPos = new POINTS[1];
                    menuPos[0].x = (short)Cursor.Position.X;
                    menuPos[0].y = (short)Cursor.Position.Y;
                    var hr = uiShell.ShowContextMenu(0, ref context, (int)PkgCmdIDList.ContextMenu, menuPos, this as IOleCommandTarget);
                    if (hr != VSConstants.S_OK)
                        return VSConstants.S_FALSE;
                    break;
                case (int)PkgCmdIDList.cmdRunTest:
                    tester.RunTests(_projectMarkerBelongsTo, _testName, false);
                    break;
                case (int)PkgCmdIDList.cmdDebugTest:
                    tester.RunTests(_projectMarkerBelongsTo, _testName, true);
                    break;
                default:

                    return VSConstants.S_FALSE;
            }
            return VSConstants.S_OK;
        }

        public void OnAfterSpanReload()
        {
        }

        public int OnAfterMarkerChange(IVsTextMarker pMarker)
        {
            return VSConstants.E_NOTIMPL;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            for (uint i = 0; i < cCmds; i++)
            {
                prgCmds[i] = new OLECMD() { cmdf = (uint) (OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED)};
            }
            return VSConstants.S_OK;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            var tester = new GTestRunner();
            TestList testList = TestPackage.GetTestList();
            tester.OnTestsUpdated += testList.UpdateTestResult;
            try
            {
                switch (nCmdID)
                {
                    case PkgCmdIDList.cmdRunTest:
                        tester.RunTests(_projectMarkerBelongsTo, _testName, false);
                        break;
                    case PkgCmdIDList.cmdDebugTest:
                        tester.RunTests(_projectMarkerBelongsTo, _testName, true);
                        break;
                }
            }
            catch (TestRunnerException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
            return VSConstants.S_OK;
        }
    }
}
