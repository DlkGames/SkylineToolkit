using System.Collections;
using UnityEngine;

namespace SkylineToolkit.Debugging
{
    internal class FpsCounter
    {
        private float current = 0.0f;
        private float updateInterval = 0.5f;

        private float accumulatedTime = 0;

        private int frames = 1;

        private float timeleft;
        private float delta;

        public float Current
        {
            get
            {
                return this.current;
            }
        }

        public FpsCounter()
        {
            timeleft = updateInterval;
        }

        public IEnumerator Update()
        {
            // Skip the first frame where everything is initializing.
            yield return null;

            while (true)
            {
                delta = Time.deltaTime;

                timeleft -= delta;
                accumulatedTime += Time.timeScale / delta;
                ++frames;

                // Interval ended - update GUI text and start a new interval
                if (timeleft <= 0.0f)
                {
                    current = accumulatedTime / frames;
                    timeleft = updateInterval;
                    accumulatedTime = 0.0f;
                    frames = 0;
                }

                yield return null;
            }
        }
    }
}
