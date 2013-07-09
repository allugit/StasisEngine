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
        private Vector2 _screenSize;
        private Vector2 _halfScreen;
        private Vector2 _screenOffset;
        private float _scale;

        public Background background { get { return _background; } set { _background = value; } }
        public Vector2 screenOffset { get { return _screenOffset; } set { _screenOffset = value; } }

        public BackgroundRenderer(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public float modulo(float x, float y)
        {
            return (x % y) + (x < 0 ? y : 0);
        }

        public void update(float scale, Vector2 screenOffset)
        {
            _scale = scale;
            _screenOffset = screenOffset;
            _screenSize = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);
            _halfScreen = _screenSize / 2f;

            foreach (BackgroundLayer layer in _background.layers)
                layer.position = _screenOffset * layer.speedScale;
        }

        public void recalculateScales()
        {
            _background.recalculateScales();
        }

        public void drawLayer(BackgroundLayer layer)
        {
            float textureWidth = (float)layer.texture.Width;
            int count = (int)(Math.Ceiling(_screenSize.X / textureWidth) + 1);
            float scrollingWidth = (float)count * textureWidth;
            float halfScrollingWidth = scrollingWidth / 2f;

            if (layer.tile)
            {
                for (int i = 0; i < count; i++)
                {
                    float xOffset = (float)i * textureWidth;
                    Vector2 position = new Vector2(
                        modulo((layer.position.X + layer.initialOffset.X) * _scale + xOffset, scrollingWidth),
                        (layer.position.Y + layer.initialOffset.Y) * _scale);

                    _spriteBatch.Draw(layer.texture, position + _halfScreen - new Vector2(halfScrollingWidth, 0), layer.texture.Bounds, layer.baseColor, 0f, new Vector2(layer.texture.Width, layer.texture.Height) / 2, layer.scale, SpriteEffects.None, layer.layerDepth);
                }
            }
            else
            {
                _spriteBatch.Draw(layer.texture, (layer.position + layer.initialOffset) * _scale + _halfScreen, layer.texture.Bounds, layer.baseColor, 0f, new Vector2(layer.texture.Width, layer.texture.Height) / 2, layer.scale, SpriteEffects.None, layer.layerDepth);
            }
        }

        public void draw()
        {
            foreach (BackgroundLayer layer in _background.layers)
                drawLayer(layer);
        }

        public void drawFirstHalf()
        {
            foreach (BackgroundLayer layer in _background.layers)
            {
                if (layer.layerDepth >= 0.5f)
                    drawLayer(layer);
            }
        }

        public void drawSecondHalf()
        {
            foreach (BackgroundLayer layer in _background.layers)
            {
                if (layer.layerDepth < 0.5f)
                    drawLayer(layer);
            }
        }
    }
}
