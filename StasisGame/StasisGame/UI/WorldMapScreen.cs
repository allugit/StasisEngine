using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using StasisCore;

namespace StasisGame.UI
{
    public class WorldMapScreen : Screen
    {
        private LoderGame _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private float _scale;
        private Vector2 _currentScreenCenter;
        private Vector2 _targetScreenCenter;
        private Vector2 _topLeft;
        private Vector2 _bottomRight;
        private Vector2 _halfTextureSize;
        private Texture2D _pixel;

        public WorldMapScreen(LoderGame game) : base(ScreenType.WorldMap)
        {
            _game = game;
            _spriteBatch = _game.spriteBatch;
            _scale = 1f;

            _texture = ResourceManager.getTexture("world_map");
            _pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
            _halfTextureSize = new Vector2(_texture.Width, _texture.Height) / 2f;
            _topLeft = Vector2.Zero;
            _bottomRight = new Vector2(_texture.Width, _texture.Height) - new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);
        }

        public override void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            if (_newGamepadState.IsConnected)
            {
                _targetScreenCenter += _newGamepadState.ThumbSticks.Left * 7 * new Vector2(1, -1);
                _targetScreenCenter += _newGamepadState.ThumbSticks.Right * 7 * new Vector2(1, -1);

                _scale = Math.Max(0.5f, _scale - _newGamepadState.Triggers.Left / 500f);
                _scale = Math.Min(1f, _scale + _newGamepadState.Triggers.Right / 500f);
            }

            if (_newKeyState.IsKeyDown(Keys.Left) || _newKeyState.IsKeyDown(Keys.A))
                _targetScreenCenter += new Vector2(7, 0);
            if (_newKeyState.IsKeyDown(Keys.Right) || _newKeyState.IsKeyDown(Keys.D))
                _targetScreenCenter += new Vector2(-7, 0);
            if (_newKeyState.IsKeyDown(Keys.Up) || _newKeyState.IsKeyDown(Keys.W))
                _targetScreenCenter += new Vector2(0, 7);
            if (_newKeyState.IsKeyDown(Keys.Down) || _newKeyState.IsKeyDown(Keys.S))
                _targetScreenCenter += new Vector2(0, -7);

            _targetScreenCenter = Vector2.Max(_topLeft, Vector2.Min(_bottomRight, _targetScreenCenter));
            _currentScreenCenter += (_targetScreenCenter - _currentScreenCenter) / 11f;

            base.update();
        }

        public override void draw()
        {
            _spriteBatch.Draw(_texture, -_currentScreenCenter, _texture.Bounds, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            _spriteBatch.Draw(_pixel, Vector2.Zero, new Rectangle(0, 0, 25, 25), Color.Red);

            base.draw();
        }
    }
}
