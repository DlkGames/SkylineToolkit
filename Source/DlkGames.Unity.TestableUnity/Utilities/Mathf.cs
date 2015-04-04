using System;

#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public static class Mathf
    {
        public const float PI = 3.141593f;

        public const float Infinity = float.PositiveInfinity;

        public const float NegativeInfinity = float.NegativeInfinity;

        public const float Deg2Rad = 0.01745329f;

        public const float Rad2Deg = 57.29578f;

        public const float Epsilon = 1.401298E-45f;

        internal const double MagicDamp1 = 0.479999989271164;
        internal const double MagicDamp2 = 0.234999999403954;
        
        public static float Sin(float f)
        {
            return (float)Math.Sin(f);
        }

        public static float Cos(float f)
        {
            return (float)Math.Cos(f);
        }

        public static float Tan(float f)
        {
            return (float)Math.Tan(f);
        }

        public static float Asin(float f)
        {
            return (float)Math.Asin(f);
        }

        public static float Acos(float f)
        {
            return (float)Math.Acos(f);
        }

        public static float Atan(float f)
        {
            return (float)Math.Atan(f);
        }

        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }

        public static float Abs(float f)
        {
            return Math.Abs(f);
        }

        public static int Abs(int value)
        {
            return Math.Abs(value);
        }

        public static float Min(float a, float b)
        {
            return (double)a < b ? a : b;
        }

        public static float Min(params float[] values)
        {
            var length = values.Length;

            if (length == 0)
            {
                return 0.0f;
            }

            var num = values[0];

            for (var index = 1; index < length; ++index)
            {
                if (values[index] < (double)num)
                {
                    num = values[index];
                }
            }

            return num;
        }

        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        public static int Min(params int[] values)
        {
            var length = values.Length;
            
            if (length == 0)
            {
                return 0;
            }

            var num = values[0];
            
            for (var index = 1; index < length; ++index)
            {
                if (values[index] < num)
                {
                    num = values[index];
                }
            }
            
            return num;
        }

        public static float Max(float a, float b)
        {
            return a > (double)b ? a : b;
        }

        public static float Max(params float[] values)
        {
            var length = values.Length;
            
            if (length == 0)
            {
                return 0.0f;
            }

            var num = values[0];
            
            for (var index = 1; index < length; ++index)
            {
                if (values[index] > (double)num)
                {
                    num = values[index];
                }
            }
            
            return num;
        }

        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        public static int Max(params int[] values)
        {
            var length = values.Length;
            
            if (length == 0)
            {
                return 0;
            }

            var num = values[0];
            
            for (var index = 1; index < length; ++index)
            {
                if (values[index] > num)
                {
                    num = values[index];
                }
            }
            
            return num;
        }

        public static float Pow(float f, float p)
        {
            return (float)Math.Pow(f, p);
        }

        public static float Exp(float power)
        {
            return (float)Math.Exp(power);
        }

        public static float Log(float f, float p)
        {
            return (float)Math.Log(f, p);
        }

        public static float Log(float f)
        {
            return (float)Math.Log(f);
        }

        public static float Log10(float f)
        {
            return (float)Math.Log10(f);
        }

        public static float Ceil(float f)
        {
            return (float)Math.Ceiling(f);
        }

        public static float Floor(float f)
        {
            return (float)Math.Floor(f);
        }

        public static float Round(float f)
        {
            return (float)Math.Round(f);
        }

        public static int CeilToInt(float f)
        {
            return (int)Math.Ceiling(f);
        }

        public static int FloorToInt(float f)
        {
            return (int)Math.Floor(f);
        }

        public static int RoundToInt(float f)
        {
            return (int)Math.Round(f);
        }

        public static float Sign(float f)
        {
            return (double)f >= 0.0 ? 1f : -1f;
        }

        public static float Clamp(float value, float min, float max)
        {
            if ((double)value < min)
            {
                value = min;
            }
            else if ((double)value > max)
            {
                value = max;
            }

            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        public static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1.0 ? 1f : value;
        }

        public static float Lerp(float from, float to, float t)
        {
            if (t < 0.0f)
            {
                return from;
            }

            if (t > 1.0f)
            {
                return to;
            }

            return (to - from) * t + from;
        }

        public static float LerpAngle(float a, float b, float t)
        {
            var num = Repeat(b - a, 360f);
            
            if (num > 180.0)
            {
                num -= 360f;
            }

            return a + num * Clamp01(t);
        }

        public static float MoveTowards(float current, float target, float maxDelta)
        {
            if (Math.Abs(target - current) <= (double)maxDelta)
            {
                return target;
            }

            return current + Sign(target - current) * maxDelta;
        }

        public static float MoveTowardsAngle(float current, float target, float maxDelta)
        {
            target = current + DeltaAngle(current, target);
            return MoveTowards(current, target, maxDelta);
        }

        public static float SmoothStep(float from, float to, float t)
        {
            if (t < 0.0f)
                return from;
            if (t > 1.0f)
                return to;
            t = t * t * (3.0f - 2.0f * t);
            return (1.0f - t) * from + t * to;
        }

        public static float Gamma(float value, float absmax, float gamma)
        {
            var flag = value < 0.0;
            var num1 = Abs(value);

            if (num1 > (double)absmax)
            {
                if (flag)
                {
                    return -num1;
                }

                return num1;
            }

            var num2 = Pow(num1 / absmax, gamma) * absmax;
            
            if (flag)
            {
                return -num2;
            }

            return num2;
        }

        public static bool Approximately(float a, float b)
        {
            return Abs(b - a) < (double)Max(1E-06f * Max(Abs(a), Abs(b)), 1.121039E-44f);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
        {
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, Time.deltaTime);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
        {
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            smoothTime = Max(0.0001f, smoothTime);

            var max = maxSpeed * smoothTime;

            var num1 = 2f / smoothTime;
            var num2 = num1 * deltaTime;
            var num3 = (float)(1.0 / (1.0 + num2 + MagicDamp1 * num2 * num2 + MagicDamp2 * num2 * num2 * num2));
            var num4 = current - target;
            var num5 = target;
            var num6 = Clamp(num4, -max, max);
            var num7 = (currentVelocity + num1 * num6) * deltaTime;
            var num8 = target + (num6 + num7) * num3;

            target = current - num6;
            currentVelocity = (currentVelocity - num1 * num7) * num3;

            if (num5 - (double)current > 0.0 == num8 > (double)num5)
            {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }

            return num8;
        }

        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
        {
            return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, Time.deltaTime);
        }

        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
        {
            return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime);
        }

        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            target = current + DeltaAngle(current, target);

            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static float Repeat(float t, float length)
        {
            return (float)(t - Math.Floor(t / length) * length);
        }

        public static float PingPong(float t, float length)
        {
            t = Repeat(t, length * 2f);
            return length - Math.Abs(t - length);
        }

        public static float InverseLerp(float from, float to, float value)
        {
            if (from < to)
            {
                if (value < from)
                {
                    return 0.0f;
                }

                if (value > to)
                {
                    return 1.0f;
                }
            }
            else
            {
                if (value < to)
                {
                    return 1.0f;
                }

                if (value > @from)
                {
                    return 0.0f;
                }
            }

            return (value - from) / (to - from);
        }

        public static float DeltaAngle(float current, float target)
        {
            var num = Repeat(target - current, 360f);

            if (num > 180.0)
            {
                num -= 360f;
            }

            return num;
        }
    }
}