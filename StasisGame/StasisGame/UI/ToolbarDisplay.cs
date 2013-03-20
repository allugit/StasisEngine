using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class ToolbarDisplay
    {
        private SpriteBatch _spriteBatch;
        private ToolbarComponent _toolbarComponent;
        private int _columnWidth = 4;
        private Vector2 _spacing = new Vector2(36, 36);
        private Texture2D _pixel;
        private Rectangle _tileSize = new Rectangle(0, 0, 32, 32);
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;
        private MouseState _newMouseState;
        private MouseState _oldMouseState;
        private GamePadState _newGamepadState;
        private GamePadState _oldGamepadState;
        private EquipmentSystem _equipmentSystem;
        private bool _inFocus;

        public bool inFocus { get { return _inFocus; } set { _inFocus = value; } }

        public ToolbarDisplay(SpriteBatch spriteBatch, EquipmentSystem equipmentSystem, ToolbarComponent toolbarComponent)
        {
            _spriteBatch = spriteBatch;
            _toolbarComponent = toolbarComponent;
            _equipmentSystem = equipmentSystem;
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
                    _equipmentSystem.selectToolbarSlot(_toolbarComponent, 0);
                if (_newKeyState.IsKeyDown(Keys.D2) && _oldKeyState.IsKeyUp(Keys.D2))
                    _equipmentSystem.selectToolbarSlot(_toolbarComponent, 1);
                if (_newKeyState.IsKeyDown(Keys.D3) && _oldKeyState.IsKeyUp(Keys.D3))
                    _equipmentSystem.selectToolbarSlot(_toolbarComponent, 2);
                if (_newKeyState.IsKeyDown(Keys.D4) && _oldKeyState.IsKeyUp(Keys.D4))
                    _equipmentSystem.selectToolbarSlot(_toolbarComponent, 3);
            }
        }

        public void draw()
        {
            for (int i = 0; i < _toolbarComponent.slots; i++)
            {
                int x = i % _columnWidth;
                int y = (int)Math.Floor((decimal)(i / _columnWidth));
                Vector2 tilePosition = new Vector2(32, _spriteBatch.GraphicsDevice.Viewport.Height - (_spacing.Y + 32)) + _spacing * new Vector2(x, y) + new Vector2(2, 2);
                ItemComponent itemComponent = _toolbarComponent.inventory[i];
                bool selected = i == _toolbarComponent.selectedIndex;

                if (selected)
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
