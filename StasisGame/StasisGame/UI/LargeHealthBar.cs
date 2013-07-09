using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class LargeHealthBar
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _healthBarOverlay;
        private Texture2D _healthBarEmpty;
        private Texture2D _healthBarFull;

        public bool selectable { get { return false; } }
        public float layerDepth { get { return 0f; } }

        public LargeHealthBar(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _healthBarOverlay = ResourceManager.getTexture("health_bar_overlay");
            _healthBarEmpty = ResourceManager.getTexture("health_bar_empty");
            _healthBarFull = ResourceManager.getTexture("health_bar_full");
        }

        public void UIUpdate()
        {
        }

        public void UIDraw()
        {
            _spriteBatch.Draw(_healthBarEmpty, new Rectangle(32, 32, _healthBarEmpty.Width, _healthBarEmpty.Height), Color.White);
            _spriteBatch.Draw(_healthBarOverlay, new Rectangle(32, 32, _healthBarOverlay.Width, _healthBarOverlay.Height), Color.White);
        }
    }
}
