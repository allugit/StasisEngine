using System;

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
    }
}
