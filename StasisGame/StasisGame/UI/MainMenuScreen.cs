using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class MainMenuScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _background;
        private Texture2D _logo;
        private ContentManager _content;

        public MainMenuScreen(LoderGame game) : base(ScreenType.MainMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _background = _content.Load<Texture2D>("main_menu/bg");
            _logo = _content.Load<Texture2D>("main_menu/logo");

            TextureButton newGameButton = new TextureButton(
                _game.spriteBatch,
                0,
                256,
                475,
                80,
                _content.Load<Texture2D>("main_menu/new_game_selected"),
                _content.Load<Texture2D>("main_menu/new_game_unselected"),
                UIComponentAlignment.TopCenter,
                (component) => { _game.newGame(); });
            
            TextureButton loadGameButton = new TextureButton(
                _game.spriteBatch,
                0,
                358,
                475,
                80,
                _content.Load<Texture2D>("main_menu/load_game_selected"),
                _content.Load<Texture2D>("main_menu/load_game_unselected"),
                UIComponentAlignment.TopCenter,
                (component) => { _game.openLoadGameMenu(); });
            
            TextureButton optionsButton = new TextureButton(
                _game.spriteBatch,
                0,
                465,
                475,
                80,
                _content.Load<Texture2D>("main_menu/options_selected"),
                _content.Load<Texture2D>("main_menu/options_unselected"),
                UIComponentAlignment.TopCenter,
                (component) => { _game.openOptionsMenu(); });

            TextureButton exitButton = new TextureButton(
                _game.spriteBatch,
                100,
                565,
                240,
                80,
                _content.Load<Texture2D>("main_menu/exit_selected"),
                _content.Load<Texture2D>("main_menu/exit_unselected"),
                UIComponentAlignment.TopCenter,
                (component) => { _game.Exit(); });
            
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

            // Mouse input
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                IUIComponent component = _UIComponents[i];

                if (component.selectable)
                {
                    ISelectableUIComponent selectableComponent = component as ISelectableUIComponent;
                    if (selectableComponent.hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
                    {
                        if (_oldMouseState.X - _newMouseState.X != 0 || _oldMouseState.Y - _newMouseState.Y != 0)
                            select(selectableComponent);

                        if (_oldMouseState.LeftButton == ButtonState.Released && _newMouseState.LeftButton == ButtonState.Pressed)
                            selectableComponent.activate();
                    }
                }
            }

            // Gamepad input
            if (InputSystem.usingGamepad)
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

                if (activate && _selectedComponent != null)
                {
                    _selectedComponent.activate();
                }
            }

            base.update();
        }

        override public void draw()
        {
            float scale = (float)_game.GraphicsDevice.Viewport.Height / (float)_background.Height;
            _game.spriteBatch.Draw(_background, Vector2.Zero, _background.Bounds, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _game.spriteBatch.Draw(_logo, new Vector2(_game.GraphicsDevice.Viewport.Width / 2f, 100f), _logo.Bounds, Color.White, 0, new Vector2(_logo.Width, _logo.Height) / 2, 0.75f, SpriteEffects.None, 0);

            base.draw();
        }
    }
}
