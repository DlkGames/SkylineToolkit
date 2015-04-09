using System.Collections.Generic;

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
