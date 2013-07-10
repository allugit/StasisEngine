using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class TextButton
    {
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
        private Color _selectedColor;
        private Color _deselectedColor;
        private Color _color;
        private Rectangle _localHitBox;

        public string text
        {
            get { return _text; }
            set 
            {
                Vector2 textSize = _font.MeasureString(value);

                _text = value;

                if (_textAlignment == TextAlignment.Center)
                {
                    _localHitBox.X = -(int)(textSize.X / 2f);
                    //_localHitBox.Y = -(int)(textSize.Y / 2f);
                }

                _localHitBox.Width = (int)textSize.X;
                _localHitBox.Height = (int)textSize.Y;
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

        public TextButton(SpriteBatch spriteBatch, SpriteFont font, UIAlignment alignment, int xOffset, int yOffset, TextAlignment textAlignment, string text, Color selectedColor, Color deselectedColor, Action onActivate)
            : this(spriteBatch, font, alignment, xOffset, yOffset, textAlignment, text, selectedColor, deselectedColor, onActivate, null, null)
        {
            _onMouseOver = () => { select(); };
            _onMouseOut = () => { deselect(); };
        }

        public TextButton(SpriteBatch spriteBatch, SpriteFont font, UIAlignment alignment, int xOffset, int yOffset, TextAlignment textAlignment, string text, Color selectedColor, Color deselectedColor, Action onActivate, Action onMouseOver, Action onMouseOut)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _alignment = alignment;
            _xOffset = xOffset;
            _yOffset = yOffset;
            _textAlignment = textAlignment;
            this.text = text;
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
            int x = this.x;
            int y = this.y;
            Vector2 origin = Vector2.Zero;
            Vector2 stringSize = _font.MeasureString(text);

            if (_textAlignment == TextAlignment.Center)
                origin = new Vector2(stringSize.X / 2f, 0);
            else if (_textAlignment == TextAlignment.Right)
                origin = new Vector2(stringSize.X, 0);

            _spriteBatch.DrawString(_font, _text, new Vector2(x, y), _color, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
