using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Managers;
using StasisGame.Systems;
using StasisGame.Components;

namespace StasisGame.UI
{
    public class LevelScreen : Screen
    {
        private LoderGame _game;
        private Level _level;
        private Texture2D _pixel;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private SpriteBatch _spriteBatch;
        private bool _displayInventory;
        private int _playerId;

        public LevelScreen(LoderGame game, Level level)
            : base(ScreenType.Level)
        {
            _game = game;
            _level = level;
            _systemManager = _level.systemManager;
            _entityManager = _level.entityManager;
            _spriteBatch = _game.spriteBatch;
            _playerId = (_systemManager.getSystem(SystemType.Player) as PlayerSystem).playerId;
            _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });

            _UIComponents.Add(new LargeHealthBar(_game.spriteBatch));
        }

        public override void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newMouseState = Mouse.GetState();
            _newKeyState = Keyboard.GetState();
            _newGamepadState = GamePad.GetState(PlayerIndex.One);

            if (_newKeyState.IsKeyDown(Keys.I) && _oldKeyState.IsKeyUp(Keys.I))
                _displayInventory = !_displayInventory;

            if (_newGamepadState.Buttons.Y == ButtonState.Pressed && _oldGamepadState.Buttons.Y == ButtonState.Released)
                _displayInventory = !_displayInventory;

            base.update();
        }

        public override void draw()
        {
            if (_displayInventory)
            {
                InventoryComponent inventoryComponent = (InventoryComponent)_entityManager.getComponent(_playerId, ComponentType.Inventory);
                Vector2 halfScreen = new Vector2(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height) / 2f;
                int columnWidth = 4;
                Vector2 spacing = new Vector2(36, 36);
                Vector2 containerPosition = halfScreen - new Vector2(columnWidth * spacing.X, (float)Math.Floor((decimal)(inventoryComponent.slots / columnWidth)) * spacing.Y) / 2f;
                Rectangle tileSize = new Rectangle(0, 0, 32, 32);

                _spriteBatch.Draw(_level.inventoryBackground, halfScreen - new Vector2(_level.inventoryBackground.Width, _level.inventoryBackground.Height) / 2f + new Vector2(0, 18), Color.White);
                for (int i = 0; i < inventoryComponent.slots; i++)
                {
                    int x = i % columnWidth;
                    int y = (int)Math.Floor((decimal)(i / columnWidth));
                    Vector2 tilePosition = containerPosition + spacing * new Vector2(x, y) + new Vector2(2, 2);
                    ItemComponent itemComponent = inventoryComponent.getItem(i);

                    _spriteBatch.Draw(_pixel, tilePosition, tileSize, Color.Black);

                    if (itemComponent != null)
                        _spriteBatch.Draw(itemComponent.inventoryTexture, new Rectangle((int)tilePosition.X + 2, (int)tilePosition.Y + 2, 28, 28), Color.White);
                }
            }

            base.draw();
        }
    }
}
