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
        private InventoryDisplay _inventoryDisplay;
        private ToolbarDisplay _toolbarDisplay;

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
            _inventoryDisplay = new InventoryDisplay(_game.spriteBatch, (InventoryComponent)_entityManager.getComponent(_playerId, ComponentType.Inventory));
            _inventoryDisplay.inFocus = true;
            _toolbarDisplay = new ToolbarDisplay(_game.spriteBatch, (ToolbarComponent)_entityManager.getComponent(_playerId, ComponentType.Toolbar));

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

            if (_displayInventory)
            {
                _inventoryDisplay.update();
            }
            _toolbarDisplay.update();

            base.update();
        }

        public override void draw()
        {
            if (_displayInventory)
            {
                _inventoryDisplay.draw();
            }
            _toolbarDisplay.draw();

            base.draw();
        }
    }
}
