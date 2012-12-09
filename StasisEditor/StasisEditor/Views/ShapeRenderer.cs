using System;
using System.Collections.Generic;
using StasisEditor.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisEditor.Views
{
    public class ShapeRenderer
    {
        private ILevelController _levelController;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private SpriteFont _arial;
        private Texture2D _circle;
        private Texture2D _playerSpawnIcon;
        private Texture2D _timerIcon;

        public ShapeRenderer(ILevelController levelController)
        {
            _levelController = levelController;
            _spriteBatch = XNAResources.spriteBatch;
            _pixel = XNAResources.pixel;
            _arial = XNAResources.arial;
            _circle = XNAResources.circle;
            _playerSpawnIcon = XNAResources.playerSpawnIcon;
            _timerIcon = XNAResources.timerIcon;
        }

        // drawBox
        public void drawBox(Vector2 position, float halfWidth, float halfHeight, float angle, Color color)
        {
            float scale = _levelController.getScale();
            Rectangle rectangle = new Rectangle(0, 0, (int)(halfWidth * 2 * scale), (int)(halfHeight * 2 * scale));
            _spriteBatch.Draw(_pixel, (position + _levelController.getWorldOffset()) * scale, rectangle, color, angle, new Vector2(halfWidth, halfHeight) * scale, 1, SpriteEffects.None, 0);
        }

        // drawLine
        public void drawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            float scale = _levelController.getScale();
            Vector2 relative = pointB - pointA;
            float length = relative.Length();
            float angle = (float)Math.Atan2(relative.Y, relative.X);
            Rectangle rect = new Rectangle(0, 0, (int)(length * scale), 2);
            _spriteBatch.Draw(_pixel, (pointA + _levelController.getWorldOffset()) * scale, rect, color, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
        }

        // drawPoint
        public void drawPoint(Vector2 position, Color color)
        {
            drawCircle(position, 4f / _levelController.getScale(), color);
        }

        // drawCircle
        public void drawCircle(Vector2 position, float radius, Color color)
        {
            float circleScale = radius / (((float)_circle.Width / 2) / _levelController.getScale());
            _spriteBatch.Draw(_circle, (position + _levelController.getWorldOffset()) * _levelController.getScale(), _circle.Bounds, color, 0, new Vector2(_circle.Width, _circle.Height) / 2, circleScale, SpriteEffects.None, 0);
        }

        // drawIcon
        public void drawIcon(ActorType actorType, Vector2 position)
        {
            Texture2D texture = _pixel;

            switch (actorType)
            {
                case ActorType.PlayerSpawn:
                    texture = _playerSpawnIcon;
                    break;
            }

            Rectangle rect = texture == _pixel ? new Rectangle(0, 0, 24, 24) : texture.Bounds;

            _spriteBatch.Draw(texture, (position + _levelController.getWorldOffset()) * _levelController.getScale(), rect, Color.White, 0, new Vector2(rect.Width, rect.Height) / 2, 1f, SpriteEffects.None, 0);
        }
    }
}
