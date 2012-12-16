using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace StasisCore.Models
{
    abstract public class XmlLoadHelper
    {
        // Convert a string to Vector2
        public static Vector2 getVector2(string value)
        {
            Regex regex = new Regex(@"X:([\d\.]+){1}\s+Y:([\d\.]+)");
            Match match = regex.Match(value);

            if (match.Groups.Count != 3)
                return Vector2.Zero;

            return new Vector2(float.Parse(match.Groups[1].Value), float.Parse(match.Groups[2].Value));
        }

        // Convert a string to Color
        public static Color getColor(string value)
        {
            Regex regex = new Regex(@"R:([\d\.]+){1}\s+G:([\d\.]+){1}\s+B:([\d\.]+){1}\s+A:([\d\.]+)");
            Match match = regex.Match(value);

            if (match.Groups.Count != 5)
                return Color.White;

            return new Color(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value));
        }
    }
}
