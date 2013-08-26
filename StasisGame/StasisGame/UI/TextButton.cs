using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class TextButton : IAlphaFadable
    {
        private Screen _screen;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private UIAlignment _alignment;
        private int _xOffset;
        private int _yOffset;
        private TextAlignment _textAlignment;
        private string _text;
        private Action _onActivate;
        private Action _onMouseOver;
        private Action _onMouseOut;
        private int _outline;
        private Color _selectedColor;
        private Color _deselectedColor;
        private Color _color;
        private Rectangle _localHitBox;
        private int _hitBoxPadding;
        private float _alpha = 1f;

        public float alpha { get { return _alpha; } set { _alpha = value; } }
        public Screen screen { get { return _screen; } }
        public string text
        {
            get { return _text; }
            set 
            {
                Vector2 textSize = _font.MeasureString(value);

                _text = value;

                if (_textAlignment == TextAlignment.Center)
                {
                    _localHitBox.X = -(int)(textSize.X / 2f + _hitBoxPadding);
                    //_localHitBox.Y = -(int)(_hitBoxPadding / 2f);
                }
                else if (_textAlignment == TextAlignment.Right)
                {
                    _localHitBox.X = -(int)(textSize.X + _hitBoxPadding);
                }

                _localHitBox.Width = (int)textSize.X + _hitBoxPadding * 2;
                _localHitBox.Height = (int)textSize.Y + _hitBoxPadding;
            }
        }
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

        public TextButton(Screen screen, SpriteFont font, UIAlignment alignment, int xOffset, int yOffset, int hitBoxPadding, TextAlignment textAlignment, string text, int outline, Color selectedColor, Color deselectedColor, Action onActivate)
            : this(screen, font, alignment, xOffset, yOffset, hitBoxPadding, textAlignment, text, outline, selectedColor, deselectedColor, onActivate, null, null)
        {
            _onMouseOver = () => { select(); };
            _onMouseOut = () => { deselect(); };
        }

        public TextButton(Screen screen, SpriteFont font, UIAlignment alignment, int xOffset, int yOffset, int hitBoxPadding, TextAlignment textAlignment, string text, int outline, Color selectedColor, Color deselectedColor, Action onActivate, Action onMouseOver, Action onMouseOut)
        {
            _screen = screen;
            _spriteBatch = _screen.screenSystem.spriteBatch;
            _font = font;
            _alignment = alignment;
            _xOffset = xOffset;
            _yOffset = yOffset;
            _hitBoxPadding = hitBoxPadding;
            _textAlignment = textAlignment;
            this.text = text;
            _outline = outline;
            _selectedColor = selectedColor;
            _deselectedColor = deselectedColor;
            _onActivate = onActivate;
            _onMouseOver = onMouseOver;
            _onMouseOut = onMouseOut;
            _color = deselectedColor;
        }

        public void activate()
        {
            _onActivate();
        }

        public void mouseOver()
        {
            _onMouseOver();
        }

        public void mouseOut()
        {
            _onMouseOut();
        }

        public void select()
        {
            _color = _selectedColor;
        }

        public void deselect()
        {
            _color = _deselectedColor;
        }

        public bool hitTest(Vector2 point)
        {
            Rectangle pointRect = new Rectangle((int)point.X, (int)point.Y, 1, 1);
            Rectangle hitBox = new Rectangle(x + _localHitBox.X, y + _localHitBox.Y, _localHitBox.Width, _localHitBox.Height);
            return pointRect.Intersects(hitBox);
        }

        public void update()
        {
        }

        public void draw()
        {
            int x = this.x + (int)_screen.translationX;
            int y = this.y + (int)_screen.translationY;
            Vector2 origin = Vector2.Zero;
            Vector2 stringSize = _font.MeasureString(text);
            float shadowAlpha = _alpha >= 1f ? 1f : _alpha / 8f;

            if (_textAlignment == TextAlignment.Center)
                origin = new Vector2(stringSize.X / 2f, 0);
            else if (_textAlignment == TextAlignment.Right)
                origin = new Vector2(stringSize.X, 0);

            if (_outline > 0)
            {
                _spriteBatch.DrawString(_font, _text, new Vector2(x - _outline, y - _outline), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x, y - _outline), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x + _outline, y - _outline), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x + _outline, y), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x + _outline, y + _outline), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x, y + _outline), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x - _outline, y + _outline), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
                _spriteBatch.DrawString(_font, _text, new Vector2(x - _outline, y), Color.Black * shadowAlpha, 0f, origin, 1f, SpriteEffects.None, 0.0001f);
            }
            _spriteBatch.DrawString(_font, _text, new Vector2(x, y), _color * _alpha, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
