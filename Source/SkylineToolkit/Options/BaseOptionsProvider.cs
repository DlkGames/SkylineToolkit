using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkylineToolkit.Options
{
    public abstract class BaseOptionsProvider : IOptionsProvider
    {
        public BaseOptionsProvider(IModOptions modOptions)
        {
            this.ModOptions = modOptions;
        }

        public bool EnableAutoReload { get; set; }

        public IModOptions ModOptions { get; protected set; }

        public abstract void Save();

        public abstract void Reload();

        protected virtual PropertyInfo[] GetSettingProperties()
        {
            if (ModOptions == null)
            {
                throw new InvalidOperationException("No mod options set for this OptionsProvider.");
            }

            Type optionsType = ModOptions.GetType();

            PropertyInfo[] properties = optionsType.GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            return properties.Where(pi => Attribute.IsDefined(pi, typeof(SettingAttribute))).ToArray();
        }

        protected virtual FieldInfo[] GetSettingFields()
        {
            if (ModOptions == null)
            {
                throw new InvalidOperationException("No mod options set for this OptionsProvider.");
            }

            Type optionsType = ModOptions.GetType();

            FieldInfo[] fields = optionsType.GetFields(BindingFlags.GetField | BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            return fields.Where(fi => Attribute.IsDefined(fi, typeof(SettingAttribute))).ToArray();
        }

        public virtual void Dispose()
        {
            this.Save();
        }
    }
}
