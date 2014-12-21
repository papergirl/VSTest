using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    public class GTestSuite : GTestStandardAttributes, IListSource
    {
        public GTestSuite()
        {
            Results = new List<GTestResult>();
        }
        [XmlAttribute("tests")]
        public int NumberOfTests;
        [XmlAttribute("failures")]
        public int NumberOfFailures;
        [XmlAttribute("errors")]
        public int NumberOfErrors;
        [XmlElement("testcase")]
        public List<GTestResult> Results;

        public IList GetList()
        {
            return Results;
        }

        public bool ContainsListCollection
        {
            get { return (Results != null) ; }
        }
    }   
}
