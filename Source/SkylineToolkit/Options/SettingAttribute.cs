using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SettingAttribute : Attribute
    {
        public SettingAttribute()
        {

        }

        public SettingAttribute(string key)
        {
            this.Key = key;
        }

        public SettingAttribute(string key, Type serializer)
            : this(key)
        {
            this.Serializer = serializer;
        }

        public string Key { get; set; }

        public Type Serializer { get; set; }
    }
}
