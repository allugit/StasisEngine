using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisCore
{
    public class BackgroundRenderer
    {
        private SpriteBatch _spriteBatch;
        private Background _background;

        public Background background { get { return _background; } set { _background = value; } }

        public BackgroundRenderer(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public float modulo(float x, float y)
        {
            return (x % y) + (x < 0 ? y : 0);
        }

        public void draw(Vector2 screenOffset)
        {
            Vector2 screenSize = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);
            Vector2 halfScreen = screenSize / 2f;

            foreach (BackgroundLayer layer in _background.layers)
            {
                float textureWidth = (float)layer.texture.Width;
                int count = (int)(Math.Ceiling(screenSize.X / textureWidth) + 1);
                float scrollingWidth = (float)count * textureWidth;
                float halfScrollingWidth = scrollingWidth / 2f;
                Vector2 scaledScreenOffset = screenOffset * layer.speedScale;

                for (int i = 0; i < count; i++)
                {
                    float xOffset = (float)i * textureWidth;
                    Vector2 position = new Vector2(
                        modulo(scaledScreenOffset.X + xOffset + layer.initialOffset.X, scrollingWidth),
                        scaledScreenOffset.Y + layer.initialOffset.Y);

                    _spriteBatch.Draw(layer.texture, position + halfScreen - new Vector2(halfScrollingWidth, 0), layer.texture.Bounds, Color.White, 0f, new Vector2(layer.texture.Width, layer.texture.Height) / 2, 1f, SpriteEffects.None, layer.layerDepth);
                }
            }
        }
    }
}
