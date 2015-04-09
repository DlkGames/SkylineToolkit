using System.Xml.Serialization;
using UnityEngine;

namespace SkylineToolkit.UI.SkyAML
{
    [XmlRoot("Vector3")]
    public class Vector3Element
    {
        private Vector3 value = Vector3.zero;

        [XmlIgnore]
        public Vector3 Value
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

        [XmlAttribute]
        public float Z
        {
            get
            {
                return this.value.z;
            }
            set
            {
                this.value.z = value;
            }
        }
    }
}
