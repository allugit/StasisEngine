using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;
using StasisGame.Managers;
using StasisGame.Data;

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
        private struct Resolution
        {
            public int width;
            public int height;
            public string text;
            public Resolution(int width, int height)
            {
                this.width = width;
                this.height = height;
                text = width.ToString() + " x " + height.ToString();
            }
        };

        private LoderGame _game;
        private Texture2D _logo;
        private ContentManager _content;
        private Pane _pane;
        private List<TextureButton> _generalButtons;
        private OptionsCategory _currentCategory;
        private SpriteFont _categoryTitleFont;
        private SpriteFont _optionsFont;
        private Texture2D _leftArrows;
        private Texture2D _rightArrows;
        private Texture2D _leftArrowsOver;
        private Texture2D _rightArrowsOver;
        private Color _optionsColor;
        private Label _videoTitle;
        private Label _resolutionLabel;
        private Label _resolutionValue;
        private TextureButton _resolutionPrevious;
        private TextureButton _resolutionNext;
        private Label _fullscreenLabel;
        private Label _fullscreenValue;
        private TextureButton _fullscreenPrevious;
        private TextureButton _fullscreenNext;
        private Label _audioTitle;
        private Label _controlsTitle;
        private List<Resolution> _availableResolutions;
        private Resolution _currentResolution;
        private bool _fullscreen;

        public OptionsMenuScreen(LoderGame game)
            : base(game.screenSystem, ScreenType.OptionsMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _logo = _content.Load<Texture2D>("logo");
            _categoryTitleFont = _content.Load<SpriteFont>("options_menu/category_title_font");
            _optionsFont = _content.Load<SpriteFont>("options_menu/options_font");
            _leftArrows = _content.Load<Texture2D>("shared_ui/left_arrows");
            _leftArrowsOver = _content.Load<Texture2D>("shared_ui/left_arrows_over");
            _rightArrows = _content.Load<Texture2D>("shared_ui/right_arrows");
            _rightArrowsOver = _content.Load<Texture2D>("shared_ui/right_arrows_over");
            _generalButtons = new List<TextureButton>();
            _optionsColor = new Color(0.8f, 0.8f, 0.8f);
            _availableResolutions = new List<Resolution>();

            loadSettings();
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
            Func<int> yOffset = () => { return categoryButtonOffset.Y + 81 * _generalButtons.Count; };

            _pane = new BluePane(
                this,
                UIAlignment.MiddleCenter,
                0,
                0,
                620,
                360);

            _generalButtons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/video_button_over"),
                _content.Load<Texture2D>("options_menu/video_button"),
                categoryButtonHitBox,
                () => { switchCategory(OptionsCategory.Video); }));

            _generalButtons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/audio_button_over"),
                _content.Load<Texture2D>("options_menu/audio_button"),
                categoryButtonHitBox,
                () => { switchCategory(OptionsCategory.Audio); }));

            _generalButtons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                categoryButtonOffset.X,
                yOffset(),
                _content.Load<Texture2D>("options_menu/controls_button_over"),
                _content.Load<Texture2D>("options_menu/controls_button"),
                categoryButtonHitBox,
                () => { switchCategory(OptionsCategory.Controls); }));

            _generalButtons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                170,
                158,
                _content.Load<Texture2D>("shared_ui/save_button_over"),
                _content.Load<Texture2D>("shared_ui/save_button"),
                confirmButtonHitBox,
                () =>
                {
                    saveSettings();
                    _game.closeOptionsMenu();
                    _game.openMainMenu();
                }));

            _generalButtons.Add(new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                6,
                158,
                _content.Load<Texture2D>("shared_ui/cancel_button_over"),
                _content.Load<Texture2D>("shared_ui/cancel_button"),
                confirmButtonHitBox,
                () =>
                {
                    _game.closeOptionsMenu();
                    _game.openMainMenu();
                }));
        }

        private void createVideoElements()
        {
            _videoTitle = new Label(
                this,
                _categoryTitleFont,
                UIAlignment.MiddleCenter,
                -255,
                -160,
                TextAlignment.Left,
                "Video",
                3);

            _resolutionLabel = new Label(
                this,
                _optionsFont,
                UIAlignment.MiddleCenter,
                -255,
                -80,
                TextAlignment.Left,
                "Resolution",
                2,
                _optionsColor);

            _resolutionValue = new Label(
                this,
                _optionsFont,
                UIAlignment.MiddleCenter,
                200,
                -80,
                TextAlignment.Center,
                _currentResolution.text,
                2,
                _optionsColor);

            _resolutionPrevious = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                120,
                -80,
                _leftArrowsOver,
                _leftArrows,
                _leftArrows.Bounds,
                () => { selectPreviousResolution(); });

            _resolutionNext = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                264,
                -80,
                _rightArrowsOver,
                _rightArrows,
                _rightArrows.Bounds,
                () => { selectNextResolution(); });

            _fullscreenLabel = new Label(
                this,
                _optionsFont,
                UIAlignment.MiddleCenter,
                -255,
                -40,
                TextAlignment.Left,
                "Fullscreen",
                2,
                _optionsColor);

            _fullscreenValue = new Label(
                this,
                _optionsFont,
                UIAlignment.MiddleCenter,
                200,
                -40,
                TextAlignment.Center,
                _fullscreen ? "True" : "False",
                2,
                _optionsColor);

            _fullscreenPrevious = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                120,
                -40,
                _leftArrowsOver,
                _leftArrows,
                _leftArrows.Bounds,
                () => { toggleFullscreen(); });

            _fullscreenNext = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                264,
                -40,
                _rightArrowsOver,
                _rightArrows,
                _rightArrows.Bounds,
                () => { toggleFullscreen(); });
        }

        private void createAudioElements()
        {
            _audioTitle = new Label(
                this,
                _categoryTitleFont,
                UIAlignment.MiddleCenter,
                -255,
                -160,
                TextAlignment.Left,
                "Audio",
                3);
        }

        private void createControlsElements()
        {
            _controlsTitle = new Label(
                this,
                _categoryTitleFont,
                UIAlignment.MiddleCenter,
                -255,
                -160,
                TextAlignment.Left,
                "Controls",
                3);
        }

        public override void applyIntroTransitions()
        {
            _generalButtons[0].translationX = -400;
            _generalButtons[1].translationX = -400;
            _generalButtons[2].translationX = -400;
            _generalButtons[3].translationX = _spriteBatch.GraphicsDevice.Viewport.Width;
            _generalButtons[4].translationX = _spriteBatch.GraphicsDevice.Viewport.Width;
            _transitions.Clear();
            _transitions.Add(new AlphaFadeTransition(_videoTitle, 0f, 1f, true, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_resolutionLabel, 0f, 1f, false, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_resolutionNext, 0f, 1f, false, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_resolutionPrevious, 0f, 1f, false, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_resolutionValue, 0f, 1f, false, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenLabel, 0f, 1f, false, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenNext, 0f, 1f, false, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenPrevious, 0f, 1f, false, 0.2f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenValue, 0f, 1f, false, 0.2f));
            _transitions.Add(new ScaleTransition(_pane, 0f, 1f, false));
            _transitions.Add(new TranslateTransition(_generalButtons[0], -400, 0, 0, 0, true, 0.2f));
            _transitions.Add(new TranslateTransition(_generalButtons[1], -400, 0, 0, 0, true, 0.2f));
            _transitions.Add(new TranslateTransition(_generalButtons[2], -400, 0, 0, 0, true, 0.2f));
            _transitions.Add(new TranslateTransition(_generalButtons[3], _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, true, 0.2f));
            _transitions.Add(new TranslateTransition(_generalButtons[4], _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, false, 0.2f));
            base.applyIntroTransitions();
        }

        public override void applyOutroTransitions(Action onFinished = null)
        {
            _transitions.Clear();
            _transitions.Add(new AlphaFadeTransition(_videoTitle, 1f, 0f, true, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_resolutionLabel, 1f, 0f, false, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_resolutionNext, 1f, 0f, false, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_resolutionPrevious, 1f, 0f, false, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_resolutionValue, 1f, 0f, false, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenLabel, 1f, 0f, false, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenNext, 1f, 0f, false, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenPrevious, 1f, 0f, false, 0.3f));
            _transitions.Add(new AlphaFadeTransition(_fullscreenValue, 1f, 0f, false, 0.3f));
            _transitions.Add(new TranslateTransition(_generalButtons[3], 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, false, 0.2f));
            _transitions.Add(new TranslateTransition(_generalButtons[4], 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, false, 0.2f));
            _transitions.Add(new TranslateTransition(_generalButtons[0], 0, 0, -400, 0, true, 0.3f));
            _transitions.Add(new TranslateTransition(_generalButtons[1], 0, 0, -400, 0, true, 0.3f));
            _transitions.Add(new TranslateTransition(_generalButtons[2], 0, 0, -400, 0, true, 0.3f));
            _transitions.Add(new ScaleTransition(_pane, 1f, 0f));
            base.applyOutroTransitions(onFinished);
        }

        private void loadSettings()
        {
            GameSettings settings = DataManager.gameSettings;
            bool addCurrentResolution = true;

            _currentResolution = new Resolution(settings.screenWidth, settings.screenHeight);
            _fullscreen = settings.fullscreen;

            // Populate available resolutions
            _availableResolutions.Add(new Resolution(800, 600));
            _availableResolutions.Add(new Resolution(1024, 768));
            _availableResolutions.Add(new Resolution(1280, 768));
            _availableResolutions.Add(new Resolution(1366, 768));
            _availableResolutions.Add(new Resolution(1440, 900));
            _availableResolutions.Add(new Resolution(1680, 1050));
            foreach (Resolution resolution in _availableResolutions)
            {
                if (resolution.width == _currentResolution.width && resolution.height == _currentResolution.height)
                {
                    addCurrentResolution = false;
                }
            }
            if (addCurrentResolution)
                _availableResolutions.Add(_currentResolution);
        }

        private void saveSettings()
        {
            GameSettings settings = DataManager.gameSettings;
            GraphicsDeviceManager graphics = _game.graphics;

            // Assign new settings
            settings.screenWidth = _currentResolution.width;
            settings.screenHeight = _currentResolution.height;
            settings.fullscreen = _fullscreen;
            
            // Apply settings
            _game.applyDisplaySettings();
            _game.menuBackgroundRenderer.recalculateScales();

            // Save settings
            DataManager.saveGameSettings();
        }

        private void switchCategory(OptionsCategory category)
        {
            _currentCategory = category;
        }

        private void hitTestButton(TextureButton button)
        {
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

        private void selectPreviousResolution()
        {
            int currentIndex = _availableResolutions.IndexOf(_currentResolution);

            if (currentIndex - 1 < 0)
                _currentResolution = _availableResolutions[_availableResolutions.Count - 1];
            else
                _currentResolution = _availableResolutions[currentIndex - 1];
        }

        private void selectNextResolution()
        {
            int currentIndex = _availableResolutions.IndexOf(_currentResolution);

            if (currentIndex + 1 > _availableResolutions.Count - 1)
                _currentResolution = _availableResolutions[0];
            else
                _currentResolution = _availableResolutions[currentIndex + 1];
        }

        private void toggleFullscreen()
        {
            _fullscreen = !_fullscreen;
        }

        private void updateVideoCategory()
        {
            _resolutionValue.text = _currentResolution.text;
            _fullscreenValue.text = _fullscreen ? "True" : "False";
            hitTestButton(_resolutionPrevious);
            hitTestButton(_resolutionNext);
            hitTestButton(_fullscreenPrevious);
            hitTestButton(_fullscreenNext);
        }

        private void updateAudioCategory()
        {
        }

        private void updateControlsCategory()
        {
        }

        override public void update()
        {
            // Update input
            base.update();

            if (!_skipUpdate)
            {
                // Handle button input
                for (int i = 0; i < _generalButtons.Count; i++)
                {
                    hitTestButton(_generalButtons[i]);
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
            }
            _skipUpdate = false;
        }

        private void drawVideoCategory()
        {
            _videoTitle.draw();
            _resolutionLabel.draw();
            _resolutionValue.draw();
            _resolutionPrevious.draw();
            _resolutionNext.draw();
            _fullscreenLabel.draw();
            _fullscreenValue.draw();
            _fullscreenPrevious.draw();
            _fullscreenNext.draw();
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

            // Draw logo
            _game.spriteBatch.Draw(_logo, new Vector2(_game.GraphicsDevice.Viewport.Width - _logo.Width, 0), _logo.Bounds, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            // Draw pane
            _pane.draw();

            // Draw buttons
            for (int i = 0; i < _generalButtons.Count; i++)
            {
                _generalButtons[i].draw();
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
