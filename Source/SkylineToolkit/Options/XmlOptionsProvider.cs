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

        public override void Reload()
        {
            LoadFrom(SettingsFile);
        }

        public override void Save()
        {
            SaveTo(SettingsFile);
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

        public virtual void SaveTo(Stream stream)
        {
            XDocument document = new XDocument(new XElement("ModOptions"));
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            XElement root = document.Root;

            PropertyInfo[] settingProperties = GetSettingProperties();

            AppendSettings(root, settingProperties);

            FieldInfo[] settingFields = GetSettingFields();

            AppendSettings(root, settingFields);

            document.Save(writer);
            stream.Flush();
        }

        protected virtual void Load()
        {
            LoadFrom(SettingsFile);
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

            using (FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                LoadFrom(stream);
            }
        }

        public virtual void LoadFrom(Stream stream)
        {
            XDocument document;

            try
            {
                document = XDocument.Load(stream);
            }
            catch(XmlException)
            {
                Log.Warning("ModOptions", "Corrupt settings file for mod {0}. Loading defaults...", this.ModOptions.Mod.Name);
                
                //File.Delete(SettingsFile);

                return;
            }

            XElement root = document.Root;

            PropertyInfo[] settingProperties = GetSettingProperties();

            LoadSettings(root, settingProperties);

            FieldInfo[] settingFields = GetSettingFields();

            LoadSettings(root, settingFields);
        }

        private void AppendSettings(XElement parent, MemberInfo[] settingMembers)
        {
            foreach (MemberInfo info in settingMembers)
            {
                Type settingType = info is PropertyInfo ? ((PropertyInfo)info).PropertyType : ((FieldInfo)info).FieldType;
                SettingAttribute settingAttribute = (SettingAttribute)info.GetCustomAttributes(typeof(SettingAttribute), true).First();
                string settingName = String.IsNullOrEmpty(settingAttribute.Name) ? info.Name : settingAttribute.Name;

                try
                {
                    object value;

                    if (info is PropertyInfo)
                    {
                        value = ((PropertyInfo)info).GetValue(this.ModOptions, null);
                    }
                    else
                    {
                        value = ((FieldInfo)info).GetValue(this.ModOptions);
                    }

                    XElement element;

                    if (settingAttribute.Serializer == null)
                    {

                        if (settingType.IsPrimitive)
                        {
                            element = new XElement(settingName,
                                new XAttribute("Value", value.ToString()));
                        }
                        else
                        {
                            if (!TrySerializeIntegratedType(settingType, value, out element))
                            {
                                element = new XElement(settingName);

                                element.Value = value.ToString();
                            }
                        }
                    }
                    else
                    {
                        Type serializerType = settingAttribute.Serializer;

                        if (settingAttribute.Serializer.IsGenericType)
                        {
                            serializerType = settingAttribute.Serializer.MakeGenericType(settingType, typeof(XElement));
                        }

                        MethodInfo serializeMethod = serializerType.GetMethod("Serialize");

                        object serializer = Activator.CreateInstance(serializerType);

                        object result = serializeMethod.Invoke(serializer, new object[] { this, value });

                        if (result is XElement)
                        {
                            element = (XElement)result;

                            element.Name = settingName;
                        }
                        else
                        {
                            element = new XElement(settingName, new XAttribute("Serializer", "NonXml"));

                            element.Value = result.ToString();
                        }
                    }

                    parent.Add(element);
                }
                catch (Exception ex)
                {
                    Log.Error("ModOptions", "Error saving the value for the setting {0} of mod {1}. See the following exception for more information.", settingName, this.ModOptions.Mod.Name);
                    Log.Exception("ModOptions", ex);
                }
            }
        }
        
        private void LoadSettings(XElement container, MemberInfo[] settingMembers)
        {
            foreach (MemberInfo info in settingMembers)
            {
                Type settingType = info is PropertyInfo ? ((PropertyInfo)info).PropertyType : ((FieldInfo)info).FieldType;
                SettingAttribute settingAttribute = (SettingAttribute)info.GetCustomAttributes(typeof(SettingAttribute), true).First();
                string settingName = String.IsNullOrEmpty(settingAttribute.Name) ? info.Name : settingAttribute.Name;

                try
                {
                    XElement element = container.Element(settingName);

                    if (element == null)
                        continue;

                    object loadedValue;

                    if (settingAttribute.Serializer == null)
                    {

                        if (settingType.IsPrimitive)
                        {
                            XAttribute valueAttribute = element.Attribute("Value");

                            if (valueAttribute == null)
                                continue;

                            loadedValue = Convert.ChangeType(valueAttribute.Value, settingType);
                        }
                        else
                        {
                            if (!TryDeserializeIntegratedType(settingType, element, out loadedValue))
                                continue;
                        }
                    }
                    else
                    {
                        Type serializerType = settingAttribute.Serializer;

                        if (settingAttribute.Serializer.IsGenericType)
                        {
                            serializerType = settingAttribute.Serializer.MakeGenericType(settingType, typeof(XElement));
                        }

                        MethodInfo deserializeMethod = serializerType.GetMethod("Deserialize");

                        object serializer = Activator.CreateInstance(serializerType);

                        if (element.Attribute("Serializer") != null && element.Attribute("Serializer").Value == "NonXml")
                        {
                            loadedValue = deserializeMethod.Invoke(serializer, new object[] { this, element.Value });
                        }
                        else
                        {
                            loadedValue = deserializeMethod.Invoke(serializer, new object[] { this, element });
                        }
                    }

                    if (info is PropertyInfo)
                    {
                        ((PropertyInfo)info).SetValue(this.ModOptions, loadedValue, null);
                    }
                    else
                    {
                        ((FieldInfo)info).SetValue(this.ModOptions, loadedValue);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("ModOptions", "Error loading the saved value for the setting {0} of mod {1}. See the following exception for more information.", settingName, this.ModOptions.Mod.Name);
                    Log.Exception("ModOptions", ex);
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
    }
}
