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
        private ContentManager _content;
        private int _textureIndex;

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
        }

        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            Texture2D texture = _textures[_textureIndex];

            _spriteBatch.Draw(texture, texture.Bounds, texture.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);

            base.draw();
        }
    }
}
