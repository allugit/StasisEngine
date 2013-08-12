using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore
{
    abstract public class Loader
    {
        // Load a Vector2
        public static Vector2 loadVector2(XAttribute attribute, Vector2 defaultValue)
        {
            if (attribute == null)
                return defaultValue;
            return loadVector2(attribute.Value, defaultValue);
        }
        public static Vector2 loadVector2(XElement element, Vector2 defaultValue)
        {
            if (element == null)
                return defaultValue;
            return loadVector2(element.Value, defaultValue);
        }
        public static Vector2 loadVector2(string value, Vector2 defaultValue)
        {
            Regex regex = new Regex(@"X:([-\d\.]+)\s+Y:([-\d\.]+)");
            Match match = regex.Match(value);

            if (match.Groups.Count != 3)
                return defaultValue;

            return new Vector2(float.Parse(match.Groups[1].Value), float.Parse(match.Groups[2].Value));
        }

        // Load a Color
        public static Color loadColor(XAttribute attribute, Color defaultValue)
        {
            if (attribute == null)
                return defaultValue;
            return loadColor(attribute.Value, defaultValue);
        }
        public static Color loadColor(XElement element, Color defaultValue)
        {
            if (element == null)
                return defaultValue;
            return loadColor(element.Value, defaultValue);
        }
        public static Color loadColor(string value, Color defaultValue)
        {
            Regex regex = new Regex(@"R:([\d\.]+){1}\s+G:([\d\.]+){1}\s+B:([\d\.]+){1}\s+A:([\d\.]+)");
            //Regex regex = new Regex(@"R=(\d+), G=(\d+), B=(\d+), A=(\d+)");
            Match match = regex.Match(value);

            if (match.Groups.Count != 5)
                return defaultValue;

            return new Color(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value));
        }

        // Load an int
        public static int loadInt(XAttribute attribute, int defaultValue)
        {
            if (attribute == null)
                return defaultValue;
            return loadInt(attribute.Value, defaultValue);
        }
        public static int loadInt(XElement element, int defaultValue)
        {
            if (element == null)
                return defaultValue;
            return loadInt(element.Value, defaultValue);
        }
        public static int loadInt(string value, int defaultValue)
        {
            return int.Parse(value);
        }

        // Load a float
        public static float loadFloat(XAttribute attribute, float defaultValue)
        {
            if (attribute == null)
                return defaultValue;
            return loadFloat(attribute.Value, defaultValue);
        }
        public static float loadFloat(XElement element, float defaultValue)
        {
            if (element == null)
                return defaultValue;
            return loadFloat(element.Value, defaultValue);
        }
        public static float loadFloat(string value, float defaultValue)
        {
            return float.Parse(value);
        }

        // Load a bool
        public static bool loadBool(XAttribute attribute, bool defaultValue)
        {
            if (attribute == null)
                return defaultValue;
            return loadBool(attribute.Value, defaultValue);
        }
        public static bool loadBool(XElement element, bool defaultValue)
        {
            if (element == null)
                return defaultValue;
            return loadBool(element.Value, defaultValue);
        }
        public static bool loadBool(string value, bool defaultValue)
        {
            return bool.Parse(value);
        }

        // Load an enum
        public static int loadEnum(Type enumType, XAttribute attribute, int defaultValue)
        {
            if (attribute == null)
                return defaultValue;
            return loadEnum(enumType, attribute.Value, defaultValue);
        }
        public static int loadEnum(Type enumType, XElement element, int defaultValue)
        {
            if (element == null)
                return defaultValue;
            return loadEnum(enumType, element.Value, defaultValue);
        }
        public static int loadEnum(Type enumType, string value, int defaultValue)
        {
            return (int)Enum.Parse(enumType, value, true);
        }

        // Load a string
        public static string loadString(XAttribute attribute, string defaultValue)
        {
            if (attribute == null)
                return defaultValue;
            return attribute.Value;
        }
        public static string loadString(XElement element, string defaultValue)
        {
            if (element == null)
                return defaultValue;
            return element.Value;
        }
    }
}
