using System;
using System.Collections.Generic;
using System.Xml;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Resources
{
    public class LevelResource
    {
        private Vector2 _wind;
        private Vector2 _gravity;

        [CategoryAttribute("Environment")]
        public Vector2 Wind { get { return _wind; } set { _wind = value; } }

        [CategoryAttribute("Environment")]
        public Vector2 Gravity { get { return _gravity; } set { _gravity = value; } }

        public LevelResource()
        {
        }

        // load
        public static LevelResource load(XmlDocument document)
        {
            return null;
        }
    }
}
