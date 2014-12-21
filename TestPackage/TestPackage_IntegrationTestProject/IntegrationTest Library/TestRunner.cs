using System;
using System.Runtime.InteropServices;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace TestPackage_IntegrationTestProject.IntegrationTest_Library
{
    internal class TestRunner : IDisposable
    {
        private DTE2 _dte;

        public TestRunner()
        {
            _dte = StartIDE();
        }

        public DTE2 DTE
        {
            get { return _dte; }
        }

        public ServiceProvider ServiceProvider
        {
            get
            {
                return new ServiceProvider((IServiceProvider)_dte);
            }
        }

        public DTE2 StartIDE()
        {

            Type t = Type.GetTypeFromProgID("VisualStudio.DTE.9.0", true);
            DTE2 dte = (DTE2)Activator.CreateInstance(t, true);
            MessageFilter.Register();
            dte.MainWindow.Activate();
            return dte;
        }

        public void ShutdownIDE()
        {
            _dte.Quit();
            MessageFilter.Revoke();
            _dte = null;
        }

        public void Dispose()
        {
            if (_dte != null)
                ShutdownIDE();
        }

        public static void Init()
        {
        }

    }

    public class MessageFilter : IOleMessageFilter
    {
        //
        // Class containing the IOleMessageFilter
        // thread error-handling functions.

        // Start the filter.
        public static void Register()
        {
            IOleMessageFilter newFilter = new MessageFilter();
            IOleMessageFilter oldFilter = null;
            CoRegisterMessageFilter(newFilter, out oldFilter);
        }

        // Done with the filter, close it.
        public static void Revoke()
        {
            IOleMessageFilter oldFilter = null;
            CoRegisterMessageFilter(null, out oldFilter);
        }

        //
        // IOleMessageFilter functions.
        // Handle incoming thread requests.
        int IOleMessageFilter.HandleInComingCall(int dwCallType,
          System.IntPtr hTaskCaller, int dwTickCount, System.IntPtr
          lpInterfaceInfo)
        {
            //Return the flag SERVERCALL_ISHANDLED.
            return 0;
        }

        // Thread call was rejected, so try again.
        int IOleMessageFilter.RetryRejectedCall(System.IntPtr
          hTaskCallee, int dwTickCount, int dwRejectType)
        {
            if (dwRejectType == 2)
            // flag = SERVERCALL_RETRYLATER.
            {
                // Retry the thread call immediately if return >=0 &
                // <100.
                return 99;
            }
            // Too busy; cancel call.
            return -1;
        }

        int IOleMessageFilter.MessagePending(IntPtr hTaskCallee,
          int dwTickCount, int dwPendingType)
        {
            //Return the flag PENDINGMSG_WAITDEFPROCESS.
            return 2;
        }

        // Implement the IOleMessageFilter interface.
        [DllImport("Ole32.dll")]
        private static extern int
          CoRegisterMessageFilter(IOleMessageFilter newFilter, out
              IOleMessageFilter oldFilter);
    }

}

