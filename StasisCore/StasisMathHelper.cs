using System;
using System.Collections.Generic;
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

        public static void bezier(ref Vector2 p0, ref Vector2 p1, ref Vector2 p2, ref Vector2 p3, float t, out Vector2 result)
        {
            float cx = 3 * (p1.X - p0.X);
            float cy = 3 * (p1.Y - p0.Y);
            float bx = 3 * (p2.X - p1.X) - cx;
            float by = 3 * (p2.Y - p1.Y) - cy;
            float ax = p3.X - p0.X - cx - bx;
            float ay = p3.Y - p0.Y - cy - by;
            float cube = t * t * t;
            float square = t * t;
            result.X = (ax * cube) + (bx * square) + (cx * t) + p0.X;
            result.Y = (ay * cube) + (by * square) + (cy * t) + p0.Y;
        }

        public static bool isPolygonClockwise(List<Vector2> points)
        {
            float sum = 0;

            for (int i = 0; i < points.Count; i++)
            {
                int j = i == points.Count - 1 ? 0 : i + 1;
                sum += (points[j].X - points[i].X) * (points[j].Y + points[i].Y);
            }

            return sum >= 0;
        }
    }
}
