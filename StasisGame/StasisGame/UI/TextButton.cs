using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class TextButton : ISelectableUIComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private string _text;
        private bool _selected;
        private UIComponentAlignment _alignment;
        private Texture2D _pixel;
        private Color _color;
        private int _xOffset;
        private int _yOffset;
        private int _hitBoxOffsetX;
        private int _hitBoxOffsetY;
        private int _hitBoxWidth;
        private int _hitBoxHeight;
        private UIComponentAction _action;

        public bool selectable { get { return true; } }
        public float layerDepth { get { return 0f; } }
        public string text
        {
            get { return _text; }
            set
            {
                _text = value;
                _hitBoxWidth = (int)_font.MeasureString(_text).X;
                _hitBoxHeight = (int)_font.MeasureString(_text).Y;

                if (_alignment == UIComponentAlignment.TopCenter)
                {
                    _hitBoxOffsetX = (int)(_hitBoxWidth / 2f);
                    _hitBoxOffsetY = (int)(_hitBoxHeight / 2f);
                }
            }
        }
        public int x
        {
            get
            {
                if (_alignment == UIComponentAlignment.TopCenter)
                    return _xOffset + (int)(_spriteBatch.GraphicsDevice.Viewport.Width / 2f);

                return _xOffset;
            }
        }
        public int y
        {
            get
            {
                return _yOffset;
            }
        }

        public TextButton(SpriteBatch spriteBatch, SpriteFont font, Color color, int xOffset, int yOffset, string text, UIComponentAlignment alignment, UIComponentAction action)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _color = color;
            _alignment = alignment;
            _xOffset = xOffset;
            _yOffset = yOffset;
            _action = action;
            this.text = text;

            _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
        }

        public void activate()
        {
            _action(this);
        }

        public bool hitTest(Vector2 point)
        {
            int pointX = (int)point.X;
            int pointY = (int)point.Y;

            if (pointX < x || pointX > x + _hitBoxWidth)
                return false;
            else if (pointY < y || pointY > y + _hitBoxHeight)
                return false;

            return true;
        }

        public void onSelect()
        {
            _selected = true;
        }

        public void onDeselect()
        {
            _selected = false;
        }

        public void UIUpdate()
        {
        }

        public void UIDraw()
        {
            Color color = _selected ? Color.White : _color;

            if (_selected)
            {
                Rectangle hitBox = new Rectangle(x, y, _hitBoxWidth, _hitBoxHeight);
                _spriteBatch.Draw(_pixel, hitBox, hitBox, Color.Red * 0.35f, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
            }

            if (_alignment == UIComponentAlignment.TopLeft)
            {
                _spriteBatch.DrawString(_font, _text, new Vector2(x, y), color);
            }
            else if (_alignment == UIComponentAlignment.TopCenter)
            {
                _spriteBatch.DrawString(_font, _text, new Vector2(x, y), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
