using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SettingAttribute : Attribute
    {
        private Type serializer = null;

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

        public Type Serializer
        {
            get
            {
                return this.serializer;
            }
            set
            {
                if (!value.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ISettingSerializer<,>)))
                {
                    Log.Error("The specified serializer {0} is not a setting serializer.", value.Name);
                    throw new NotSupportedException(String.Format("The specified serializer {0} is not a setting serializer.", value.Name));
                }

                this.serializer = value;
            }
        }
    }
}
