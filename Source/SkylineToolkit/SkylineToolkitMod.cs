using ICities;
using SkylineToolkit.PermaMod;
using System;
using UnityEngine;

namespace SkylineToolkit
{
    public class SkylineToolkitMod : IUserMod
    {
        public const string SKYLINETOOLKIT_NAME = "___SkylineToolkit___";

        /// <summary>
        /// Do not change this value until you know what you're doing!
        /// </summary>
        public static bool overwriteExistingMods = true;

        private static bool isInitialized = false;

        private static GameObject toolkitGameObject;

        private static GameObject mainMenuModGameObject;

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
                    EnsureGameObject();

                    InitializePermaMod();
                    InitializeModOptionsController();
                    InitializeMainMenuMod();

                    isInitialized = true;
                }

                return "SkylineToolkit";
            }
        }

        private void EnsureGameObject()
        {
            if (ToolkitGameObject != null)
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
            if (ToolkitGameObject == null)
            {
                EnsureGameObject();
            }

            ToolkitGameObject.AddComponent<SkylineToolkitPermaMod>();
        }

        private void InitializeModOptionsController()
        {
            if (ToolkitGameObject == null)
            {
                EnsureGameObject();
            }

            ToolkitGameObject.AddComponent<ModOptionsController>();
        }

        private void InitializeMainMenuMod()
        {
            if (ToolkitGameObject == null)
            {
                EnsureGameObject();
            }

            ToolkitGameObject.AddComponent<MainMenuModification>();
        }
    }
}
