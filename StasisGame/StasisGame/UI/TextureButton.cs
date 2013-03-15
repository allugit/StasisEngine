using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class TextureButton : ISelectableUIComponent
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _selectedTexture;
        private Texture2D _deselectedTexture;
        private bool _selected;
        private UIComponentAlignment _alignment;
        private Texture2D _pixel;
        private int _xOffset;
        private int _yOffset;
        private int _hitBoxOffsetX;
        private int _hitBoxOffsetY;
        private int _hitBoxWidth;
        private int _hitBoxHeight;
        private UIComponentAction _action;

        public bool selectable { get { return true; } }
        public float layerDepth { get { return 0f; } }
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

        public TextureButton(SpriteBatch spriteBatch, int xOffset, int yOffset, int hitBoxWidth, int hitBoxHeight, Texture2D selectedTexture, Texture2D deselectedTexture, UIComponentAlignment alignment, UIComponentAction action)
        {
            _spriteBatch = spriteBatch;
            _selectedTexture = selectedTexture;
            _deselectedTexture = deselectedTexture;
            _alignment = alignment;
            _xOffset = xOffset;
            _yOffset = yOffset;
            _hitBoxWidth = hitBoxWidth;
            _hitBoxHeight = hitBoxHeight;
            _action = action;

            if (_alignment == UIComponentAlignment.TopCenter)
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

            if (pointX < x - _hitBoxOffsetX || pointX > -_hitBoxOffsetX + x + _hitBoxWidth)
                return false;
            else if (pointY < y - _hitBoxOffsetY || pointY > -_hitBoxOffsetY + y + _hitBoxHeight)
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

            if (_alignment == UIComponentAlignment.TopLeft)
                _spriteBatch.Draw(texture, new Rectangle(x, y, texture.Width, texture.Height), texture.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            else if (_alignment == UIComponentAlignment.TopCenter)
                _spriteBatch.Draw(texture, new Rectangle(x, y, texture.Width, texture.Height), texture.Bounds, Color.White, 0f, new Vector2((int)(texture.Width / 2f), (int)(texture.Height / 2f)), SpriteEffects.None, 0f);

            //Rectangle hitBox = new Rectangle(_x, _y, _hitBoxWidth, _hitBoxHeight);
            //_spriteBatch.Draw(_pixel, hitBox, hitBox, Color.Green * 0.5f, 0f, new Vector2(_hitBoxOffsetX, _hitBoxOffsetY), SpriteEffects.None, 0f);
        }
    }
}
