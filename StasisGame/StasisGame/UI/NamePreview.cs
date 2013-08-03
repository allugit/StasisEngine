using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class NamePreview
    {
        private Screen _screen;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private UIAlignment _alignment;
        private int _xOffset;
        private int _yOffset;
        private string _name = "";
        private int _maxLetters;
        private int _letterSpacing = 32;
        private int _width;
        private Color _color = new Color(0.7f, 1f, 0.6f);
        private Texture2D _letterLine;

        public string name { get { return _name; } set { _name = value; } }
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

        public NamePreview(Screen screen, SpriteFont font, UIAlignment alignment, int x, int y, int maxLetters)
        {
            _screen = screen;
            _spriteBatch = _screen.screenSystem.spriteBatch;
            _font = font;
            _alignment = alignment;
            _xOffset = x;
            _yOffset = y;
            _maxLetters = maxLetters;
            _width = _maxLetters * _letterSpacing;
            _letterLine = ResourceManager.getTexture("line_indicator");
        }

        public void draw()
        {
            // Draw spaces
            for (int i = 0; i < _maxLetters; i++)
            {
                _spriteBatch.Draw(_letterLine, new Vector2(x + i * _letterSpacing, y + _letterSpacing), _letterLine.Bounds, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            }

            // Draw letters
            for (int i = 0; i < _name.Length; i++)
            {
                Vector2 letterPosition = new Vector2(x + (i * _letterSpacing) + (_letterSpacing / 2f) - 2, y);
                string letter = _name[i].ToString();
                Vector2 letterSize = _font.MeasureString(letter);


                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(-1, -1), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(0, -1), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(1, -1), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(1, 0), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(1, 1), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(0, 1), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(-1, 1), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(-1, 0), Color.Black, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition, _color, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
