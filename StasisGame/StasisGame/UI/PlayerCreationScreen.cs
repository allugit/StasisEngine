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
        private int _maxLetters = 13;

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
                "Please choose a name");
        }

        ~PlayerCreationScreen()
        {
            _content.Unload();
        }

        public override void update()
        {
            // Update components
            _nameInputPane.update();

            // Copy name from input pane to preview component
            _namePreview.name = _nameInputPane.name;

            // Background renderer
            _game.menuBackgroundRenderer.update(35f, _game.menuBackgroundScreenOffset);

            base.update();
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
        }
    }
}
