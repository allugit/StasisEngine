using System;
using System.Collections.Generic;
using System.Xml;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class Level
    {
        private Vector2 _wind;
        private Vector2 _gravity;

        [CategoryAttribute("Environment")]
        public Vector2 Wind { get { return _wind; } set { _wind = value; } }

        [CategoryAttribute("Environment")]
        public Vector2 Gravity { get { return _gravity; } set { _gravity = value; } }

        public Level()
        {
        }

        // load
        public static Level load(XmlDocument document)
        {
            return null;
        }
    }
}
