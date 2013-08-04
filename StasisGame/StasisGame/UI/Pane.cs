using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    abstract public class Pane : IScalable
    {
        protected SpriteBatch _spriteBatch;
        protected Screen _screen;
        private Texture2D _topLeftCorner;
        private Texture2D _topRightCorner;
        private Texture2D _bottomRightCorner;
        private Texture2D _bottomLeftCorner;
        private Texture2D _leftSide;
        private Texture2D _topSide;
        private Texture2D _rightSide;
        private Texture2D _bottomSide;
        private Texture2D _background;
        protected UIAlignment _alignment;
        protected int _xOffset;
        protected int _yOffset;
        protected int _width;
        protected int _height;
        protected float _scale = 1f;

        public float scale { get { return _scale; } set { _scale = value; } }
        public Screen screen { get { return _screen; } }
        public int x
        {
            get
            {
                int halfWidth = (int)(((float)_width * _scale) / 2f);
                if (_alignment == UIAlignment.TopLeft || _alignment == UIAlignment.MiddleLeft || _alignment == UIAlignment.BottomLeft)
                    return _xOffset - halfWidth;
                else if (_alignment == UIAlignment.TopCenter || _alignment == UIAlignment.MiddleCenter || _alignment == UIAlignment.BottomCenter)
                    return _xOffset - halfWidth + (int)(_spriteBatch.GraphicsDevice.Viewport.Width / 2f);
                else if (_alignment == UIAlignment.TopRight || _alignment == UIAlignment.MiddleRight || _alignment == UIAlignment.BottomRight)
                    return _xOffset - halfWidth + _spriteBatch.GraphicsDevice.Viewport.Width;

                return _xOffset - halfWidth;
            }
        }
        public int y
        {
            get
            {
                int halfHeight = (int)(((float)_height * _scale) / 2f);
                if (_alignment == UIAlignment.TopLeft || _alignment == UIAlignment.TopCenter || _alignment == UIAlignment.TopRight)
                    return _yOffset - halfHeight;
                else if (_alignment == UIAlignment.MiddleLeft || _alignment == UIAlignment.MiddleCenter || _alignment == UIAlignment.MiddleRight)
                    return _yOffset - halfHeight + (int)(_spriteBatch.GraphicsDevice.Viewport.Height / 2f);
                else if (_alignment == UIAlignment.BottomLeft || _alignment == UIAlignment.BottomCenter || _alignment == UIAlignment.BottomRight)
                    return _yOffset - halfHeight + _spriteBatch.GraphicsDevice.Viewport.Height;

                return _yOffset - halfHeight;
            }
        }

        // Constructor for custom pane textures
        public Pane(
            Screen screen,
            Texture2D topLeftCorner,
            Texture2D topRightCorner,
            Texture2D bottomRightCorner,
            Texture2D bottomLeftCorner,
            Texture2D leftSide,
            Texture2D topSide,
            Texture2D rightSide,
            Texture2D bottomSide,
            Texture2D background,
            UIAlignment alignment,
            int x,
            int y,
            int width,
            int height)
        {
            _screen = screen;
            _spriteBatch = _screen.screenSystem.spriteBatch;
            _topLeftCorner = topLeftCorner;
            _topRightCorner = topRightCorner;
            _bottomRightCorner = bottomRightCorner;
            _bottomLeftCorner = bottomLeftCorner;
            _leftSide = leftSide;
            _topSide = topSide;
            _rightSide = rightSide;
            _bottomSide = bottomSide;
            _background = background;
            _alignment = alignment;
            _xOffset = x;
            _yOffset = y;
            _width = width;
            _height = height;
        }

        virtual public void update()
        {
        }

        virtual public void draw()
        {
            int x = this.x + (int)_screen.translationX;     // prevent redundant calculation... only needs to be done once per frame
            int y = this.y + (int)_screen.translationY;
            int backgroundMargin = 3;

            // Draw background
            _spriteBatch.Draw(
                _background,
                new Rectangle(x + backgroundMargin, y + backgroundMargin, (int)(_width * _scale) - backgroundMargin * 2, (int)(_height * _scale) - backgroundMargin * 2),
                new Rectangle(0, 0, _width - backgroundMargin * 2, _height - backgroundMargin * 2),
                Color.White);

            // Draw corners
            _spriteBatch.Draw(
                _topLeftCorner,
                new Rectangle(x, y, _topLeftCorner.Width, _topLeftCorner.Height),
                Color.White);

            _spriteBatch.Draw(
                _topRightCorner,
                new Rectangle(x + (int)(_width * _scale) - _topRightCorner.Width, y, _topRightCorner.Width, _topRightCorner.Height),
                Color.White);

            _spriteBatch.Draw(
                _bottomRightCorner,
                new Rectangle(x + (int)(_width * _scale) - _bottomRightCorner.Width, y + (int)(_height * _scale) - _bottomRightCorner.Height, _bottomRightCorner.Width, _bottomRightCorner.Height),
                Color.White);

            _spriteBatch.Draw(
                _bottomLeftCorner,
                new Rectangle(x, y + (int)(_height * _scale) - _bottomLeftCorner.Height, _bottomLeftCorner.Width, _bottomLeftCorner.Height),
                Color.White);

            // Draw sides
            int leftSideHeight = (int)(_height * _scale) - (_topLeftCorner.Height + _bottomLeftCorner.Height);
            int rightSideHeight = (int)(_height * _scale) - (_topRightCorner.Height + _bottomRightCorner.Height);
            int topSideWidth = (int)(_width * _scale) - (_topLeftCorner.Width + _topRightCorner.Width);
            int bottomSideWidth = (int)(_width * _scale) - (_bottomLeftCorner.Width + _bottomRightCorner.Width);

            _spriteBatch.Draw(
                _leftSide,
                new Rectangle(x, y + _topLeftCorner.Height, _leftSide.Width, leftSideHeight),
                new Rectangle(0, 0, _leftSide.Width, leftSideHeight),
                Color.White);

            _spriteBatch.Draw(
                _topSide,
                new Rectangle(x + _topLeftCorner.Width, y, topSideWidth, _topSide.Height),
                new Rectangle(0, 0, topSideWidth, _topSide.Height),
                Color.White);

            _spriteBatch.Draw(
                _rightSide,
                new Rectangle(x + (int)(_width * _scale) - _rightSide.Width, y + _topRightCorner.Height, _rightSide.Width, rightSideHeight),
                new Rectangle(0, 0, _rightSide.Width, rightSideHeight),
                Color.White);

            _spriteBatch.Draw(
                _bottomSide,
                new Rectangle(x + _bottomLeftCorner.Width, y + (int)(_height * _scale) - _bottomSide.Height, bottomSideWidth, _bottomSide.Height),
                new Rectangle(0, 0, bottomSideWidth, _bottomSide.Height),
                Color.White);
        }
    }
}
