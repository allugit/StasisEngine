using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class Label : IAlphaFadable, ITranslatable
    {
        private Screen _screen;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private string _text;
        private int _xOffset;
        private int _yOffset;
        private UIAlignment _alignment;
        private TextAlignment _textAlignment;
        private int _outline;
        private Color _color;
        private float _alpha = 1f;
        private float _translationX;
        private float _translationY;

        public string text { get { return _text; } set { _text = value; } }
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

        public Label(Screen screen, SpriteFont font, UIAlignment alignment, int x, int y, TextAlignment textAlignment, string text, int outline)
            : this(screen, font, alignment, x, y, textAlignment, text, outline, Color.White)
        {
        }

        public Label(Screen screen, SpriteFont font, UIAlignment alignment, int x, int y, TextAlignment textAlignment, string text, int outline, Color color)
        {
            _screen = screen;
            _spriteBatch = screen.screenSystem.spriteBatch;
            _font = font;
            _text = text;
            _outline = outline;
            _xOffset = x;
            _yOffset = y;
            _alignment = alignment;
            _textAlignment = textAlignment;
            _color = color;
        }

        public void draw()
        {
            Vector2 origin = Vector2.Zero;
            Vector2 stringSize = _font.MeasureString(text);
            float outlineAlpha = _alpha >= 1f ? 1f : _alpha / 8f;

            if (_textAlignment == TextAlignment.Center)
                origin = new Vector2(stringSize.X / 2f, 0);
            else if (_textAlignment == TextAlignment.Right)
                origin = new Vector2(stringSize.X, 0);

            if (_outline > 0)
            {
                _spriteBatch.DrawString(_font, _text, new Vector2(x - _outline, y - _outline), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x, y - _outline), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x + _outline, y - _outline), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x + _outline, y), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x + _outline, y + _outline), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x, y + _outline), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x - _outline, y + _outline), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x - _outline, y), Color.Black * outlineAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
            }
            _spriteBatch.DrawString(_font, _text, new Vector2(x, y), _color * _alpha, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
