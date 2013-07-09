using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class Label
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private string _text;
        private int _xOffset;
        private int _yOffset;
        private UIAlignment _alignment;

        public bool selectable { get { return false; } }
        public float layerDepth { get { return 0f; } }
        public int x
        {
            get
            {
                if (_alignment == UIAlignment.TopCenter)
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

        public Label(SpriteBatch spriteBatch, SpriteFont font, string text, int xOffset, int yOffset, UIAlignment alignment)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _text = text;
            _xOffset = xOffset;
            _yOffset = yOffset;
            _alignment = alignment;
        }

        public void UIUpdate()
        {
        }

        public void UIDraw()
        {
            _spriteBatch.DrawString(_font, _text, new Vector2(x, y), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
