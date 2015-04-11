using ICities;
using SkylineToolkit.CitiesExtension;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkylineToolkit
{
    public abstract class Mod : LoadingExtension, IMod
    {
        private static IDictionary<int, bool> firstRun = new Dictionary<int, bool>();

        private bool inGame = false;

        static Mod()
        {
            EmbeddedAssembly.RegisterResolver();
        }

        private bool IsFirstRun
        {
            get
            {
                if (!firstRun.ContainsKey(ModName.GetHashCode()))
                {
                    return true;
                }

                return firstRun[ModName.GetHashCode()];
            }
            set
            {
                if (!firstRun.ContainsKey(ModName.GetHashCode()))
                {
                    firstRun.Add(ModName.GetHashCode(), value);
                }

                firstRun[ModName.GetHashCode()] = value;
            }
        }

        public string Description
        {
            get
            {
                return ModDescription;
            }
        }

        public string Name
        {
            get
            {
                if (IsFirstRun && !inGame)
                {
                    IsFirstRun = false;

                    InternalOnApplicationStarted();
                    InternalOnMainMenuLoaded();
                }

                if (!IsFirstRun && Application.loadedLevel == (int)Level.MainMenu)
                {
                    InternalOnMainMenuLoaded();
                }

                return String.Format("{0} [{1}]", ModName, Version);
            }
        }

        public abstract string ModName
        {
            get;
        }

        public abstract string ModDescription
        {
            get;
        }

        public abstract string Version
        {
            get;
        }

        public abstract string Author
        {
            get;
        }

        private void InternalOnApplicationStarted()
        {
            OnApplicationStarted();
        }

        private void InternalOnMainMenuLoaded()
        {
            OnMainMenuLoaded();
        }

        public override void OnCreated(ILoading loading)
        {
            inGame = true;

            base.OnCreated(loading);
        }

        public override void OnReleased()
        {
        }

        protected virtual void OnApplicationStarted()
        {
        }

        protected virtual void OnMainMenuLoaded()
        {
        }
    }
}
