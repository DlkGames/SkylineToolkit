using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkylineToolkit
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type type)
        {
            if (type == null || !type.IsValueType || type == typeof(void))
            {
                return null;
            }

            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Could not create a default instance of the supplied value type.", ex);
                }
            }

            return null;
        }

        public static bool IsDefaultValue(this Type type, object value)
        {
            if (type == null)
            {
                if (value == null)
                {
                    MethodBase currmethod = MethodInfo.GetCurrentMethod();

                    throw new ArgumentNullException("Cannot determine the Type from a null Value");
                }

                type = value.GetType();
            }

            object Default = type.GetDefaultValue();

            if (value != null)
            {
                return value.Equals(Default);
            }

            return Default == null;
        }

        public static T GetDefault<T>()
        {
            return (T)GetDefaultValue(typeof(T));
        }
    }
}
