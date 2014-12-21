using System.Diagnostics.Contracts;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    [ContractClass(typeof(TestListener))]
    public interface ITestListener
    {
        void ListTests(ConfiguredProject project);
        void RunTests(ConfiguredProject project,string filterString,bool debugTest);
    }

    [ContractClassFor(typeof(ITestListener))]
    internal abstract class TestListener : ITestListener
    {
        public void ListTests(ConfiguredProject project)
        {
            Contract.Requires(project != null);
        }
        public void RunTests(ConfiguredProject project, string filterString, bool debugTest)
        {
            Contract.Requires(project != null);
            Contract.Requires(filterString != null);
        }
    }
}
    