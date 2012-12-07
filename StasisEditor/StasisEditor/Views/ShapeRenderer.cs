using System;
using System.Collections.Generic;
using StasisEditor.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.Views
{
    public class ShapeRenderer
    {
        ILevelController _levelController;
        SpriteBatch _spriteBatch;

        public ShapeRenderer(ILevelController levelController, SpriteBatch spriteBatch)
        {
            _levelController = levelController;
            _spriteBatch = spriteBatch;
        }

        // drawBox
        public void drawBox(float halfWidth, float halfHeight, float angle)
        {
            Console.WriteLine("draw box");
        }
    }
}
