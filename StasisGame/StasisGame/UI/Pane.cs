using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class Pane
    {
        protected Screen _screen;
        protected SpriteBatch _spriteBatch;
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

        // Constructor for default pane textures
        public Pane(Screen screen, UIAlignment alignment, int x, int y, int width, int height)
        {
            _screen = screen;
            _spriteBatch = screen.spriteBatch;
            _alignment = alignment;
            _xOffset = x;
            _yOffset = y;
            _width = width;
            _height = height;
            _topLeftCorner = ResourceManager.getTexture("pane_top_left_corner");
            _topRightCorner = ResourceManager.getTexture("pane_top_right_corner");
            _bottomRightCorner = ResourceManager.getTexture("pane_bottom_right_corner");
            _bottomLeftCorner = ResourceManager.getTexture("pane_bottom_left_corner");
            _leftSide = ResourceManager.getTexture("pane_left_side");
            _topSide = ResourceManager.getTexture("pane_top_side");
            _rightSide = ResourceManager.getTexture("pane_right_side");
            _bottomSide = ResourceManager.getTexture("pane_bottom_side");
            _background = ResourceManager.getTexture("pane_background");
        }

        // Constructor for custom pane textures
        public Pane(
            SpriteBatch spriteBatch,
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
            _spriteBatch = spriteBatch;
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
            int x = this.x;     // prevent redundant calculation... only needs to be done once per frame
            int y = this.y;
            int backgroundMargin = 3;

            // Draw logo

            // Draw background
            _spriteBatch.Draw(
                _background,
                new Rectangle(x + backgroundMargin, y + backgroundMargin, _width - backgroundMargin * 2,_height - backgroundMargin * 2),
                new Rectangle(0, 0, _width - backgroundMargin * 2, _height - backgroundMargin * 2),
                Color.White);

            // Draw corners
            _spriteBatch.Draw(
                _topLeftCorner,
                new Rectangle(x, y, _topLeftCorner.Width, _topLeftCorner.Height),
                Color.White);

            _spriteBatch.Draw(
                _topRightCorner,
                new Rectangle(x + _width - _topRightCorner.Width, y, _topRightCorner.Width, _topRightCorner.Height),
                Color.White);

            _spriteBatch.Draw(
                _bottomRightCorner,
                new Rectangle(x + _width - _bottomRightCorner.Width, y + _height - _bottomRightCorner.Height, _bottomRightCorner.Width, _bottomRightCorner.Height),
                Color.White);

            _spriteBatch.Draw(
                _bottomLeftCorner,
                new Rectangle(x, y + _height - _bottomLeftCorner.Height, _bottomLeftCorner.Width, _bottomLeftCorner.Height),
                Color.White);

            // Draw sides
            int leftSideHeight = _height - (_topLeftCorner.Height + _bottomLeftCorner.Height);
            int rightSideHeight = _height - (_topRightCorner.Height + _bottomRightCorner.Height);
            int topSideWidth = _width - (_topLeftCorner.Width + _topRightCorner.Width);
            int bottomSideWidth = _width - (_bottomLeftCorner.Width + _bottomRightCorner.Width);

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
                new Rectangle(x + _width - _rightSide.Width, y + _topRightCorner.Height, _rightSide.Width, rightSideHeight),
                new Rectangle(0, 0, _rightSide.Width, rightSideHeight),
                Color.White);

            _spriteBatch.Draw(
                _bottomSide,
                new Rectangle(x + _bottomLeftCorner.Width, y + _height - _bottomSide.Height, bottomSideWidth, _bottomSide.Height),
                new Rectangle(0, 0, bottomSideWidth, _bottomSide.Height),
                Color.White);
        }
    }
}
