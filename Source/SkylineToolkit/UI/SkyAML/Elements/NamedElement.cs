using System.Xml.Serialization;

namespace SkylineToolkit.UI.SkyAML.Elements
{
    [XmlRoot("UnknownElement")]
    public abstract class NamedElement : SkyAmlElement
    {
        private string name;

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
