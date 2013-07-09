using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public enum OptionsCategory
    {
        Video,
        Audio,
        Controls
    };

    public class OptionsMenuScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _logo;
        private ContentManager _content;
        private List<TextureButton> _buttons;
        private Texture2D _container;
        private OptionsCategory _currentCategory;
        private SpriteFont _categoryTitleFont;
        private SpriteFont _optionsFont;
        private Color _optionsColor;
        private Label _videoTitle;
        private Label _resolutionLabel;
        private Label _fullscreenLabel;
        private Label _audioTitle;
        private Label _controlsTitle;

        public OptionsMenuScreen(LoderGame game)
            : base(game.spriteBatch, ScreenType.OptionsMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _logo = _content.Load<Texture2D>("logo");
            _container = _content.Load<Texture2D>("options_menu/container");
            _categoryTitleFont = _content.Load<SpriteFont>("options_menu/category_title_font");
            _optionsFont = _content.Load<SpriteFont>("options_menu/options_font");
            _buttons = new List<TextureButton>();
            _optionsColor = new Color(0.8f, 0.8f, 0.8f);

            createInterfaceElements();
            createVideoElements();
            createAudioElements();
            createControlsElements();
        }

        ~OptionsMenuScreen()
        {
            _content.Unload();
        }

        private void createInterfaceElements()
        {
            Rectangle categoryButtonHitBox = new Rectangle(20, 0, 198, 68);
            Rectangle confirmButtonHitBox = new Rectangle(0, 0, 152, 33);
            Point categoryButtonOffset = new Point(-527, -160);
            Func<int> yOffset = () => { return categoryButtonOffset.Y + 81 * _buttons.Count; };

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/video_button_over"),
                _content.Load<Texture2D>("options_menu/video_button"),
                categoryButtonHitBox,
                () => { switchCategory(OptionsCategory.Video); }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/audio_button_over"),
                _content.Load<Texture2D>("options_menu/audio_button"),
                categoryButtonHitBox,
                () => { switchCategory(OptionsCategory.Audio); }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/controls_button_over"),
                _content.Load<Texture2D>("options_menu/controls_button"),
                categoryButtonHitBox,
                () => { switchCategory(OptionsCategory.Controls); }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                170,
                158,
                _content.Load<Texture2D>("options_menu/save_button_over"),
                _content.Load<Texture2D>("options_menu/save_button"),
                confirmButtonHitBox,
                () =>
                {
                    save();
                    _game.closeOptionsMenu();
                }));

            _buttons.Add(new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                6,
                158,
                _content.Load<Texture2D>("options_menu/cancel_button_over"),
                _content.Load<Texture2D>("options_menu/cancel_button"),
                confirmButtonHitBox,
                () => { _game.closeOptionsMenu(); }));
        }

        private void createVideoElements()
        {
            _videoTitle = new Label(
                _spriteBatch,
                _categoryTitleFont,
                -255,
                -160,
                UIAlignment.MiddleCenter,
                "Video");

            _resolutionLabel = new Label(
                _spriteBatch,
                _optionsFont,
                -255,
                -80,
                UIAlignment.MiddleCenter,
                "Resolution",
                _optionsColor);

            _fullscreenLabel = new Label(
                _spriteBatch,
                _optionsFont,
                -255,
                -40,
                UIAlignment.MiddleCenter,
                "Fullscreen",
                _optionsColor);
        }

        private void createAudioElements()
        {
            _audioTitle = new Label(
                _spriteBatch,
                _categoryTitleFont,
                -255,
                -160,
                UIAlignment.MiddleCenter,
                "Audio");
        }

        private void createControlsElements()
        {
            _controlsTitle = new Label(
                _spriteBatch,
                _categoryTitleFont,
                -255,
                -160,
                UIAlignment.MiddleCenter,
                "Controls");
        }

        private void save()
        {
            Console.WriteLine("TODO: Save...");
        }

        private void switchCategory(OptionsCategory category)
        {
            _currentCategory = category;
        }

        private void updateVideoCategory()
        {
        }

        private void updateAudioCategory()
        {
        }

        private void updateControlsCategory()
        {
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

            // Handle options category update
            if (_currentCategory == OptionsCategory.Video)
                updateVideoCategory();
            else if (_currentCategory == OptionsCategory.Audio)
                updateAudioCategory();
            else if (_currentCategory == OptionsCategory.Controls)
                updateControlsCategory();

            // Background
            _game.menuBackgroundRenderer.update(35f, _game.menuBackgroundScreenOffset);

            base.update();
        }

        private void drawVideoCategory()
        {
            _videoTitle.draw();
            _resolutionLabel.draw();
            _fullscreenLabel.draw();
        }

        private void drawAudioCategory()
        {
            _audioTitle.draw();
        }

        private void drawControlsCategory()
        {
            _controlsTitle.draw();
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

            // Handle options category drawing
            if (_currentCategory == OptionsCategory.Video)
                drawVideoCategory();
            else if (_currentCategory == OptionsCategory.Audio)
                drawAudioCategory();
            else if (_currentCategory == OptionsCategory.Controls)
                drawControlsCategory();

            base.draw();
        }
    }
}
