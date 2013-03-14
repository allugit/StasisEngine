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
        private int _x;
        private int _y;
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

                if (_alignment == UIComponentAlignment.Center)
                {
                    _hitBoxOffsetX = (int)(_hitBoxWidth / 2f);
                    _hitBoxOffsetY = (int)(_hitBoxHeight / 2f);
                }
            }
        }

        public TextButton(SpriteBatch spriteBatch, SpriteFont font, Color color, int x, int y, string text, UIComponentAlignment alignment, UIComponentAction action)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _color = color;
            _alignment = alignment;
            _x = x;
            _y = y;
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

            if (pointX < _x - _hitBoxOffsetX || pointX > -_hitBoxOffsetX + _x + _hitBoxWidth)
                return false;
            else if (pointY < _y - _hitBoxOffsetY || pointY > -_hitBoxOffsetY + _y + _hitBoxHeight)
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
                Rectangle hitBox = new Rectangle(_x, _y, _hitBoxWidth, _hitBoxHeight);
                _spriteBatch.Draw(_pixel, hitBox, hitBox, Color.Red * 0.5f, 0f, new Vector2(_hitBoxOffsetX, _hitBoxOffsetY), SpriteEffects.None, 0.1f);
            }

            if (_alignment == UIComponentAlignment.TopLeft)
            {
                _spriteBatch.DrawString(_font, _text, new Vector2(_x, _y), color);
            }
            else if (_alignment == UIComponentAlignment.Center)
            {
                _spriteBatch.DrawString(_font, _text, new Vector2(_x, _y), color, 0f, new Vector2(_hitBoxWidth, _hitBoxHeight) / 2f, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
