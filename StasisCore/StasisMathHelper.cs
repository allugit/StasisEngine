﻿using System;

namespace StasisCore
{
    public class StasisMathHelper
    {
        public static float phi = (1f + (float)Math.Sqrt(5)) / 2;
        public static float pi = (float)Math.PI;

        public static float floatBetween(float low, float high, Random random)
        {
            float range = high - low;
            return (float)random.NextDouble() * range + low;
        }
    }
}
