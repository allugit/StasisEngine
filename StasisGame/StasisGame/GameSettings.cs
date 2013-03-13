using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace StasisGame
{
    public enum ControllerType
    {
        KeyboardAndMouse,
        Gamepad
    }

    public class GameSettings
    {
        private static int _screenWidth;
        private static int _screenHeight;
        private static ControllerType _controllerType = ControllerType.Gamepad;

        public static int screenWidth { get { return _screenWidth; } set { _screenWidth = value; } }
        public static int screenHeight { get { return _screenHeight; } set { _screenHeight = value; } }
        public static ControllerType controllerType { get { return _controllerType; } set { _controllerType = value; } }

        public static XElement data
        {
            get
            {
                XElement settings = new XElement("Settings");
                settings.Add(new XElement("ScreenWidth", _screenWidth));
                settings.Add(new XElement("ScreenHeight", _screenHeight));
                settings.Add(new XElement("ControllerType", _controllerType));
                return settings;
            }
        }

        GameSettings()
        {
        }
    }
}
