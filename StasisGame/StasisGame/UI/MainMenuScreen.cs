using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
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
            _background = _content.Load<Texture2D>("main_menu/bg");
            _logo = _content.Load<Texture2D>("main_menu/logo");

            TextureButton newGameButton = new TextureButton(
                _game.spriteBatch,
                (int)(_game.GraphicsDevice.Viewport.Width / 2f),
                256,
                475,
                80,
                _content.Load<Texture2D>("main_menu/new_game_selected"),
                _content.Load<Texture2D>("main_menu/new_game_unselected"),
                TextureButtonAlignment.Center);

            TextureButton loadGameButton = new TextureButton(
                _game.spriteBatch,
                (int)(_game.GraphicsDevice.Viewport.Width / 2f),
                358,
                475,
                80,
                _content.Load<Texture2D>("main_menu/load_game_selected"),
                _content.Load<Texture2D>("main_menu/load_game_unselected"),
                TextureButtonAlignment.Center);

            TextureButton optionsButton = new TextureButton(
                _game.spriteBatch,
                (int)(_game.GraphicsDevice.Viewport.Width / 2f),
                465,
                475,
                80,
                _content.Load<Texture2D>("main_menu/options_selected"),
                _content.Load<Texture2D>("main_menu/options_unselected"),
                TextureButtonAlignment.Center);

            TextureButton exitButton = new TextureButton(
                _game.spriteBatch,
                (int)(_game.GraphicsDevice.Viewport.Width / 2f + 100f),
                565,
                240, 
                80,
                _content.Load<Texture2D>("main_menu/exit_selected"),
                _content.Load<Texture2D>("main_menu/exit_unselected"),
                TextureButtonAlignment.Center);

            addComponent(newGameButton);
            addComponent(loadGameButton);
            addComponent(optionsButton);
            addComponent(exitButton);
        }

        ~MainMenuScreen()
        {
            _content.Unload();
        }

        override public void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            for (int i = 0; i < _UIComponents.Count; i++)
            {
                if (_UIComponents[i].hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
                {
                    select(_UIComponents[i]);
                }
            }

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
