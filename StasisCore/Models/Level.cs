using System;
using System.Collections.Generic;
using StasisCore.Resources;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class Level
    {
        private Vector2 _gravity;
        private Vector2 _wind;

        public Vector2 gravity { get { return _gravity; } set { _gravity = value; } }
        public Vector2 wind { get { return _wind; } set { _wind = value; } }

        public Level(ResourceObject resource)
        {
            _gravity = XmlLoadHelper.getVector2(resource.data.Attribute("gravity").Value);
            _wind = XmlLoadHelper.getVector2(resource.data.Attribute("wind").Value);
        }
    }
}
