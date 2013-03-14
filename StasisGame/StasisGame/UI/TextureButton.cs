using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public enum TextureButtonAlignment
    {
        TopLeft,
        Center
    };

    public class TextureButton : IUIComponent
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _selectedTexture;
        private Texture2D _deselectedTexture;
        private bool _selected;
        private TextureButtonAlignment _alignment;
        private Texture2D _pixel;
        private Rectangle _destRect;
        private int _x;
        private int _y;
        private int _hitBoxOffsetX;
        private int _hitBoxOffsetY;
        private int _hitBoxWidth;
        private int _hitBoxHeight;
        private UIComponentAction _action;

        public bool selectable { get { return true; } }
        public float layerDepth { get { return 0f; } }

        public TextureButton(SpriteBatch spriteBatch, int x, int y, int hitBoxWidth, int hitBoxHeight, Texture2D selectedTexture, Texture2D deselectedTexture, TextureButtonAlignment alignment, UIComponentAction action)
        {
            _spriteBatch = spriteBatch;
            _selectedTexture = selectedTexture;
            _deselectedTexture = deselectedTexture;
            _alignment = alignment;
            _x = x;
            _y = y;
            _hitBoxWidth = hitBoxWidth;
            _hitBoxHeight = hitBoxHeight;
            _destRect = new Rectangle(_x, _y, 1, 1);
            _action = action;

            if (_alignment == TextureButtonAlignment.Center)
            {
                _hitBoxOffsetX = (int)(_hitBoxWidth / 2f);
                _hitBoxOffsetY = (int)(_hitBoxHeight / 2f);
            }

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
            Texture2D texture = _selected ? _selectedTexture : _deselectedTexture;
            _destRect.Width = texture.Width;
            _destRect.Height = texture.Height;

            _spriteBatch.Draw(texture, _destRect, texture.Bounds, Color.White, 0f, new Vector2((int)(texture.Width / 2f), (int)(texture.Height / 2f)), SpriteEffects.None, 0f);

            //Rectangle hitBox = new Rectangle(_x, _y, _hitBoxWidth, _hitBoxHeight);
            //_spriteBatch.Draw(_pixel, hitBox, hitBox, Color.Green * 0.5f, 0f, new Vector2(_hitBoxOffsetX, _hitBoxOffsetY), SpriteEffects.None, 0f);
        }
    }
}
