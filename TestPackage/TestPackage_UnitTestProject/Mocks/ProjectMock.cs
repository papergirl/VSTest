using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;

namespace TestPackage_UnitTestProject.Mocks
{
    class ProjectMock : Project
    {
        public void SaveAs(string NewFileName)
        {
            
        }

        public void Save(string FileName)
        {
        }

        public void Delete()
        {
        }

        public string Name { get; set; }
        public string FileName { get; private set; }
        public bool IsDirty { get; set; }
        public Projects Collection { get; private set; }
        public DTE DTE 
        { 
            get
            {
                return (DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.10.0");
            }
        }
        public string Kind { get; private set; }
        public ProjectItems ProjectItems { get; private set; }
        public EnvDTE.Properties Properties { get; private set; }
        public string UniqueName { get; private set; }
        public object Object { get; private set; }
        public object get_Extender(string ExtenderName)
        {
            return null;
        }

        public object ExtenderNames { get; private set; }
        public string ExtenderCATID { get; private set; }
        public string FullName { get; private set; }
        public bool Saved { get; set; }
        public ConfigurationManager ConfigurationManager { get; private set; }
        public Globals Globals { get; private set; }
        public ProjectItem ParentProjectItem { get; private set; }
        public CodeModel CodeModel { get; private set; }
    }
}
