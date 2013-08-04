using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class NamePreview : IAlphaFadable, ITranslatable
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
        private float _alpha = 1f;
        private float _translationX;
        private float _translationY;

        public string name { get { return _name; } set { _name = value; } }
        public float alpha { get { return _alpha; } set { _alpha = value; } }
        public float translationX { get { return _translationX; } set { _translationX = value; } }
        public float translationY { get { return _translationY; } set { _translationY = value; } }
        public Screen screen { get { return _screen; } }
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
            float outlineAlpha = _alpha >= 1f ? 1f : _alpha / 8f;

            // Draw spaces
            for (int i = 0; i < _maxLetters; i++)
            {
                _spriteBatch.Draw(_letterLine, new Vector2(x + i * _letterSpacing + (int)_translationX, y + _letterSpacing + (int)_translationY), _letterLine.Bounds, Color.White * _alpha, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            }

            // Draw letters
            for (int i = 0; i < _name.Length; i++)
            {
                Vector2 letterPosition = new Vector2(x + (i * _letterSpacing) + (_letterSpacing / 2f) - 2, y) + new Vector2(_translationX, _translationY);
                string letter = _name[i].ToString();
                Vector2 letterSize = _font.MeasureString(letter);


                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(-2, -2), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(0, -2), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(2, -2), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(2, 0), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(2, 2), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(0, 2), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(-2, 2), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition + new Vector2(-2, 0), Color.Black * outlineAlpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, letter, letterPosition, _color * _alpha, 0f, new Vector2(letterSize.X / 2f, 0), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
