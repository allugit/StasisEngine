using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisGame.Data
{
    public class GameSettings
    {
        private int _screenWidth;
        private int _screenHeight;
        private bool _fullscreen;
        private ControllerType _controllerType = ControllerType.Gamepad;

        public int screenWidth { get { return _screenWidth; } set { _screenWidth = value; } }
        public int screenHeight { get { return _screenHeight; } set { _screenHeight = value; } }
        public bool fullscreen { get { return _fullscreen; } set { _fullscreen = value; } }
        public ControllerType controllerType { get { return _controllerType; } set { _controllerType = value; } }

        public XElement data
        {
            get
            {
                XElement settings = new XElement("Settings");
                settings.Add(new XElement("ScreenWidth", _screenWidth));
                settings.Add(new XElement("ScreenHeight", _screenHeight));
                settings.Add(new XElement("Fullscreen", _fullscreen));
                settings.Add(new XElement("ControllerType", _controllerType));
                return settings;
            }
        }

        // Construct new GameSettings instance using default values
        public GameSettings(LoderGame game)
        {
            // Find suitable screen size
            int screenWidth = 1024;
            int screenHeight = 768;
            int maxScreenWidth = game.GraphicsDevice.DisplayMode.Width - 100;
            int maxScreenHeight = game.GraphicsDevice.DisplayMode.Height - 100;
            /*
            DisplayDevice currentDisplayDevice = DisplayDevice.GetDisplay(DisplayIndex.Primary);
            List<DisplayResolution> availableResolutions = currentDisplayDevice.AvailableResolutions as List<DisplayResolution>;

            foreach (DisplayResolution displayResolution in availableResolutions)
            {
                if (displayResolution.Width < maxScreenWidth && displayResolution.Height < maxScreenHeight &&
                    displayResolution.Width >= screenWidth && displayResolution.Height >= screenHeight)
                {
                    screenWidth = displayResolution.Width;
                    screenHeight = displayResolution.Height;
                }
            }
            */
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _fullscreen = false;
            _controllerType = GamePad.GetState(PlayerIndex.One).IsConnected ? ControllerType.Gamepad : ControllerType.KeyboardAndMouse;
        }

        // Construct new GameSettings instance using values loaded from file
        public GameSettings(XElement data)
        {
            _screenWidth = Loader.loadInt(data.Element("ScreenWidth"), 800);
            _screenHeight = Loader.loadInt(data.Element("ScreenHeight"), 600);
            _fullscreen = Loader.loadBool(data.Element("Fullscreen"), false);
            _controllerType = (ControllerType)Loader.loadEnum(typeof(ControllerType), data.Element("ControllerType"), (int)ControllerType.KeyboardAndMouse);
        }
    }
}
