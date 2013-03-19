using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;

namespace StasisGame.UI
{
    public class ToolbarDisplay
    {
        private SpriteBatch _spriteBatch;
        private ToolbarComponent _toolbarComponent;
        private int _columnWidth = 4;
        private Vector2 _spacing = new Vector2(36, 36);
        private Texture2D _pixel;
        private int _selectedIndex = 0;
        private Rectangle _tileSize = new Rectangle(0, 0, 32, 32);
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;
        private MouseState _newMouseState;
        private MouseState _oldMouseState;
        private GamePadState _newGamepadState;
        private GamePadState _oldGamepadState;
        private bool _inFocus;

        public bool inFocus { get { return _inFocus; } set { _inFocus = value; } }

        public ToolbarDisplay(SpriteBatch spriteBatch, ToolbarComponent toolbarComponent)
        {
            _spriteBatch = spriteBatch;
            _toolbarComponent = toolbarComponent;
            _pixel = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
        }

        public void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newMouseState = Mouse.GetState();
            _newKeyState = Keyboard.GetState();

            if (_inFocus)
            {
                if (_newKeyState.IsKeyDown(Keys.D1) && _oldKeyState.IsKeyUp(Keys.D1))
                    _selectedIndex = 0;
                if (_newKeyState.IsKeyDown(Keys.D2) && _oldKeyState.IsKeyUp(Keys.D2))
                    _selectedIndex = 1;
                if (_newKeyState.IsKeyDown(Keys.D3) && _oldKeyState.IsKeyUp(Keys.D3))
                    _selectedIndex = 2;
                if (_newKeyState.IsKeyDown(Keys.D4) && _oldKeyState.IsKeyUp(Keys.D4))
                    _selectedIndex = 3;
            }
        }

        public void draw()
        {
            //Vector2 halfScreen = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height) / 2f;
            //Vector2 containerPosition = halfScreen - new Vector2(_columnWidth * _spacing.X, (float)Math.Floor((decimal)(_toolbarComponent.slots / _columnWidth)) * _spacing.Y) / 2f;

            //_spriteBatch.Draw(_level.inventoryBackground, halfScreen - new Vector2(_level.inventoryBackground.Width, _level.inventoryBackground.Height) / 2f + new Vector2(0, 18), Color.White);
            for (int i = 0; i < _toolbarComponent.slots; i++)
            {
                int x = i % _columnWidth;
                int y = (int)Math.Floor((decimal)(i / _columnWidth));
                Vector2 tilePosition = new Vector2(32, _spriteBatch.GraphicsDevice.Viewport.Height - (_spacing.Y + 32)) + _spacing * new Vector2(x, y) + new Vector2(2, 2);
                ItemComponent itemComponent = _toolbarComponent.getItem(i);

                if (_selectedIndex == i)
                {
                    _spriteBatch.Draw(_pixel, tilePosition, _tileSize, Color.Yellow);
                    _spriteBatch.Draw(_pixel, tilePosition + new Vector2(2, 2), new Rectangle(0, 0, _tileSize.Width - 4, _tileSize.Height - 4), Color.Black);
                }
                else
                {
                    _spriteBatch.Draw(_pixel, tilePosition, _tileSize, Color.Black);
                }

                if (itemComponent != null)
                    _spriteBatch.Draw(itemComponent.inventoryTexture, new Rectangle((int)tilePosition.X + 2, (int)tilePosition.Y + 2, 28, 28), Color.White);
            }
        }
    }
}
