using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class OptionsMenuScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _background;
        private Texture2D _logo;
        private ContentManager _content;

        public OptionsMenuScreen(LoderGame game) : base(ScreenType.OptionsMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _background = _content.Load<Texture2D>("main_menu/bg");
            _logo = _content.Load<Texture2D>("main_menu/logo");
        }

        ~OptionsMenuScreen()
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

            // Mouse input
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                if (_UIComponents[i].hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
                {
                    if (_oldMouseState.X - _newMouseState.X != 0 || _oldMouseState.Y - _newMouseState.Y != 0)
                        select(_UIComponents[i]);

                    if (_oldMouseState.LeftButton == ButtonState.Released && _newMouseState.LeftButton == ButtonState.Pressed)
                        _UIComponents[i].activate();
                }
            }

            // Gamepad input
            if (_newGamepadState.IsConnected)
            {
                bool movingUp = (_oldGamepadState.ThumbSticks.Left.Y < 0.25f && _newGamepadState.ThumbSticks.Left.Y > 0.25f) ||
                    (_oldGamepadState.DPad.Up == ButtonState.Released && _newGamepadState.DPad.Up == ButtonState.Pressed);
                bool movingDown = (_oldGamepadState.ThumbSticks.Left.Y > -0.25f && _newGamepadState.ThumbSticks.Left.Y < -0.25f) ||
                    (_oldGamepadState.DPad.Down == ButtonState.Released && _newGamepadState.DPad.Down == ButtonState.Pressed);
                bool activate = _oldGamepadState.Buttons.A == ButtonState.Released && _newGamepadState.Buttons.A == ButtonState.Pressed;

                if (movingUp)
                    selectPreviousComponent();
                else if (movingDown)
                    selectNextComponent();

                if (activate && _selectedIndex != -1)
                {
                    _UIComponents[_selectedIndex].activate();
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
