using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    [Serializable]
    [XmlRoot("testsuites")]
    public class GTestResultCollection : IListSource
    { 
        public GTestResultCollection()
        {
            TestResults = new List<GTestSuite>();
        }
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("tests")]
        public int TotalNumberOfTests;
        [XmlAttribute("failures")]
        public int TotalNumberOfFailures;
        [XmlAttribute("errors")]
        public int TotalNumberOfErrors;
        [XmlAttribute("time")]
        public float TotalTime;
        [XmlElement("testsuite")]
        public List<GTestSuite> TestResults;

        public IList GetList()
        {
            return TestResults;
        }

        public bool ContainsListCollection
        {
            get { return (TestResults != null); }
        }
        public GTestSuite this[int i]
        {
            get { return TestResults[i]; }
            set{ TestResults[i] = value; }
        }
    }
}
