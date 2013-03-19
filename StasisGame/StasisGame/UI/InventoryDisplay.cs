﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;

namespace StasisGame.UI
{
    public class InventoryDisplay
    {
        private SpriteBatch _spriteBatch;
        private InventoryComponent _inventoryComponent;
        private Texture2D _pixel;
        private int _columnWidth = 5;
        private Vector2 _spacing = new Vector2(36, 36);
        private Rectangle _tileSize = new Rectangle(0, 0, 32, 32);
        private bool _inFocus;
        private int _selectedIndex = 0;
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;
        private MouseState _newMouseState;
        private MouseState _oldMouseState;
        private GamePadState _newGamepadState;
        private GamePadState _oldGamepadState;

        public InventoryComponent inventoryComponent { get { return _inventoryComponent; } set { _inventoryComponent = value; } }
        public bool inFocus { get { return _inFocus; } set { _inFocus = value; } }

        public InventoryDisplay(SpriteBatch spriteBatch, InventoryComponent inventoryComponent)
        {
            _spriteBatch = spriteBatch;
            _inventoryComponent = inventoryComponent;
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
                Vector2 halfScreen = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height) / 2f;
                Vector2 containerPosition = halfScreen - new Vector2(_columnWidth * _spacing.X, (float)Math.Floor((decimal)(_inventoryComponent.slots / _columnWidth)) * _spacing.Y) / 2f;
                Vector2 mouse = new Vector2(_newMouseState.X, _newMouseState.Y);
                bool mouseMoved = _newMouseState.X - _oldMouseState.X != 0 || _newMouseState.Y - _oldMouseState.Y != 0;

                if (_newGamepadState.IsConnected)
                {
                    bool movingDown = (_oldGamepadState.ThumbSticks.Right.Y < 0.15f && _newGamepadState.ThumbSticks.Right.Y > 0.15f);
                    bool movingUp = (_oldGamepadState.ThumbSticks.Right.Y > -0.15f && _newGamepadState.ThumbSticks.Right.Y < -0.15f);
                    bool movingLeft = (_oldGamepadState.ThumbSticks.Right.X > -0.15f && _newGamepadState.ThumbSticks.Right.X < -0.15f);
                    bool movingRight = (_oldGamepadState.ThumbSticks.Right.X < 0.15f && _newGamepadState.ThumbSticks.Right.X > 0.15f);

                    if (movingUp)
                    {
                        if (_selectedIndex + _columnWidth < _inventoryComponent.slots)
                            _selectedIndex += _columnWidth;
                    }
                    if (movingDown)
                    {
                        if (_selectedIndex - _columnWidth >= 0)
                            _selectedIndex -= _columnWidth;
                    }
                    if (movingLeft)
                    {
                        if (_selectedIndex % _columnWidth != 0)
                            _selectedIndex--;
                    }
                    if (movingRight)
                    {
                        if (_selectedIndex % _columnWidth != _columnWidth - 1)
                            _selectedIndex++;
                    }

                    _selectedIndex = Math.Min(_inventoryComponent.slots - 1, Math.Max(0, _selectedIndex));
                }

                // Mouse input
                if (mouseMoved)
                {
                    for (int i = 0; i < _inventoryComponent.slots; i++)
                    {
                        int x = i % _columnWidth;
                        int y = (int)Math.Floor((decimal)(i / _columnWidth));
                        Vector2 tilePosition = containerPosition + _spacing * new Vector2(x, y) + new Vector2(2, 2);

                        if (mouse.X >= tilePosition.X && mouse.X < tilePosition.X + _tileSize.Width &&
                            mouse.Y >= tilePosition.Y && mouse.Y < tilePosition.Y + _tileSize.Height)
                        {
                            _selectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        public void draw()
        {
            Vector2 halfScreen = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height) / 2f;
            Vector2 containerPosition = halfScreen - new Vector2(_columnWidth * _spacing.X, (float)Math.Floor((decimal)(_inventoryComponent.slots / _columnWidth)) * _spacing.Y) / 2f;

            //_spriteBatch.Draw(_level.inventoryBackground, halfScreen - new Vector2(_level.inventoryBackground.Width, _level.inventoryBackground.Height) / 2f + new Vector2(0, 18), Color.White);
            for (int i = 0; i < _inventoryComponent.slots; i++)
            {
                int x = i % _columnWidth;
                int y = (int)Math.Floor((decimal)(i / _columnWidth));
                Vector2 tilePosition = containerPosition + _spacing * new Vector2(x, y) + new Vector2(2, 2);
                ItemComponent itemComponent = _inventoryComponent.getItem(i);

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