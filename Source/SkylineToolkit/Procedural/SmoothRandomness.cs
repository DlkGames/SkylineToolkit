using UnityEngine;

namespace SkylineToolkit.Procedural
{
    public class SmoothRandom
    {
        private static FractalNoise fracNoise;

        public static Vector3 GetVector3(float speed)
        {
            float time = Time.time * 0.01f * speed;

            return new Vector3(
                GetFractalNoise().HybridMultifractal(time, 15.73f, 0.58f),
                GetFractalNoise().HybridMultifractal(time, 63.94f, 0.58f),
                GetFractalNoise().HybridMultifractal(time, 0.2f, 0.58f));
        }

        public static float Get(float speed)
        {
            float time = Time.time * 0.01f * speed;

            return GetFractalNoise()
                .HybridMultifractal(time * 0.01f, 15.7f, 0.65f);
        }

        private static FractalNoise GetFractalNoise()
        {
            if (fracNoise == null)
            {
                fracNoise = new FractalNoise(1.27f, 2.04f, 8.36f);
            }

            return fracNoise;
        }

    }
}
