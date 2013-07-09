using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class PlayerCreationScreen : Screen
    {
        private LoderGame _game;
        private SpriteFont _titleFont;
        private SpriteFont _letterFont;
        private Vector2 _titleSize;
        private string _title;
        private NamePreview _namePreview;
        private NameInputPane _nameInputPane;
        private TextureButton _cancelButton;
        private Texture2D _cancelButtonTexture;
        private Texture2D _cancelButtonOverTexture;
        private int _maxLetters = 13;

        public PlayerCreationScreen(LoderGame game) : base(game.spriteBatch, ScreenType.PlayerCreation)
        {
            _game = game;
            _titleFont = game.Content.Load<SpriteFont>("player_creation_screen/title_font");
            _letterFont = game.Content.Load<SpriteFont>("dialogue_font");
            _cancelButtonTexture = game.Content.Load<Texture2D>("player_creation_screen/cancel_button");
            _cancelButtonOverTexture = game.Content.Load<Texture2D>("player_creation_screen/cancel_button_over");
            _title = "Please choose a name";
            _titleSize = _titleFont.MeasureString(_title);
            _namePreview = new NamePreview(_spriteBatch, _letterFont, UIAlignment.TopCenter, 0, 64, _maxLetters);
            _nameInputPane = new NameInputPane(this, _letterFont, UIAlignment.TopCenter, 0, 120, _maxLetters);
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
            Vector2 titlePosition = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, 32) / 2f - new Vector2(_titleSize.X / 2f, 0);

            // Background renderer
            _game.menuBackgroundRenderer.draw();

            // Title
            _spriteBatch.DrawString(_titleFont, "Please choose a name", titlePosition, Color.White);

            // Draw name input preview and pane
            _nameInputPane.draw();
            _namePreview.draw();
        }
    }
}
