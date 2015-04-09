using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public class InvalidSerializedValueException : Exception
    {
        public string Setting { get; set; }

        public object Value { get; set; }

        public InvalidSerializedValueException(string setting, object value)
        {
            this.Setting = setting;

            this.Value = value;
        }
    }
}
