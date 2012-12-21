using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Resources;
using Microsoft.Xna.Framework;

namespace StasisEditor.Models
{
    public class EditorLevel
    {
        private Vector2 _gravity;
        private Vector2 _wind;

        public Vector2 gravity { get { return _gravity; } set { _gravity = value; } }
        public Vector2 wind { get { return _wind; } set { _wind = value; } }

        // Create new
        public EditorLevel()
        {
            _gravity = new Vector2(0, 32f);
            _wind = new Vector2(0, 0);
        }

        // Create from xml
        public EditorLevel(ResourceObject resource)
        {
            _gravity = XmlLoadHelper.getVector2(resource.data.Attribute("gravity").Value);
            _wind = XmlLoadHelper.getVector2(resource.data.Attribute("wind").Value);
        }
    }
}
