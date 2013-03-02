using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore
{
    public class BackgroundRenderer
    {
        private SpriteBatch _spriteBatch;
        private List<BackgroundLayer> _layers;
        private Vector2 _screenOffset;

        public BackgroundRenderer(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void addLayer(Texture2D texture, Vector2 initialOffset, Vector2 speedScale, float layerDepth)
        {
            _layers.Add(new BackgroundLayer(texture, initialOffset, speedScale, layerDepth));
        }

        public float modulo(float x, float y)
        {
            return (x % y) + (x < 0 ? y : 0);
        }

        public void draw()
        {
            Vector2 screenSize = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);

            foreach (BackgroundLayer layer in _layers)
            {
                float textureWidth = (float)layer.texture.Width;
                float scrollingWidth = (float)(Math.Ceiling(screenSize.X / textureWidth) + 1) * textureWidth;
                int count = (int)Math.Ceiling(scrollingWidth / textureWidth);
                Vector2 scaledScreenOffset = _screenOffset * layer.speedScale;

                for (int i = 0; i < count; i++)
                {
                    float xOffset = (float)i * textureWidth;
                    float tiledScaledX = scaledScreenOffset.X + xOffset + layer.initialOffset.X;
                    Vector2 position = new Vector2(
                        modulo(tiledScaledX, scrollingWidth) - textureWidth / 2f,
                        scaledScreenOffset.Y + layer.initialOffset.Y);

                    _spriteBatch.Draw(layer.texture, position, layer.texture.Bounds, Color.White, 0f, new Vector2(layer.texture.Width, layer.texture.Height) / 2, 1f, SpriteEffects.None, layer.layerDepth);
                }
            }
        }
    }
}
