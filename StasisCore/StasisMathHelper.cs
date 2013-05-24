using System;
using Microsoft.Xna.Framework;

namespace StasisCore
{
    public class StasisMathHelper
    {
        public static float phi = (1f + (float)Math.Sqrt(5)) / 2;
        public static float halfPi = (float)(Math.PI / 2);
        public static float pi = (float)Math.PI;
        public static float pi2 = (float)(Math.PI * 2);

        public static float floatBetween(float low, float high, Random random)
        {
            float newLow = Math.Min(low, high);
            float newHigh = Math.Max(low, high);
            float range = newHigh - newLow;
            return (float)random.NextDouble() * range + newLow;
        }

        public static int mod(int x, int modulus)
        {
            int result = x % modulus;
            return result < 0 ? modulus + x : result;
        }

        public static void interpolate(ref Vector2 a, ref Vector2 b, ref Vector2 c, ref Vector2 d, float mu, out Vector2 result)
        {
            float mu2 = mu * mu;
            float a0 = d.Y - c.Y - a.Y + b.Y;
            float a1 = a.Y - b.Y - a0;
            float a2 = c.Y - a.Y;
            float a3 = b.Y;
            result.Y = a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3;

            a0 = d.X - c.X - a.X + b.X;
            a1 = a.X - b.X - a0;
            a2 = c.X - a.X;
            a3 = b.X;
            result.X = a0 * mu * mu2 + a1 * mu2 + a2 * mu + a3;
        }
    }
}
