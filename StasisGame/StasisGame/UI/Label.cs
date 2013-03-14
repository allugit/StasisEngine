using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class Label : IUIComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private string _text;
        private int _x;
        private int _y;

        public bool selectable { get { return false; } }
        public float layerDepth { get { return 0f; } }

        public Label(SpriteBatch spriteBatch, SpriteFont font, string text, int x, int y)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _text = text;
            _x = x;
            _y = y;
        }

        public void UIUpdate()
        {
        }

        public void UIDraw()
        {
            _spriteBatch.DrawString(_font, _text, new Vector2(_x, _y), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
