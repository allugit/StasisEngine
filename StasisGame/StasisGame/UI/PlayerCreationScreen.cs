using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisGame.Managers;

namespace StasisGame.UI
{
    public class PlayerCreationScreen : Screen
    {
        private LoderGame _game;
        private ContentManager _content;
        private SpriteFont _titleFont;
        private SpriteFont _letterFont;
        private Label _title;
        private NamePreview _namePreview;
        private NameInputPane _nameInputPane;
        private TextureButton _cancelButton;
        private TextureButton _createButton;
        private int _maxLetters = 13;

        public PlayerCreationScreen(LoderGame game) : base(game.screenSystem, ScreenType.PlayerCreation)
        {
            _game = game;
            _content = new ContentManager(game.Services, "Content");
            _titleFont = _content.Load<SpriteFont>("player_creation_screen/title_font");
            _letterFont = _content.Load<SpriteFont>("shared_ui/dialogue_font");
            _namePreview = new NamePreview(this, _letterFont, UIAlignment.MiddleCenter, -200, -122, _maxLetters);
            _nameInputPane = new NameInputPane(this, _letterFont, UIAlignment.MiddleCenter, 0, 98, 648, 320, _maxLetters);

            _title = new Label(
                this,
                _titleFont,
                UIAlignment.MiddleCenter,
                0,
                -180,
                TextAlignment.Center,
                "Please choose a name",
                2);

            _cancelButton = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                24,
                240,
                _content.Load<Texture2D>("shared_ui/cancel_button_over"),
                _content.Load<Texture2D>("shared_ui/cancel_button"),
                new Rectangle(0, 0, 152, 33),
                () => 
                {
                    _game.closePlayerCreationScreen();
                    _game.openMainMenu();
                });

            _createButton = new TextureButton(
                this,
                _spriteBatch,
                UIAlignment.MiddleCenter,
                184,
                240,
                _content.Load<Texture2D>("player_creation_screen/create_button_over"),
                _content.Load<Texture2D>("player_creation_screen/create_button"),
                new Rectangle(0, 0, 152, 33),
                () =>
                {
                    _game.closePlayerCreationScreen();
                    _screenSystem.addTransition(new ScreenFadeOutTransition(this, Color.Black, true, 0.05f, null, () =>
                    {
                        int playerSlot;
                        _game.startPersistentSystems();
                        _game.playerSystem.createPlayer();
                        playerSlot = DataManager.createPlayerData(_nameInputPane.name);
                        DataManager.loadPlayerData(playerSlot);
                        _game.loadLevel("dagny_house");
                        //_game.openWorldMap();
                    }));
                });
        }

        ~PlayerCreationScreen()
        {
            _content.Unload();
        }

        public void resetName()
        {
            _nameInputPane.reset();
            _namePreview.name = _nameInputPane.name;
        }

        public override void applyIntroTransitions()
        {
            _nameInputPane.scale = 0f;
            _title.alpha = 0f;
            _cancelButton.translationX = _spriteBatch.GraphicsDevice.Viewport.Width;
            _createButton.translationX = _spriteBatch.GraphicsDevice.Viewport.Width;
            _transitions.Clear();
            _transitions.Add(new ScaleTransition(_nameInputPane, 0f, 1f));
            _transitions.Add(new TranslateTransition(_cancelButton, _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, false));
            _transitions.Add(new TranslateTransition(_createButton, _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, false));
            _transitions.Add(new TranslateTransition(_namePreview, _spriteBatch.GraphicsDevice.Viewport.Width, 0, 0, 0, false));
            _transitions.Add(new AlphaFadeTransition(_title, 0f, 1f, false));
            for (int i = 0; i < _nameInputPane.letterButtons.Count; i++)
            {
                _nameInputPane.letterButtons[i].alpha = 0f;
                _transitions.Add(new AlphaFadeTransition(_nameInputPane.letterButtons[i], 0f, 1f, false, 0.05f));
            }
            base.applyIntroTransitions();
        }

        public override void applyOutroTransitions(Action onFinished = null)
        {
            _transitions.Clear();
            _transitions.Add(new ScaleTransition(_nameInputPane, 1f, 0f));
            _transitions.Add(new TranslateTransition(_cancelButton, 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, false));
            _transitions.Add(new TranslateTransition(_createButton, 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, false));
            _transitions.Add(new TranslateTransition(_namePreview, 0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, 0, false));
            _transitions.Add(new AlphaFadeTransition(_title, 1f, 0f, false));
            for (int i = 0; i < _nameInputPane.letterButtons.Count; i++)
            {
                _transitions.Add(new AlphaFadeTransition(_nameInputPane.letterButtons[i], 1f, 0f, false, 0.2f));
            }
            base.applyOutroTransitions(onFinished);
        }

        private void hitTestButton(TextureButton button)
        {
            if (button.hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
            {
                button.mouseOver();

                if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                {
                    button.activate();
                }
            }
            else if (button.selected)
            {
                button.mouseOut();
            }
        }

        public override void update()
        {
            // Update input
            base.update();

            if (!_skipUpdate)
            {
                // Update components
                _nameInputPane.update();

                // Copy name from input pane to preview component
                _namePreview.name = _nameInputPane.name;

                // Hit test buttons
                hitTestButton(_cancelButton);
                hitTestButton(_createButton);

                // Background renderer
                _game.menuBackgroundRenderer.update(35f, _game.menuBackgroundScreenOffset);
            }
            _skipUpdate = false;
        }

        public override void draw()
        {
            // Draw title
            _title.draw();

            // Draw name input preview and pane
            _nameInputPane.draw();
            _namePreview.draw();

            // Draw cancel and create buttons
            _cancelButton.draw();
            _createButton.draw();
        }
    }
}
