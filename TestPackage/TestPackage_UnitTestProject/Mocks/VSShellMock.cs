using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPackage_UnitTestProject.Mocks
{
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VsSDK.UnitTestLibrary;

    internal static class VSShellMock
    {
        private static GenericMockFactory uiShellFactory;

        internal static BaseMock GetVsShellInstance()
        {
            if (uiShellFactory == null)
            {
                uiShellFactory = new GenericMockFactory("VsShell", new[] { typeof(IVsShell)});
            }
            BaseMock uiShell = uiShellFactory.GetInstance();
            return uiShell;
        }

        internal static BaseMock GetVsShellInstance0()
        {
            IVsShell shell;
            var vsshell = GetVsShellInstance();
            vsshell.AddMethodCallback("AdviseShellPropertyChanges",AdviseShellPropertyChangesCallback);
            return vsshell;
        }

        private static void AdviseShellPropertyChangesCallback(object sender, CallbackArgs e)
        {
            e.ReturnValue = VSConstants.S_OK;
        }
    }
}
