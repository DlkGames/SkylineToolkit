using System.Xml.Serialization;
using UnityEngine;

namespace SkylineToolkit.UI.SkyAML
{
    [XmlRoot("Vector2")]
    public class Vector2Element
    {
        private Vector2 value = Vector2.zero;

        [XmlIgnore]
        public Vector2 Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        [XmlAttribute]
        public float X
        {
            get
            {
                return this.value.x;
            }
            set
            {
                this.value.x = value;
            }
        }

        [XmlAttribute]
        public float Y
        {
            get
            {
                return this.value.y;
            }
            set
            {
                this.value.y = value;
            }
        }
    }
}
