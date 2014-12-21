using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    [Serializable]
    public class GTestResult : GTestStandardAttributes
    {
        public GTestResult()
        {
            Errors = new List<GTestFailure>();
        }
        private string _status = "";
        private string _suiteName = "";

        [XmlElement("failure")]
        public List<GTestFailure> Errors;


        [XmlAttribute("status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
 
        [XmlAttribute("classname")]
        public string SuiteName
        {
            get { return _suiteName; }
            set { _suiteName = value; }
        }

        [XmlIgnore]
        public bool TestRan
        {
            get { return _status == "run"; }
            set { _status = value ? "run" : "notrun"; }
        }

        [XmlIgnore]
        public string Failures
        {
            get
            {
                StringBuilder fails = new StringBuilder("");
                foreach(GTestFailure failure in Errors)
                {
                    Contract.Assert(failure != null);
                    fails.Append(failure.Message);
                }
                return fails.ToString();
            }
        }
        [XmlIgnore]
        public string FailuresSingleLine
        {
            get { return Failures.Replace('\n', ' '); }
        }
        public bool HasPassed()
        {
            return Errors.Count == 0;
        }
        [XmlIgnore]
        public Image GetRelatedImage
        {
            get {
                if (!TestRan)
                    return Resources.TestNotRun;

                return HasPassed() ? Resources.TestPassed : Resources.TestFail;
             }
        }
        [XmlIgnore]
        public string Fullname
        {
            get { return _suiteName + "." + Name; }
        }
    }
    public class GTestFailure
    {
        [XmlAttribute("message")] 
        public string Message;
    }
}
