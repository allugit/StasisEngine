using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame
{
    public class MainMenu
    {
        private LoderGame _game;
        private Texture2D _logoBackground;
        private Texture2D _logo;

        public MainMenu(LoderGame game)
        {
            _game = game;
            _logoBackground = _game.Content.Load<Texture2D>("logo_bg");
            _logo = _game.Content.Load<Texture2D>("logo");
        }

        public void update(GameTime gameTime)
        {
        }

        public void draw(GameTime gameTime)
        {
            _game.spriteBatch.Draw(_logoBackground, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.White);
            _game.spriteBatch.Draw(_logo, new Vector2(_game.GraphicsDevice.Viewport.Width / 2f, 100f), _logo.Bounds, Color.White, 0, new Vector2(_logo.Width, _logo.Height) / 2, 0.75f, SpriteEffects.None, 0);
        }
    }
}
