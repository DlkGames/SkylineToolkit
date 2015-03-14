using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.PermaMod
{
    public class SkylineToolkitPermaMod : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);

            Log.Info("Current Level: {0} {1}", Application.loadedLevel, Application.loadedLevelName);
        }

        void OnLevelWasLoaded(int level)
        {
            Log.Info("Loaded Level: {0} {1}", level, Application.loadedLevelName);
        }
    }
}
