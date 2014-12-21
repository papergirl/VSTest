using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.VCProjectEngine;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    public partial class GSettings : Form
    {
        private readonly ConfiguredProject ConfiguredProject;
        public GSettings(ConfiguredProject project)
        {
            Contract.Requires(project != null);
            InitializeComponent();
            ConfiguredProject = project;

            if (ConfiguredProject.Saved)
            {
                chkDefaults.Checked = false;
                txtListTests.Enabled = true;
                txtRunTests.Enabled = true;
                txtFile.Enabled = true;
                cmdGetExe.Enabled = true;
            }

            txtListTests.Text = ConfiguredProject.ListTestCommand ?? "";
            txtRunTests.Text = ConfiguredProject.RunTestCommand ?? "";
            txtFile.Text = ConfiguredProject.TestExe ?? "";
        }


        private void CmdCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void CmdSetClick1(object sender, EventArgs e)
        {
            ConfiguredProject.Dirty = true;
            ConfiguredProject.RunTestCommand = txtRunTests.Text;
            ConfiguredProject.ListTestCommand = txtListTests.Text;
            ConfiguredProject.TestExe = txtFile.Text;
            Close();
        }

        private void CmdGetExeClick(object sender, EventArgs e)
        {
            if (dlgTestExe.ShowDialog() == DialogResult.OK)
                txtFile.Text = dlgTestExe.FileName;
        }
        public ConfiguredProject Configuration
        {
            get { return ConfiguredProject; }
        }

        private void DefaultsChanged(object sender, EventArgs e)
        {
            // restore defaults
            if (chkDefaults.Checked)
            {
                if(MessageBox.Show(Resources.GSettings_DefaultsChanged_Text,
                                Resources.GSettings_DefaultsChanged_Title, MessageBoxButtons.YesNo) ==DialogResult.No)
                    return;
                ConfiguredProject.RunTestCommand = txtRunTests.Text = Resources.DefaultGTestRun;
                ConfiguredProject.ListTestCommand = txtListTests.Text = Resources.DefaultGTestList;
                //urk 
                DTE2 dte = (DTE2)Package.GetGlobalService((typeof(DTE)));
                foreach(Project p in (Array)dte.ActiveSolutionProjects)
                    if (ConfiguredProject.Name == p.Name && p.Kind == GuidList.CPPProject)
                     {
                          VCProject cppProject = (VCProject) p.Object;
                        VCConfiguration vcC =
                        (VCConfiguration)
                        (((IVCCollection) cppProject.Configurations).Item(p.ConfigurationManager.ActiveConfiguration.ConfigurationName));
                        ConfiguredProject.TestExe = txtFile.Text = vcC.PrimaryOutput;
                         break;
                     }
                ConfiguredProject.Saved = false;
            }
            else
                ConfiguredProject.Saved = true;
            txtListTests.Enabled = !chkDefaults.Checked;
            txtRunTests.Enabled = !chkDefaults.Checked;
            txtFile.Enabled = !chkDefaults.Checked;
            cmdGetExe.Enabled = !chkDefaults.Checked;
            
        }
    }
}
