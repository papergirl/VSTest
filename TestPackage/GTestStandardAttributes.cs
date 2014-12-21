using System.Xml.Serialization;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    public class GTestStandardAttributes
    {
        [XmlAttribute("time")]
        public float Time { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("disabled")]
        public int Disabled { get; set; }
    }
}
