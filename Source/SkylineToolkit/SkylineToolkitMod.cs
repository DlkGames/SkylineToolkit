﻿using ICities;
using SkylineToolkit.PermaMod;
using UnityEngine;

namespace SkylineToolkit
{
    public class SkylineToolkitMod : IUserMod
    {
        public const string PERMAMOD_NAME = "___SkylineToolkit";

        public const string MAINMENUMOD_NAME = "___SkylineToolkit_MainMenuMod";

        /// <summary>
        /// Do not change this value until you know what you're doing!
        /// </summary>
        public static bool overwriteExistingMods = true;

        private static GameObject permaModGameObject;

        private static GameObject mainMenuModGameObject;

        public static GameObject PermaModGameObject
        {
            get
            {
                return permaModGameObject;
            }
            private set
            {
                permaModGameObject = value;
            }
        }

        public static SkylineToolkitPermaMod PermaModComponent
        {
            get
            {
                if (PermaModGameObject == null)
                {
                    return null;
                }

                return PermaModGameObject.GetComponent<SkylineToolkitPermaMod>();
            }
        }

        public static GameObject MainMenuModGameObject
        {
            get
            {
                return mainMenuModGameObject;
            }
            private set
            {
                mainMenuModGameObject = value;
            }
        }

        public static MainMenuModification MainMenuModComponent
        {
            get
            {
                if (MainMenuModGameObject == null)
                {
                    return null;
                }

                return MainMenuModGameObject.GetComponent<MainMenuModification>();
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
                InitializePermaMod();
                InitializeMainMenuMod();

                return "SkylineToolkit";
            }
        }

        private void InitializePermaMod()
        {
            if (PermaModGameObject != null)
            {
                GameObject.DestroyImmediate(PermaModGameObject);
                PermaModGameObject = null;
            }

            GameObject toolkitObject = GameObject.Find(PERMAMOD_NAME);

            if (toolkitObject != null)
            {
                if (overwriteExistingMods)
                {
                    GameObject.DestroyImmediate(toolkitObject);
                }
                else
                {
                    PermaModGameObject = toolkitObject;
                    return;
                }
            }

            toolkitObject = new GameObject(PERMAMOD_NAME);

            SkylineToolkitPermaMod component = toolkitObject.AddComponent<SkylineToolkitPermaMod>();

            PermaModGameObject = toolkitObject;
        }

        private void InitializeMainMenuMod()
        {
            if (MainMenuModGameObject != null)
            {
                GameObject.DestroyImmediate(MainMenuModGameObject);
                MainMenuModGameObject = null;
            }

            GameObject mainMenuObject = GameObject.Find(MAINMENUMOD_NAME);

            if (mainMenuObject != null)
            {
                if (overwriteExistingMods)
                {
                    GameObject.DestroyImmediate(mainMenuObject);
                }
                else
                {
                    MainMenuModGameObject = mainMenuObject;
                    return;
                }
            }

            mainMenuObject = new GameObject(MAINMENUMOD_NAME);

            MainMenuModification component = mainMenuObject.AddComponent<MainMenuModification>();

            MainMenuModGameObject = mainMenuObject;
        }
    }
}
