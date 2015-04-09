using ColossalFramework.IO;
using System;
using System.IO;

namespace SkylineToolkit
{
    public static class SkylinePath
    {
        public const string MOD_OPTIONS_DIRNAME = "ModOptions";

        public static string CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        public static string Application
        {
            get
            {
                return DataLocation.applicationBase;
            }
        }

        public static string LocalAppData
        {
            get
            {
                return DataLocation.localApplicationData;
            }
        }

        /// <summary>
        /// Don't use on OSX !
        /// </summary>
        public static string AppData
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);    
                string gamePath = Path.Combine(path, Path.Combine(DataLocation.companyName, DataLocation.productName));

                if (!Directory.Exists(gamePath))
                {
                    Directory.CreateDirectory(gamePath);
                }

                return gamePath;
            }
        }

        /// <summary>
        /// Don't use on OSX !
        /// </summary>
        public static string Documents
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string gamePath = Path.Combine(path, Path.Combine(DataLocation.companyName, DataLocation.productName));

                if (!Directory.Exists(gamePath))
                {
                    Directory.CreateDirectory(gamePath);
                }

                return gamePath;
            }
        }

        public static string GameContent
        {
            get
            {
                return DataLocation.gameContentPath;
            }
        }

        public static string Addons
        {
            get
            {
                return DataLocation.addonsPath;
            }
        }

        public static string Mods
        {
            get
            {
                return DataLocation.modsPath;
            }
        }

        public static string Assets
        {
            get
            {
                return DataLocation.assetsPath;
            }
        }

        public static string Saves
        {
            get
            {
                return DataLocation.saveLocation;
            }
        }

        public static string Maps
        {
            get
            {
                return DataLocation.mapLocation;
            }
        }

        public static string ModOptions
        {
            get
            {
                string path = Path.Combine(LocalAppData, MOD_OPTIONS_DIRNAME);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        public static string Temp
        {
            get
            {
                return DataLocation.tempFolder;
            }
        }

        public static void PrintCurrentPaths()
        {
            Log.Info("Paths", "Current Directory: {0}", CurrentDirectory);
            Log.Info("Paths", "Application: ", Application);
            Log.Info("Paths", "LocalAppData: ", LocalAppData);
            Log.Info("Paths", "AppData: ", AppData);
            Log.Info("Paths", "Documents: ", Documents);
            Log.Info("Paths", "GameContent: ", GameContent);
            Log.Info("Paths", "Mods: ", Mods);
            Log.Info("Paths", "Assets: ", Assets);
            Log.Info("Paths", "Saves: ", Saves);
            Log.Info("Paths", "Maps: ", Maps);
            Log.Info("Paths", "ModOptions: ", ModOptions);
            Log.Info("Paths", "Temp: ", Temp);
        }
    }
}
