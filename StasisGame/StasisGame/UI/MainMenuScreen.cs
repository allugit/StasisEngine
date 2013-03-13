using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class MainMenuScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _background;
        private Texture2D _logo;
        private ContentManager _content;

        public MainMenuScreen(LoderGame game)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _background = _content.Load<Texture2D>("main_menu_bg");
            _logo = _content.Load<Texture2D>("logo");
        }

        ~MainMenuScreen()
        {
            _content.Unload();
        }

        override public void update()
        {
            base.update();
        }

        override public void draw()
        {
            float scale = (float)_background.Height / (float)_game.GraphicsDevice.Viewport.Height;
            _game.spriteBatch.Draw(_background, Vector2.Zero, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _game.spriteBatch.Draw(_logo, new Vector2(_game.GraphicsDevice.Viewport.Width / 2f, 100f), _logo.Bounds, Color.White, 0, new Vector2(_logo.Width, _logo.Height) / 2, 0.75f, SpriteEffects.None, 0);

            base.draw();
        }
    }
}
