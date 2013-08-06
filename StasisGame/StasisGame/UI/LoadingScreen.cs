using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class LoadingScreen : Screen
    {
        private List<Texture2D> _textures;
        private Texture2D _background;
        private ContentManager _content;
        private int _textureIndex;
        private Label _title;
        private Label _message;
        private float _progress;
        private ProgressBar _progressBar;
        private SpriteFont _titleFont;
        private SpriteFont _messageFont;
        private int _elementsToLoad;
        private int _elementsLoaded;

        public string message { get { return _message.text; } set { _message.text = value; } }
        public int elementsToLoad { get { return _elementsToLoad; } set { _elementsToLoad = value; } }
        public int elementsLoaded {
            get { return _elementsLoaded; } 
            set 
            {
                _elementsLoaded = value;
                _progress = (float)_elementsLoaded / (float)_elementsToLoad;
                _progressBar.progress = _progress;
                _textureIndex = (int)(_progress * (float)(_textures.Count - 1));
            } 
        }

        public LoadingScreen(LoderGame game)
            : base(game.screenSystem, ScreenType.Loading)
        {
            _textures = new List<Texture2D>();
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            for (int i = 0; i < 10; i++)
            {
                _textures.Add(_content.Load<Texture2D>(string.Format("loading_screen/gear_{0}", i)));
            }
            _background = _content.Load<Texture2D>("loading_screen/background");
            _titleFont = _content.Load<SpriteFont>("loading_screen/title");
            _messageFont = _content.Load<SpriteFont>("loading_screen/message");

            _title = new Label(
                this,
                _titleFont,
                UIAlignment.TopLeft,
                32,
                32,
                TextAlignment.Left,
                "Loading",
                4);
            _title.layerDepth = 0.1f;

            _message = new Label(
                this,
                _messageFont,
                UIAlignment.BottomRight,
                -50,
                -90,
                TextAlignment.Right,
                "Loading level data...",
                2);
            _message.layerDepth = 0.1f;

            _progressBar = new ProgressBar(
                this,
                UIAlignment.BottomRight,
                -590,
                -64);
            _progressBar.layerDepth = 0.1f;
        }

        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            Texture2D texture = _textures[_textureIndex];

            _spriteBatch.Draw(_background, _background.Bounds, _background.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.11f);
            _spriteBatch.Draw(texture, new Rectangle(0, -110, texture.Width, texture.Height), texture.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);

            _title.draw();
            _message.draw();
            _progressBar.draw();

            base.draw();
        }
    }
}
