using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class UIComponentContainer : IUIComponent
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private int _x;
        private int _y;
        private Rectangle _destRect;
        private UIComponentAlignment _alignment;

        public bool selectable { get { return false; } }
        public float layerDepth { get { return 0f; } }

        public UIComponentContainer(SpriteBatch spriteBatch, Texture2D texture, int x, int y, UIComponentAlignment alignment)
        {
            _spriteBatch = spriteBatch;
            _texture = texture;
            _x = x;
            _y = y;
            _destRect = new Rectangle(_x, _y, _texture.Width, _texture.Height);
            _alignment = alignment;
        }

        public void UIUpdate()
        {
        }

        public void UIDraw()
        {
            if (_alignment == UIComponentAlignment.TopLeft)
                _spriteBatch.Draw(_texture, _destRect, _texture.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            else if (_alignment == UIComponentAlignment.TopCenter)
                _spriteBatch.Draw(_texture, _destRect, _texture.Bounds, Color.White, 0f, new Vector2((int)(_texture.Width / 2f), 0), SpriteEffects.None, 0f);
        }
    }
}
