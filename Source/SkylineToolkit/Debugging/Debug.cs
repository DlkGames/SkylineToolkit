using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.Debugging
{
    public static class Debug
    {
        public static string DumpGameObjectRecursive(GameObject obj, int depth = 0)
        {
            string indention = GetIndention(depth);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine(String.Format("{0}{1} ({2})", indention, obj.name, DumpComponents(obj)));

            foreach (Transform child in obj.transform)
            {
                builder.Append(DumpGameObjectRecursive(child.gameObject, depth + 1));
            }

            return builder.ToString();
        }

        public static string DumpGameObject(GameObject obj)
        {
            return String.Format("{0} ({1})", obj.name, DumpComponents(obj));
        }

        public static string DumpComponents(GameObject obj)
        {
            StringBuilder result = new StringBuilder();

            Component[] components = obj.GetComponents<Component>();
            
            for (int i = 0; i < components.Length; i++)
            {
                result.Append(components[i].GetType().ToString());

                if (i < components.Length - 1)
                {
                    result.Append(", ");
                }
            }

            return result.ToString();
        }

        public static string DumpSceneHierarchy()
        {
            StringBuilder builder = new StringBuilder(Environment.NewLine);

            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            
            foreach (GameObject obj in objects)
            {
                if (obj.transform.parent == null)
                {
                    builder.AppendLine(DumpGameObjectRecursive(obj, 0));

                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        private static string GetIndention(int depth)
        {
            string indention = string.Empty;

            for (int i = 0; i < depth; i++)
            {
                indention += "    ";
            }

            return indention;
        }

        public static string DumpFields(object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            StringBuilder builder = new StringBuilder("(Fields: " + fields.Length + ")");

            foreach (FieldInfo field in fields)
            {
                builder.AppendLine(String.Format("{0}.{1} = {2}", obj, field.Name, field.GetValue(obj)));
            }

            fields = obj.GetType().GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            builder.AppendLine("(Static fields: " + fields.Length + ")");

            foreach (FieldInfo field in fields)
            {
                builder.AppendLine(String.Format("{0}.{1} = {2}", obj, field.Name, field.GetValue(obj)));
            }

            return builder.ToString();
        }

        public static object Evaluate(string code)
        {
            CodeRunner runner = new CodeRunner();

            object result = runner.RunCode(code);

            return result;
        }
    }
}
