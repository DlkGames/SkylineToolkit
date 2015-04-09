using SkylineToolkit.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace SkylineToolkit.Options.Serializers
{
    public sealed class RectXmlSerializer : ISettingSerializer<Rect, XElement>
    {
        public XElement Serialize(IOptionsProvider provider, Rect setting)
        {
            return new XElement("Rect",
                new XAttribute("X", setting.x),
                new XAttribute("Y", setting.y),
                new XAttribute("Width", setting.width),
                new XAttribute("Height", setting.height));
        }

        public Rect Deserialize(IOptionsProvider provider, XElement value)
        {
            float x, y, width, height;

            try
            {
                x = Single.Parse(value.Attribute("X").Value);
                y = Single.Parse(value.Attribute("Y").Value);
                width = Single.Parse(value.Attribute("Width").Value);
                height = Single.Parse(value.Attribute("Height").Value);
            }
            catch (NullReferenceException ex)
            {
                string message = "The given XElement is not a serialized Rect.";
                Log.Error(message);

                throw new InvalidSerializedValueException(message, ex);
            }
            catch(FormatException ex)
            {
                string message = "Error parsing the value of the serialized Rect setting.";

                Log.Error(message);

                throw new InvalidSerializedValueException(message, ex);
            }

            return new Rect(x, y, width, height);
        }
    }
}
