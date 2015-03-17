using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SkylineToolkit
{
    /// <summary>
    /// http://www.reddit.com/r/CitiesSkylinesModding/comments/2ypcl5/guide_using_visual_studio_2013_to_develop_mods/
    /// </summary>
    public static class Debug
    {
        private static Queue<Tuple<string, long>> activeProfiles;

        static Debug()
        {
            activeProfiles = new Queue<Tuple<string, long>>();
        }

        public static void BeginProfile(string name)
        {
            if (Log.LogLevel < MessageType.Profile)
            {
                return;
            }

            long currentTicks = DateTime.Now.Ticks;

            activeProfiles.Enqueue(new Tuple<string, long>(name, currentTicks));

            Log.Profile("Profile {0} started at {1}", name, currentTicks);
        }

        public static void EndProfile()
        {
            if (Log.LogLevel < MessageType.Profile)
            {
                return;
            }

            long currentTicks = DateTime.Now.Ticks;

            Tuple<string, long> profile = activeProfiles.Dequeue();

            long durationTicks = currentTicks - profile.Item2;
            TimeSpan duration = new TimeSpan(durationTicks);

            Log.Profile("Profile {0} ended at {1} - Duration: {2} ms ({3} ticks)", profile.Item1, currentTicks, duration.Milliseconds, durationTicks);
        }

        public static GameObject[] GetAllGameObjects()
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            return allObjects;
        }

        public static string DumpAllGameObjects()
        {
            StringBuilder result = new StringBuilder();

            foreach (GameObject obj in GetAllGameObjects())
            {
                result.AppendLine(obj.name);
            }

            return result.ToString();
        }
    }
}
