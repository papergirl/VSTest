using System;
using System.Collections;
using System.IO;
using EnvDTE;

namespace TestPackage_UnitTestProject.MenuItemTests
{
    class SolutionMock : Solution
    {

        public string get_TemplatePath(string ProjectType)
        {
            throw new NotImplementedException();
        }

        public string FullName
        {
            get { return Directory.GetCurrentDirectory() + "\\mocksolution.sln"; }
        }

        public bool Saved { get; set; }
        public Globals Globals { get; private set; }
        public AddIns AddIns { get; private set; }
        public object get_Extender(string ExtenderName)
        {
            throw new NotImplementedException();
        }

        public object ExtenderNames { get; private set; }
        public string ExtenderCATID { get; private set; }
        public bool IsOpen { get; private set; }
        public SolutionBuild SolutionBuild { get; private set; }
        public Projects Projects { get; private set; }

        #region Implementation of IEnumerable

        public Project Item(object index)
        {
            throw new NotImplementedException();
        }

        IEnumerator _Solution.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void SaveAs(string FileName)
        {
            throw new NotImplementedException();
        }

        public Project AddFromTemplate(string FileName, string Destination, string ProjectName, bool Exclusive)
        {
            throw new NotImplementedException();
        }

        public Project AddFromFile(string FileName, bool Exclusive)
        {
            throw new NotImplementedException();
        }

        public void Open(string FileName)
        {
            throw new NotImplementedException();
        }

        public void Close(bool SaveFirst)
        {
            throw new NotImplementedException();
        }

        public void Remove(Project proj)
        {
            throw new NotImplementedException();
        }

        public void Create(string Destination, string Name)
        {
            throw new NotImplementedException();
        }

        public ProjectItem FindProjectItem(string FileName)
        {
            throw new NotImplementedException();
        }

        public string ProjectItemsTemplatePath(string ProjectKind)
        {
            throw new NotImplementedException();
        }

        public DTE DTE { get; private set; }
        public DTE Parent { get; private set; }
        public int Count { get; private set; }
        public string FileName { get; private set; }
        public EnvDTE.Properties Properties { get; private set; }
        public bool IsDirty { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
