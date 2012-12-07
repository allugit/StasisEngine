using System;
using System.Collections.Generic;
using StasisEditor.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.Views
{
    public class ShapeRenderer
    {
        private ILevelController _levelController;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private SpriteFont _arial;

        public ShapeRenderer(ILevelController levelController)
        {
            _levelController = levelController;
            _spriteBatch = XNAResources.spriteBatch;
            _pixel = XNAResources.pixel;
            _arial = XNAResources.arial;
        }

        // drawBox
        public void drawBox(Vector2 position, float halfWidth, float halfHeight, float angle)
        {
            float scale = _levelController.getScale();
            Rectangle rectangle = new Rectangle(0, 0, (int)(halfWidth * 2 * scale), (int)(halfHeight * 2 * scale));
            _spriteBatch.Draw(_pixel, (position + _levelController.getWorldOffset()) * scale, rectangle, Color.LightBlue, angle, new Vector2(halfWidth, halfHeight) * scale, 1, SpriteEffects.None, 0);
        }
    }
}
