using SkylineToolkit.Options.Serializers;
using SkylineToolkit.Xml.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SkylineToolkit.Options
{
    public class XmlOptionsProvider : BaseOptionsProvider, IFileOptionsProvider
    {
        private static Vector2XmlSerializer vector2Serializer = new Vector2XmlSerializer();
        private static Vector3XmlSerializer vector3Serializer = new Vector3XmlSerializer();
        private static RectXmlSerializer rectSerializer = new RectXmlSerializer();

        public XmlOptionsProvider(ModOptions modOptions)
            : base(modOptions)
        {
            SetDefaultSettingsFile();

            if (File.Exists(SettingsFile))
            {
                Load();
            }
        }

        public XmlOptionsProvider(ModOptions modOptions, string file)
            : base(modOptions)
        {
            SettingsFile = file;

            Load();
        }

        public string SettingsFile
        {
            get;
            set;
        }

        private void SetDefaultSettingsFile()
        {
            string path = Path.Combine(SkylinePath.ModOptions, ModOptions.Mod.ModName + ".xml");

            if (!Directory.Exists(SkylinePath.ModOptions))
            {
                Directory.CreateDirectory(SkylinePath.ModOptions);
            }

            SettingsFile = path;
        }

        public override void Save()
        {
            SaveTo(SettingsFile);
        }

        public override void Reload()
        {
            LoadFrom(SettingsFile);
        }

        public virtual void SaveTo(Stream stream)
        {
            XDocument document = new XDocument(new XElement("ModOptions"));
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            XElement root = document.Root;

            PropertyInfo[] settingProperties = GetSettingProperties();

            AppendSettingProperties(root, settingProperties);

            FieldInfo[] settingFields = GetSettingFields();

            AppendSettingFields(root, settingFields);

            document.Save(writer);
            stream.Flush();
        }

        private void AppendSettingProperties(XElement parent, PropertyInfo[] settingProperties)
        {
            foreach (PropertyInfo info in settingProperties)
            {
                SettingAttribute settingAttribute = (SettingAttribute)info.GetCustomAttributes(typeof(SettingAttribute), true).First();
                object value = info.GetValue(this.ModOptions, null);

                if (settingAttribute.Serializer == null)
                {
                    XElement element;

                    if (info.PropertyType.IsPrimitive)
                    {
                        element = new XElement(info.Name,
                            new XAttribute("Value", value.ToString()));
                    }
                    else
                    {
                        if (!TrySerializeIntegratedType(info.PropertyType, value, out element))
                        {
                            element = new XElement(info.Name);

                            element.Value = value.ToString();
                        }
                    }

                    parent.Add(element);
                }
                else
                {
                    Type serializerType = settingAttribute.Serializer;

                    if (settingAttribute.Serializer.IsGenericType)
                    {
                        serializerType = settingAttribute.Serializer.MakeGenericType(info.PropertyType, typeof(XElement));
                    }

                    MethodInfo serializeMethod = serializerType.GetMethod("Serialize");

                    object serializer = Activator.CreateInstance(serializerType);

                    object result = serializeMethod.Invoke(serializer, new object[] { this, value });

                    XElement element;

                    if (result is XElement)
                    {
                        element = (XElement)result;

                        element.Name = info.Name;
                    }
                    else
                    {
                        element = new XElement(info.Name);

                        element.Value = result.ToString();
                    }

                    parent.Add(element);
                }
            }
        }

        private void AppendSettingFields(XElement parent, FieldInfo[] settingFields)
        {
            foreach (FieldInfo info in settingFields)
            {
                SettingAttribute settingAttribute = (SettingAttribute)info.GetCustomAttributes(typeof(SettingAttribute), true).First();
                object value = info.GetValue(this.ModOptions);

                if (settingAttribute.Serializer == null)
                {
                    XElement element;

                    if (info.FieldType.IsPrimitive)
                    {
                        element = new XElement(info.Name,
                            new XAttribute("Value", value.ToString()));
                    }
                    else
                    {
                        if (!TrySerializeIntegratedType(info.FieldType, value, out element))
                        {
                            element = new XElement(info.Name);

                            element.Value = value.ToString();
                        }
                    }

                    parent.Add(element);
                }
                else
                {
                    Type serializerType = settingAttribute.Serializer;

                    if (settingAttribute.Serializer.IsGenericType)
                    {
                        serializerType = settingAttribute.Serializer.MakeGenericType(info.FieldType, typeof(XElement));
                    }

                    MethodInfo serializeMethod = serializerType.GetMethod("Serialize");

                    object serializer = Activator.CreateInstance(serializerType);

                    object result = serializeMethod.Invoke(serializer, new object[] { this, value });

                    XElement element;

                    if (result is XElement)
                    {
                        element = (XElement)result;

                        element.Name = info.Name;
                    }
                    else
                    {
                        element = new XElement(info.Name);

                        element.Value = result.ToString();
                    }

                    parent.Add(element);
                }
            }
        }

        private bool TrySerializeIntegratedType(Type type, object value, out XElement element)
        {
            if (type == typeof(UnityEngine.Vector2))
            {
                element = vector2Serializer.Serialize(this, (UnityEngine.Vector2)value);
                return true;
            }
            else if (type == typeof(UnityEngine.Vector3))
            {
                element = vector3Serializer.Serialize(this, (UnityEngine.Vector3)value);
                return true;
            }
            else if (type == typeof(UnityEngine.Rect))
            {
                element = rectSerializer.Serialize(this, (UnityEngine.Rect)value);
                return true;
            }

            element = null;

            return false;
        }

        private bool TryDeserializeIntegratedType(Type type, XElement element, out object value)
        {
            if (type == typeof(UnityEngine.Vector2))
            {
                value = vector2Serializer.Deserialize(this, element);
                return true;
            }
            else if (type == typeof(UnityEngine.Vector3))
            {
                value = vector3Serializer.Deserialize(this, element);
                return true;
            }
            else if (type == typeof(UnityEngine.Rect))
            {
                value = rectSerializer.Deserialize(this, element);
                return true;
            }

            value = null;

            return false;
        }

        public virtual void SaveTo(string file)
        {
            FileInfo info = new FileInfo(file);

            if (!info.Directory.Exists)
            {
                Directory.CreateDirectory(info.DirectoryName);
            }

            using (FileStream stream = File.Open(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SaveTo(stream);
            }
        }

        public virtual void LoadFrom(string file)
        {
            if (String.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException("file");
            }

            if (!File.Exists(file))
            {
                throw new FileNotFoundException("Could not find settings file");
            }
        }

        protected virtual void Load()
        {
            LoadFrom(SettingsFile);
        }
    }
}
