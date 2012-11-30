using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.Models
{
    public class Level
    {
        private List<Brush> _brushes;

        public Level()
        {
            _brushes = new List<Brush>();
        }

        // load
        public static Level load(XmlDocument document)
        {
            return null;
        }

        // addBrush
        public void addBrush(Brush brush)
        {
            _brushes.Add(brush);
        }

        // removeBrush
        public void removeBrush(Brush brush)
        {
            _brushes.Remove(brush);
        }
    }
}
