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

            Log.Info("PermaMod", "Current Level: {0} {1}", Application.loadedLevel, Application.loadedLevelName);

            if (Debugging.DebugConsole.Instance != null)
            {
                Log.Info("PermaMod", "Debug console created. Open with {0}.", Debugging.DebugConsole.Instance.toggleKey);
            }
        }

        void OnLevelWasLoaded(int level)
        {
            Log.Info("Loaded Level: {0} {1}", level, Application.loadedLevelName);
        }
    }
}
