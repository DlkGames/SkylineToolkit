using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SkylineToolkit.UI.SkyAML
{
    public class SkyAmlElement
    {
        public string Name { get; set; }

        public IList<SkyAmlAttribute> Attributes { get; set; }

        public IList<SkyAmlElement> Children { get; set; }

        public SkyAmlElement Parent { get; set; }
    }
}
