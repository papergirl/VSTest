using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using Microsoft.Win32;
using Shell32;
using System.Diagnostics; 

namespace DevenvSetupCustomAction
{
    [RunInstaller(true)]
    public partial class DevenvSetup : Installer
    {
        public DevenvSetup()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            if (Context.Parameters["vs2008"].ToLower() == "1")
                using (RegistryKey setupKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\9.0\Setup\VS"))
                {
                    if (setupKey != null)
                    {
                        string devenv = setupKey.GetValue("EnvironmentPath").ToString();
                        if (!string.IsNullOrEmpty(devenv))
                        {
                            Process.Start(devenv, "/setup /nosetupvstemplates").WaitForExit();
                        }
                    }
                }
        }
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            
        }

    }---------------------------
Microsoft Visual Studio
---------------------------
Class not registered.
Looking for object with CLSID: {8E7B96A8-E33D-11D0-A6D5-00C04FB67F6A}.
---------------------------
OK   
---------------------------

}
