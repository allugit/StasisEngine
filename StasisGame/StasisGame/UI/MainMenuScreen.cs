using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;
using StasisCore;

namespace StasisGame.UI
{
    public class MainMenuScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _buttonsBackground;
        private ContentManager _content;
        private List<TextureButton> _buttons;

        public MainMenuScreen(LoderGame game) : base(game.screenSystem, ScreenType.MainMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _buttonsBackground = _content.Load<Texture2D>("main_menu/buttons_background");
            _buttons = new List<TextureButton>();

            Func<int> yOffset = () => { return 182 + 81 * _buttons.Count; };
            Rectangle localHitBox = new Rectangle(20, 0, 198, 68);
            _buttons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.TopLeft,
                20,
                yOffset(),
                _content.Load<Texture2D>("main_menu/new_game_over"),
                _content.Load<Texture2D>("main_menu/new_game"),
                localHitBox,
                () => 
                {
                    Logger.log("New game button clicked.");
                    _game.closeMainMenu();
                    _game.openPlayerCreationScreen();
                }));

            _buttons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.TopLeft,
                20,
                yOffset(),
                _content.Load<Texture2D>("main_menu/load_game_over"),
                _content.Load<Texture2D>("main_menu/load_game"),
                localHitBox,
                () =>
                {
                    Logger.log("Load game button clicked.");
                    _game.closeMainMenu();
                    _game.openLoadGameMenu();
                }));

            _buttons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.TopLeft,
                20,
                yOffset(),
                _content.Load<Texture2D>("main_menu/options_over"),
                _content.Load<Texture2D>("main_menu/options"),
                localHitBox,
                () => 
                {
                    Logger.log("Options button clicked.");
                    _game.closeMainMenu();
                    _game.openOptionsMenu();
                }));

            _buttons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.TopLeft,
                20,
                yOffset(),
                _content.Load<Texture2D>("main_menu/quit_game_over"),
                _content.Load<Texture2D>("main_menu/quit_game"),
                localHitBox,
                () => { _game.Exit(); }));
        }

        ~MainMenuScreen()
        {
            _content.Unload();
        }

        override public void update()
        {
            base.update();

            // Handle button input
            for (int i = 0; i < _buttons.Count; i++)
            {
                TextureButton button = _buttons[i];

                if (button.hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
                {
                    button.mouseOver();

                    if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                        button.activate();
                }
                else if (button.selected)
                {
                    button.mouseOut();
                }
            }

            // Background
            _game.menuBackgroundRenderer.update(35f, _game.menuBackgroundScreenOffset);

            base.update();
        }

        override public void draw()
        {
            // Draw button background
            _game.spriteBatch.Draw(_buttonsBackground, new Vector2(0 + slideX, 100 + slideY), _buttonsBackground.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Draw buttons
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].draw();
            }

            base.draw();
        }
    }
}
