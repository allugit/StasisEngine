using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace StasisGame
{
    public class GameSettings
    {
        private static int _screenWidth;
        private static int _screenHeight;

        public static int screenWidth { get { return _screenWidth; } set { _screenWidth = value; } }
        public static int screenHeight { get { return _screenHeight; } set { _screenHeight = value; } }

        public static XElement data
        {
            get
            {
                XElement settings = new XElement("Settings");
                settings.Add(new XElement("ScreenWidth", _screenWidth));
                settings.Add(new XElement("ScreenHeight", _screenHeight));
                return settings;
            }
        }

        GameSettings()
        {
        }
    }
}
