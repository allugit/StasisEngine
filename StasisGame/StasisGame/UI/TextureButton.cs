using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class TextureButton : ITranslatable
    {
        protected Screen _screen;
        protected SpriteBatch _spriteBatch;
        private Texture2D _selectedTexture;
        private Texture2D _deselectedTexture;
        private bool _selected;
        private UIAlignment _alignment;
        private Texture2D _pixel;
        private int _xOffset;
        private int _yOffset;
        private Rectangle _localHitBox;
        private Action _onActivate;
        private Action _onMouseOver;
        private Action _onMouseOut;
        private float _translationX;
        private float _translationY;

        public bool selected { get { return _selected; } }
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

        public TextureButton(Screen screen, SpriteBatch spriteBatch, UIAlignment alignment, int x, int y, Texture2D selectedTexture, Texture2D deselectedTexture, Rectangle localHitBox, Action onActivate) :
            this(screen, spriteBatch, alignment, x, y, selectedTexture, deselectedTexture, localHitBox, onActivate, null, null)
        {
            _onMouseOver = () => { select(); };
            _onMouseOut = () => { deselect(); };
        }

        public TextureButton(Screen screen, SpriteBatch spriteBatch, UIAlignment alignment, int x, int y, Texture2D selectedTexture, Texture2D deselectedTexture, Rectangle localHitBox, Action onActivate, Action onMouseOver, Action onMouseOut)
        {
            _screen = screen;
            _spriteBatch = spriteBatch;
            _alignment = alignment;
            _xOffset = x;
            _yOffset = y;
            _selectedTexture = selectedTexture;
            _deselectedTexture = deselectedTexture;
            _localHitBox = localHitBox;
            _onActivate = onActivate;
            _onMouseOver = onMouseOver;
            _onMouseOut = onMouseOut;

            _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
        }

        public void activate()
        {
            _onActivate();
        }

        virtual public void select()
        {
            _selected = true;
        }

        virtual public void deselect()
        {
            _selected = false;
        }

        public void mouseOut()
        {
            _onMouseOut();
        }

        public void mouseOver()
        {
            _onMouseOver();
        }

        public bool hitTest(Vector2 point)
        {
            Rectangle pointRect = new Rectangle((int)point.X, (int)point.Y, 1, 1);
            Rectangle screenHitBox = new Rectangle(_localHitBox.X + x, _localHitBox.Y + y, _localHitBox.Width, _localHitBox.Height);
            return screenHitBox.Intersects(pointRect);
        }

        virtual public void draw()
        {
            Texture2D texture = _selected ? _selectedTexture : _deselectedTexture;
            
            _spriteBatch.Draw(
                texture,
                new Rectangle(x + (int)_screen.translationX + (int)_translationX, y + (int)_screen.translationY + (int)_translationY, texture.Width, texture.Height),
                texture.Bounds,
                Color.White,
                0f,
                Vector2.Zero, 
                SpriteEffects.None,
                0f);

            //Rectangle hitBox = new Rectangle(_x, _y, _hitBoxWidth, _hitBoxHeight);
            //_spriteBatch.Draw(_pixel, hitBox, hitBox, Color.Green * 0.5f, 0f, new Vector2(_hitBoxOffsetX, _hitBoxOffsetY), SpriteEffects.None, 0f);
        }
    }
}
