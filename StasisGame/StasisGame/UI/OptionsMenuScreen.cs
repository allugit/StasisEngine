using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class OptionsMenuScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _logo;
        private ContentManager _content;
        private List<TextureButton> _buttons;
        private Texture2D _container;

        public OptionsMenuScreen(LoderGame game)
            : base(game.spriteBatch, ScreenType.OptionsMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _logo = _content.Load<Texture2D>("logo");
            _container = _content.Load<Texture2D>("options_menu/container");
            _buttons = new List<TextureButton>();

            Rectangle localHitBox = new Rectangle(20, 0, 198, 68);
            Point categoryButtonOffset = new Point(-527, -160);
            Func<int> yOffset = () => { return categoryButtonOffset.Y + 81 * _buttons.Count; };

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/video_button_over"),
                _content.Load<Texture2D>("options_menu/video_button"),
                localHitBox,
                () => { }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/audio_button_over"),
                _content.Load<Texture2D>("options_menu/audio_button"),
                localHitBox,
                () => { }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/controls_button_over"),
                _content.Load<Texture2D>("options_menu/controls_button"),
                localHitBox,
                () => { }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                170,
                158,
                _content.Load<Texture2D>("options_menu/save_button_over"),
                _content.Load<Texture2D>("options_menu/save_button"),
                localHitBox,
                () => { }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                6,
                158,
                _content.Load<Texture2D>("options_menu/cancel_button_over"),
                _content.Load<Texture2D>("options_menu/cancel_button"),
                localHitBox,
                () => { }));
        }

        ~OptionsMenuScreen()
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
            // Draw background
            _game.menuBackgroundRenderer.draw();
            _game.spriteBatch.Draw(_logo, new Vector2(_game.GraphicsDevice.Viewport.Width - _logo.Width, 0), _logo.Bounds, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            // Draw container
            Vector2 containerPosition = new Vector2((int)(_spriteBatch.GraphicsDevice.Viewport.Width / 2f), (int)(_spriteBatch.GraphicsDevice.Viewport.Height / 2f));
            _spriteBatch.Draw(_container, containerPosition, _container.Bounds, Color.White, 0f, new Vector2((int)(_container.Width / 2f) , (int)(_container.Height / 2f)), 1f, SpriteEffects.None, 0.1f);

            // Draw buttons
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].draw();
            }

            base.draw();
        }
    }
}
