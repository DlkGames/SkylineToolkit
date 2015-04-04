using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

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
