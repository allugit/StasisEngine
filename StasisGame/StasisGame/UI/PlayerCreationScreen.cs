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
        private bool _skipUpdate = true;

        public PlayerCreationScreen(LoderGame game) : base(game.screenSystem, ScreenType.PlayerCreation)
        {
            _game = game;
            _content = new ContentManager(game.Services, "Content");
            _titleFont = _content.Load<SpriteFont>("player_creation_screen/title_font");
            _letterFont = _content.Load<SpriteFont>("dialogue_font");
            _namePreview = new NamePreview(this, _letterFont, UIAlignment.MiddleCenter, -200, -122, _maxLetters);
            _nameInputPane = new NameInputPane(this, _letterFont, UIAlignment.MiddleCenter, -324, -60, 648, 320, _maxLetters);

            _title = new Label(
                _spriteBatch,
                _titleFont,
                UIAlignment.MiddleCenter,
                0,
                -180,
                TextAlignment.Center,
                "Please choose a name",
                1);

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
                    int playerSlot = DataManager.createPlayerData(_game.systemManager, _nameInputPane.name);
                    _game.closePlayerCreationScreen();
                    _game.loadGame(playerSlot);
                });
        }

        ~PlayerCreationScreen()
        {
            _content.Unload();
        }

        public override void applyIntroTransitions()
        {
            _nameInputPane.scale = 0f;
            _transitions.Clear();
            _transitions.Add(new ScaleTransition(_nameInputPane, 0f, 1f));
            base.applyIntroTransitions();
        }

        public override void applyOutroTransitions(Action onFinished = null)
        {
            _transitions.Clear();
            _transitions.Add(new ScaleTransition(_nameInputPane, 1f, 0f));
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
