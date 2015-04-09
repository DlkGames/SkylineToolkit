using SkylineToolkit.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.Options.Serializers
{
    public sealed class Vector3XmlSerializer : ISettingSerializer<Vector3, XElement>
    {
        public XElement Serialize(IOptionsProvider provider, Vector3 setting)
        {
            return new XElement("Vector3",
                new XAttribute("X", setting.x),
                new XAttribute("Y", setting.y),
                new XAttribute("Z", setting.z));
        }

        public Vector3 Deserialize(IOptionsProvider provider, XElement value)
        {
            float x, y, z;

            try
            {
                x = Single.Parse(value.Attribute("X").Value);
                y = Single.Parse(value.Attribute("Y").Value);
                z = Single.Parse(value.Attribute("Z").Value);
            }
            catch (NullReferenceException ex)
            {
                string message = "The given XElement is not a serialized Vector3.";
                Log.Error(message);

                throw new InvalidSerializedValueException(message, ex);
            }
            catch (FormatException ex)
            {
                string message = "Error parsing the value of the serialized Vector3 setting.";

                Log.Error(message);

                throw new InvalidSerializedValueException(message, ex);
            }

            return new Vector3(x, y, z);
        }
    }
}
