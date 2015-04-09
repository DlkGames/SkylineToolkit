using SkylineToolkit.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.Options.Serializers
{
    public sealed class Vector2XmlSerializer : ISettingSerializer<Vector2, XElement>
    {
        public XElement Serialize(IOptionsProvider provider, Vector2 setting)
        {
            return new XElement("Vector2",
                new XAttribute("X", setting.x),
                new XAttribute("Y", setting.y));
        }

        public Vector2 Deserialize(IOptionsProvider provider, XElement value)
        {
            float x, y;

            try
            {
                x = Single.Parse(value.Attribute("X").Value);
                y = Single.Parse(value.Attribute("Y").Value);
            }
            catch (NullReferenceException ex)
            {
                string message = "The given XElement is not a serialized Vector2.";
                Log.Error(message);

                throw new InvalidSerializedValueException(message, ex);
            }
            catch (FormatException ex)
            {
                string message = "Error parsing the value of the serialized Vector2 setting.";

                Log.Error(message);

                throw new InvalidSerializedValueException(message, ex);
            }

            return new Vector2(x, y);
        }
    }
}
