using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace StasisGame.UI
{
    public class WorldMapScreen : Screen
    {
        private struct PositionTexture
        {
            public Vector2 position;
            public Texture2D texture;
            public PositionTexture(Vector2 position, Texture2D texture)
            {
                this.position = position;
                this.texture = texture;
            }
        };

        private LoderGame _game;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private List<PositionTexture> _textures;
        private int _worldWidth = 8400;
        private int _worldHeight = 5250;
        private Matrix _view;
        private float _scale;

        public WorldMapScreen(LoderGame game) : base(ScreenType.WorldMap)
        {
            _game = game;
            _spriteBatch = _game.spriteBatch;
            _textures = new List<PositionTexture>();
            _content = new ContentManager(_game.Services);
            _content.RootDirectory = "Content";
            _scale = 0.05f;
            int widthSegments = (int)Math.Ceiling((float)_worldWidth / 2048f);
            int heightSegments = (int)Math.Ceiling((float)_worldHeight / 2048f);

            int count = 0;
            for (int j = 0; j < heightSegments; j++)
            {
                for (int i = 0; i < widthSegments; i++)
                {
                    _textures.Add(new PositionTexture(new Vector2(2048f * i, 2048f * j), _content.Load<Texture2D>("world_map\\" + count.ToString())));
                    count++;
                }
            }
        }

        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            _view =
                Matrix.CreateTranslation(new Vector3(-_worldWidth, -_worldHeight, 0) / 2f) *
                Matrix.CreateScale(_scale) *
                Matrix.CreateTranslation(new Vector3(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height, 0) / 2f);

            for (int i = 0; i < _textures.Count; i++)
            {
                Vector2 position = Vector2.Transform(_textures[i].position, _view);
                _spriteBatch.Draw(_textures[i].texture, position, _textures[i].texture.Bounds, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }

            base.draw();
        }
    }
}
