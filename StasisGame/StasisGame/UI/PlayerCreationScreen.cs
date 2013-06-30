using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class PlayerCreationScreen : Screen
    {
        private LoderGame _game;
        private SpriteBatch _spriteBatch;
        private SpriteFont _titleFont;
        private SpriteFont _letterFont;
        private Vector2 _titleSize;
        private string _title;
        private NamePreview _namePreview;
        private NameInputPane _nameInputPane;
        private int _maxLetters = 13;

        public PlayerCreationScreen(LoderGame game) : base(ScreenType.PlayerCreation)
        {
            _game = game;
            _spriteBatch = game.spriteBatch;
            _titleFont = game.Content.Load<SpriteFont>("player_creation_screen/title_font");
            _letterFont = game.Content.Load<SpriteFont>("dialogue_font");
            _title = "Please choose a name";
            _titleSize = _titleFont.MeasureString(_title);
            _namePreview = new NamePreview(_spriteBatch, _letterFont, UIComponentAlignment.TopCenter, 0, 64, _maxLetters);
            _nameInputPane = new NameInputPane(_spriteBatch, _letterFont, UIComponentAlignment.TopCenter, 0, 120, _maxLetters);

            _UIComponents.Add(_namePreview);
            _UIComponents.Add(_nameInputPane);
        }

        public override void update()
        {
            base.update();

            _namePreview.name = _nameInputPane.name;
        }

        public override void draw()
        {
            Vector2 titlePosition = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, 32) / 2f - new Vector2(_titleSize.X / 2f, 0);

            // Title
            _spriteBatch.DrawString(_titleFont, "Please choose a name", titlePosition, Color.White);

            // Draw components
            base.draw();
        }
    }
}
