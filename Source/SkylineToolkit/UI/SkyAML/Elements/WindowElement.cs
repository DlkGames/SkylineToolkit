using System.Collections.Generic;
using System.Xml.Serialization;

namespace SkylineToolkit.UI.SkyAML.Elements
{
    [XmlRoot("Window")]
    public class WindowElement : ContainerElement
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlArray("Window.Properties")]
        public IList<SkyAmlElement> Properties { get; set; }
    }
}
