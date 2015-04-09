using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public class InvalidSerializedValueException : Exception
    {
        public string Setting { get; set; }

        public object Value { get; protected set; }

        public InvalidSerializedValueException()
        {
        }

        public InvalidSerializedValueException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InvalidSerializedValueException(string setting, object value, Exception inner = null)
            : base("Unable to parse serialized setting " + setting, inner)
        {
            this.Setting = setting;

            this.Value = value;
        }
    }
}
