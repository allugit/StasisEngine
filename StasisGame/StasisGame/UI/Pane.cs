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
        private UIAlignment _alignment;
        protected int _x;
        protected int _y;
        protected int _width;
        protected int _height;
        protected Rectangle _destRect;
        private Rectangle _sourceRect;

        public float layerDepth { get { return 1f; } }
        public bool selectable { get { return false; } }

        // Constructor for default pane textures
        public Pane(Screen screen, UIAlignment alignment, int x, int y, int width, int height)
        {
            _screen = screen;
            _spriteBatch = screen.spriteBatch;
            _alignment = alignment;
            _x = x;
            _y = y;
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
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        virtual public void update()
        {
        }

        virtual public void draw()
        {
            int innerBorderWidth = _width - (_topLeftCorner.Width + _topRightCorner.Width);
            int innerBorderHeight = _height - (_topLeftCorner.Height + _bottomLeftCorner.Height);
            int backgroundPadding = 3;

            // Calculate rectangle
            switch (_alignment)
            {
                case UIAlignment.TopCenter:
                    _destRect.X = _x + (_spriteBatch.GraphicsDevice.Viewport.Width / 2) - (_width / 2);
                    _destRect.Y = _y;
                    _destRect.Width = _width;
                    _destRect.Height = _height;
                    _sourceRect.X = 0;
                    _sourceRect.Y = 0;
                    _sourceRect.Width = _destRect.Width - backgroundPadding * 2;
                    _sourceRect.Height = _destRect.Height - backgroundPadding * 2;
                    break;
            }

            // Background
            _spriteBatch.Draw(_background, new Rectangle(_destRect.X + backgroundPadding, _destRect.Y + backgroundPadding, _width - backgroundPadding * 2, _height - backgroundPadding * 2), _sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            // Sides
            _spriteBatch.Draw(_leftSide, new Rectangle(_destRect.X, _destRect.Y + _topLeftCorner.Height, _leftSide.Width, innerBorderHeight), new Rectangle(0, 0, _leftSide.Width, innerBorderHeight), Color.White);
            _spriteBatch.Draw(_topSide, new Rectangle(_destRect.X + _topLeftCorner.Width, _destRect.Y, innerBorderWidth, _topSide.Height), new Rectangle(0, 0, innerBorderWidth, _topSide.Height), Color.White);
            _spriteBatch.Draw(_rightSide, new Rectangle(_destRect.X + _width - _rightSide.Width, _destRect.Y + _topRightCorner.Height, _rightSide.Width, innerBorderHeight), new Rectangle(0, 0, _rightSide.Width, innerBorderHeight), Color.White);
            _spriteBatch.Draw(_bottomSide, new Rectangle(_destRect.X + _bottomRightCorner.Width, _destRect.Y + _height - _bottomSide.Height, innerBorderWidth, _bottomSide.Height), new Rectangle(0, 0, innerBorderWidth, _bottomSide.Height), Color.White);

            // Corners
            _spriteBatch.Draw(_topLeftCorner, new Rectangle(_destRect.X, _destRect.Y, _topLeftCorner.Width, _topLeftCorner.Height), Color.White);
            _spriteBatch.Draw(_topRightCorner, new Rectangle(_destRect.X + _width - _topRightCorner.Width, _destRect.Y, _topRightCorner.Width, _topRightCorner.Height), Color.White);
            _spriteBatch.Draw(_bottomRightCorner, new Rectangle(_destRect.X + _width - _bottomRightCorner.Width, _destRect.Y + _height - _bottomRightCorner.Height, _bottomRightCorner.Width, _bottomRightCorner.Height), Color.White);
            _spriteBatch.Draw(_bottomLeftCorner, new Rectangle(_destRect.X, _destRect.Y + _height - _bottomLeftCorner.Height, _bottomLeftCorner.Width, _bottomLeftCorner.Height), Color.White);
        }
    }
}
