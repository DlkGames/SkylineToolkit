using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Procedural
{
    public class PerlinAlt1
    {
        private int[,] m_aNoise;
        protected int m_nNoiseWidth, m_nNoiseHeight;
        private float m_fScaleX, m_fScaleY;

        public PerlinAlt1()
        {
            m_nNoiseWidth = 100;
            m_nNoiseHeight = 100;
            m_fScaleX = 1.0F;
            m_fScaleY = 1.0F;
            System.Random rnd = new System.Random();
            m_aNoise = new int[m_nNoiseWidth, m_nNoiseHeight];
            for (int x = 0; x < m_nNoiseWidth; x++)
            {
                for (int y = 0; y < m_nNoiseHeight; y++)
                {
                    m_aNoise[x, y] = rnd.Next(255);
                }
            }
        }

        public float Noise(float x)
        {
            return Noise(x, 0.5F);
        }

        public float Noise(float x, float y)
        {
            int Xint = (int)x;
            int Yint = (int)y;
            float Xfrac = x - Xint;
            float Yfrac = y - Yint;

            // Find the noise values of the four corners
            float x0y0 = Smooth_Noise(Xint, Yint);  
            float x1y0 = Smooth_Noise(Xint + 1, Yint);
            float x0y1 = Smooth_Noise(Xint, Yint + 1);
            float x1y1 = Smooth_Noise(Xint + 1, Yint + 1);

            // Interpolate between those values according to the x and y fractions
            float v1 = Interpolate(x0y0, x1y0, Xfrac); // Interpolate in x direction (y)
            float v2 = Interpolate(x0y1, x1y1, Xfrac); //Interpolate in x direction (y+1)
            float fin = Interpolate(v1, v2, Yfrac);  // Interpolate in y direction

            return fin;
        }

        private float Interpolate(float x, float y, float a)
        {
            float b = 1 - a;

            float fac1 = (float)(3 * b * b - 2 * b * b * b);
            float fac2 = (float)(3 * a * a - 2 * a * a * a);

            // Add the weighted factors
            return x * fac1 + y * fac2; 
        }

        private float GetRandomValue(int x, int y)
        {
            x = (x + m_nNoiseWidth) % m_nNoiseWidth;
            y = (y + m_nNoiseHeight) % m_nNoiseHeight;

            float fVal = (float)m_aNoise[(int)(m_fScaleX * x), (int)(m_fScaleY * y)];

            return fVal / 255 * 2 - 1f;
        }

        private float Smooth_Noise(int x, int y)
        {
            float corners = (Noise2d(x - 1, y - 1) + Noise2d(x + 1, y - 1) + Noise2d(x - 1, y + 1) + Noise2d(x + 1, y + 1)) / 16.0f;
            float sides = (Noise2d(x - 1, y) + Noise2d(x + 1, y) + Noise2d(x, y - 1) + Noise2d(x, y + 1)) / 8.0f;
            float center = Noise2d(x, y) / 4.0f;

            return corners + sides + center;
        }

        private float Noise2d(int x, int y)
        {
            x = (x + m_nNoiseWidth) % m_nNoiseWidth;
            y = (y + m_nNoiseHeight) % m_nNoiseHeight;

            float fVal = (float)m_aNoise[(int)(m_fScaleX * x), (int)(m_fScaleY * y)];

            return fVal / 255 * 2 - 1f;
        }
    }
}
