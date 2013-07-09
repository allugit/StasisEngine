using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class Label
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private string _text;
        private int _xOffset;
        private int _yOffset;
        private UIAlignment _alignment;
        private Color _color;

        public int x
        {
            get
            {
                if (_alignment == UIAlignment.TopLeft || _alignment == UIAlignment.MiddleLeft || _alignment == UIAlignment.BottomLeft)
                    return _xOffset;
                else if (_alignment == UIAlignment.TopCenter || _alignment == UIAlignment.MiddleCenter || _alignment == UIAlignment.BottomCenter)
                    return _xOffset + (int)(_spriteBatch.GraphicsDevice.Viewport.Width / 2f);
                else if (_alignment == UIAlignment.TopRight || _alignment == UIAlignment.MiddleRight || _alignment == UIAlignment.BottomRight)
                    return _xOffset + _spriteBatch.GraphicsDevice.Viewport.Width;

                return _xOffset;
            }
        }
        public int y
        {
            get
            {
                if (_alignment == UIAlignment.TopLeft || _alignment == UIAlignment.TopCenter || _alignment == UIAlignment.TopRight)
                    return _yOffset;
                else if (_alignment == UIAlignment.MiddleLeft || _alignment == UIAlignment.MiddleCenter || _alignment == UIAlignment.MiddleRight)
                    return _yOffset + (int)(_spriteBatch.GraphicsDevice.Viewport.Height / 2f);
                else if (_alignment == UIAlignment.BottomLeft || _alignment == UIAlignment.BottomCenter || _alignment == UIAlignment.BottomRight)
                    return _yOffset + _spriteBatch.GraphicsDevice.Viewport.Height;

                return _yOffset;
            }
        }

        public Label(SpriteBatch spriteBatch, SpriteFont font, int x, int y, UIAlignment alignment, string text)
            : this(spriteBatch, font, x, y, alignment, text, Color.White)
        {
        }

        public Label(SpriteBatch spriteBatch, SpriteFont font, int x, int y, UIAlignment alignment, string text, Color color)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _text = text;
            _xOffset = x;
            _yOffset = y;
            _alignment = alignment;
            _color = color;
        }

        public void draw()
        {
            _spriteBatch.DrawString(_font, _text, new Vector2(x, y), _color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
