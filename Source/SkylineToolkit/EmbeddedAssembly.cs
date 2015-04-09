using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SkylineToolkit
{
    public static class EmbeddedAssembly
    {
        private static Dictionary<string, Assembly> assemblies = null;

        private static bool resolverRegistered = false;

        internal static void RegisterResolver()
        {
            if (resolverRegistered)
            {
                return;
            }

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            resolverRegistered = true;

            Log.Debug("Registered assemlbly resolver.");
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }

        internal static void Load(string assemblyName)
        {
            Load(assemblyName, typeof(EmbeddedAssembly).Assembly, "SkylineToolkit.Resources");
        }

        public static void Load(string assemblyName, Assembly containingAssembly, string containingNamespace)
        {
            Log.Debug("Loading assemlby " + assemblyName);

            if (assemblies == null)
            {
                assemblies = new Dictionary<string, Assembly>();
            }

            string resource = containingNamespace + "." + assemblyName;

            byte[] data = null;
            Assembly loadedAssembly;

            using (Stream stream = containingAssembly.GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    Log.Error("Assemlbly " + assemblyName + " not found in embedded resources.");
                    return;
                }

                data = new byte[(int)stream.Length];
                stream.Read(data, 0, (int)stream.Length);

                try
                {
                    loadedAssembly = Assembly.Load(data);

                    assemblies.Add(loadedAssembly.FullName, loadedAssembly);

                    Log.Debug("Assemlbly " + loadedAssembly.FullName + " laoded");

                    return;
                }
                catch
                {
                    Log.Debug("Assemlbly cannot be loaded from byte[], trying to load unmanaged library.");
                }
            }

            bool fileOk = false;
            string tempFile = "";

            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                string fileHash = BitConverter.ToString(sha1.ComputeHash(data)).Replace("-", string.Empty); ;

                tempFile = Path.Combine(Path.GetTempPath(), assemblyName);

                if (File.Exists(tempFile))
                {
                    byte[] data2 = File.ReadAllBytes(tempFile);
                    string fileHash2 = BitConverter.ToString(sha1.ComputeHash(data2)).Replace("-", string.Empty);

                    if (fileHash == fileHash2)
                    {
                        fileOk = true;
                    }
                    else
                    {
                        fileOk = false;
                    }
                }
                else
                {
                    fileOk = false;
                }
            }

            if (!fileOk)
            {
                System.IO.File.WriteAllBytes(tempFile, data);
            }

            loadedAssembly = Assembly.LoadFile(tempFile);

            assemblies.Add(loadedAssembly.FullName, loadedAssembly);

            Log.Debug("Assemlbly " + loadedAssembly.FullName + " laoded");
        }

        internal static Assembly Get(string assemblyFullName)
        {
            Log.Debug("Resolving assemlbly " + assemblyFullName);

            if (assemblies == null || assemblies.Count == 0)
            {
                return null;
            }

            if (assemblies.ContainsKey(assemblyFullName))
            {
                Log.Debug("Found assembly " + assemblyFullName);

                return assemblies[assemblyFullName];
            }

            Log.Debug("Assemlbly " + assemblyFullName + " unknown");

            return null;
        }
    }
}
