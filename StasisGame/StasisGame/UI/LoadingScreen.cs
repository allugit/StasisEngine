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
                _textureIndex = (int)(((float)_elementsLoaded / (float)_elementsToLoad) * (float)(_textures.Count - 1));
                _elementsLoaded = value;
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
                UIAlignment.TopLeft,
                32,
                80,
                TextAlignment.Left,
                "Loading level data...",
                2);
            _message.layerDepth = 0.1f;
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

            base.draw();
        }
    }
}
