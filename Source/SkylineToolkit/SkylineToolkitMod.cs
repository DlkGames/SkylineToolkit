using ICities;
using SkylineToolkit.Debugging;
using SkylineToolkit.PermaMod;
using System;
using UnityEngine;

namespace SkylineToolkit
{
    public sealed class SkylineToolkitMod : IUserMod
    {
        public const string SKYLINETOOLKIT_NAME = "___SkylineToolkit___";

        /// <summary>
        /// Do not change this value until you know what you're doing!
        /// </summary>
        public static bool overwriteExistingMods = true;

        private static bool isInitialized = false;

        private static GameObject toolkitGameObject;

        public static GameObject ToolkitGameObject
        {
            get
            {
                return toolkitGameObject;
            }
            private set
            {
                toolkitGameObject = value;
            }
        }

        public static SkylineToolkitPermaMod PermaModComponent
        {
            get
            {
                if (ToolkitGameObject == null)
                {
                    return null;
                }

                return ToolkitGameObject.GetComponent<SkylineToolkitPermaMod>();
            }
        }

        public static ModOptionsController ModOptionsControllerComponent
        {
            get
            {
                if (ToolkitGameObject == null)
                {
                    return null;
                }

                return ToolkitGameObject.GetComponent<ModOptionsController>();
            }
        }

        public static MainMenuModification MainMenuModComponent
        {
            get
            {
                if (ToolkitGameObject == null)
                {
                    return null;
                }

                return ToolkitGameObject.GetComponent<MainMenuModification>();
            }
        }

        public static bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
        }

        public string Description
        {
            get
            {
                return "Toolkit for easier Cities: Skylines mod creation. NOTE: Some features of this mod will be enabled even when it's disabled here."
                    + " If you enable this mod, achievements WON'T get disabled even for other mods.";
            }
        }

        public string Name
        {
            get
            {
                // The following method calls are hooks to initialize our permanent mod and main menu modification
                if (!isInitialized)
                {
                    Log.Info("SkylineToolkit", "Enabling SkylineToolkit...");

                    Initialize();
                }

                return "SkylineToolkit";
            }
        }

        private void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            Debugger.Initialize();

            try
            {
                EnsureGameObject();

                InitializePermaMod();
                InitializeModOptionsController();
                InitializeMainMenuMod();

                isInitialized = true;
            }
            catch (Exception ex)
            {
                Log.Error("SkylineToolkit", "Error while initializing SkylineToolkit. See the folowwing exception for more details:");
                Log.Exception(ex);

                isInitialized = false;
            }
        }

        private void EnsureGameObject()
        {
            Log.Debug("SkylineToolkit", "Ensure SkylineToolkit game object");

            if (overwriteExistingMods && ToolkitGameObject != null)
            {
                GameObject.DestroyImmediate(ToolkitGameObject);
                ToolkitGameObject = null;
            }

            GameObject toolkitObject = GameObject.Find(SKYLINETOOLKIT_NAME);

            if (toolkitObject != null)
            {
                if (overwriteExistingMods)
                {
                    GameObject.DestroyImmediate(toolkitObject);
                }
                else
                {
                    ToolkitGameObject = toolkitObject;
                    return;
                }
            }

            toolkitObject = new GameObject(SKYLINETOOLKIT_NAME);

            ToolkitGameObject = toolkitObject;
        }

        private void InitializePermaMod()
        {
            Log.Debug("SkylineToolkit", "Initialize PermaMod");

            if (ToolkitGameObject == null)
            {
                EnsureGameObject();
            }

            if (ToolkitGameObject.GetComponent<SkylineToolkitPermaMod>() == null)
            {
                ToolkitGameObject.AddComponent<SkylineToolkitPermaMod>();
            }
        }

        private void InitializeModOptionsController()
        {
            Log.Debug("SkylineToolkit", "Initialize mod options controller");

            if (ToolkitGameObject == null)
            {
                EnsureGameObject();
            }

            if (ToolkitGameObject.GetComponent<ModOptionsController>() == null)
            {
                ToolkitGameObject.AddComponent<ModOptionsController>();
            }
        }

        private void InitializeMainMenuMod()
        {
            Log.Debug("SkylineToolkit", "Initialize MainMenu modification");

            if (ToolkitGameObject == null)
            {
                EnsureGameObject();
            }

            if (ToolkitGameObject.GetComponent<MainMenuModification>() == null)
            {
                ToolkitGameObject.AddComponent<MainMenuModification>();
            }
        }
    }
}
