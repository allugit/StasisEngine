using System;

namespace StasisCore
{
    public class StasisMathHelper
    {
        public static float floatBetween(float low, float high, Random random)
        {
            float range = high - low;
            return (float)random.NextDouble() * range + low;
        }
    }
}
