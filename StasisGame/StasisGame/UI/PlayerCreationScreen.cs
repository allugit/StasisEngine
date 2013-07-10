using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class PlayerCreationScreen : Screen
    {
        private LoderGame _game;
        private ContentManager _content;
        private Texture2D _logo;
        private SpriteFont _titleFont;
        private SpriteFont _letterFont;
        private Label _title;
        private NamePreview _namePreview;
        private NameInputPane _nameInputPane;
        private TextureButton _cancelButton;
        private TextureButton _createButton;
        private int _maxLetters = 13;
        private bool _skipUpdate = true;

        public PlayerCreationScreen(LoderGame game) : base(game.spriteBatch, ScreenType.PlayerCreation)
        {
            _game = game;
            _content = new ContentManager(game.Services, "Content");
            _logo = _content.Load<Texture2D>("logo");
            _titleFont = _content.Load<SpriteFont>("player_creation_screen/title_font");
            _letterFont = _content.Load<SpriteFont>("dialogue_font");
            _namePreview = new NamePreview(_spriteBatch, _letterFont, UIAlignment.MiddleCenter, -200, -122, _maxLetters);
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
                _spriteBatch,
                UIAlignment.MiddleCenter,
                24,
                240,
                _content.Load<Texture2D>("shared_ui/cancel_button_over"),
                _content.Load<Texture2D>("shared_ui/cancel_button"),
                new Rectangle(0, 0, 152, 33),
                () => { });

            _createButton = new TextureButton(
                _spriteBatch,
                UIAlignment.MiddleCenter,
                184,
                240,
                _content.Load<Texture2D>("player_creation_screen/create_button_over"),
                _content.Load<Texture2D>("player_creation_screen/create_button"),
                new Rectangle(0, 0, 152, 33),
                () => { });
        }

        ~PlayerCreationScreen()
        {
            _content.Unload();
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

        public override void update()
        {
            if (!_skipUpdate)
            {
                // Update input
                base.update();

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

            // Background renderer
            _game.menuBackgroundRenderer.draw();

            // Draw logo
            _spriteBatch.Draw(_logo, new Rectangle(_spriteBatch.GraphicsDevice.Viewport.Width - _logo.Width, 0, _logo.Width, _logo.Height), Color.White);

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
