using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class ProgressBar
    {
        private Screen _screen;
        private SpriteBatch _spriteBatch;
        private UIAlignment _alignment;
        private int _xOffset;
        private int _yOffset;
        private Texture2D _background;
        private Texture2D _fill;
        private Texture2D _border;
        private float _progress;
        private float _layerDepth = 0f;

        public float progress { get { return _progress; } set { _progress = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
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

        public ProgressBar(Screen screen, UIAlignment alignment, int x, int y)
        {
            _screen = screen;
            _spriteBatch = screen.screenSystem.spriteBatch;
            _alignment = alignment;
            _xOffset = x;
            _yOffset = y;
            _background = ResourceManager.getTexture("progress_bar_background");
            _fill = ResourceManager.getTexture("progress_bar_fill");
            _border = ResourceManager.getTexture("progress_bar_border");
        }

        public void draw()
        {
            int x = this.x;
            int y = this.y;
            int progressWidth = (int)Math.Ceiling((float)_fill.Width * _progress);

            _spriteBatch.Draw(_background, new Rectangle(x, y, _background.Width, _background.Height), _background.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth + 0.0002f);
            _spriteBatch.Draw(_fill, new Rectangle(x, y, progressWidth, _fill.Height), new Rectangle(0, 0, progressWidth, _fill.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth + 0.0001f);
            _spriteBatch.Draw(_border, new Rectangle(x, y, _border.Width, _border.Height), _border.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth);
        }
    }
}
