﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private LevelSystem _levelSystem;
        private ContentManager _content;
        private Texture2D _pixel;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private SpriteBatch _spriteBatch;
        private bool _displayInventory;
        private int _playerId;
        private InventoryDisplay _inventoryDisplay;
        private ToolbarDisplay _toolbarDisplay;
        private EquipmentSystem _equipmentSystem;
        private SpriteFont _arial;

        public LevelScreen(LoderGame game, SystemManager systemManager, EntityManager entityManager)
            : base(ScreenType.Level)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _levelSystem = (LevelSystem)_systemManager.getSystem(SystemType.Level);
            _spriteBatch = _game.spriteBatch;
            _content = new ContentManager(_game.Services);
            _content.RootDirectory = "Content";
            _equipmentSystem = (EquipmentSystem)_systemManager.getSystem(SystemType.Equipment);
            _playerId = (_systemManager.getSystem(SystemType.Player) as PlayerSystem).playerId;
            _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
            _arial = _content.Load<SpriteFont>("arial");

            ToolbarComponent toolbarComponent = (ToolbarComponent)_entityManager.getComponent(_playerId, ComponentType.Toolbar);

            _toolbarDisplay = new ToolbarDisplay(_game.spriteBatch, _equipmentSystem, toolbarComponent);
            _inventoryDisplay = new InventoryDisplay(_game.spriteBatch, _equipmentSystem, (InventoryComponent)_entityManager.getComponent(_playerId, ComponentType.Inventory), toolbarComponent);
            _inventoryDisplay.inFocus = false;
            _toolbarDisplay.inFocus = true;

            _UIComponents.Add(new LargeHealthBar(_game.spriteBatch));
        }

        ~LevelScreen()
        {
            _content.Unload();
        }

        public override void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newMouseState = Mouse.GetState();
            _newKeyState = Keyboard.GetState();
            _newGamepadState = GamePad.GetState(PlayerIndex.One);

            if ((_newKeyState.IsKeyDown(Keys.I) && _oldKeyState.IsKeyUp(Keys.I)) ||
                (_newGamepadState.Buttons.Y == ButtonState.Pressed && _oldGamepadState.Buttons.Y == ButtonState.Released))
            {
                _displayInventory = !_displayInventory;

                if (_displayInventory)
                {
                    _inventoryDisplay.inFocus = true;
                    _toolbarDisplay.inFocus = false;
                }
                else
                {
                    _inventoryDisplay.inFocus = false;
                    _toolbarDisplay.inFocus = true;
                }
            }

            if (_displayInventory)
            {
                _inventoryDisplay.update();
            }
            _toolbarDisplay.update();

            base.update();
        }

        public override void draw()
        {
            int playerId = (_systemManager.getSystem(SystemType.Player) as PlayerSystem).playerId;
            PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(playerId, ComponentType.Physics);
            //string text = string.Format("Body count: {0}", physicsComponent.body.GetWorld().BodyCount);

            if (_displayInventory)
            {
                _inventoryDisplay.draw();
            }
            _toolbarDisplay.draw();

            //_spriteBatch.DrawString(_arial, text, new Vector2(8, 8), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            //_spriteBatch.DrawString(_arial, text, new Vector2(9, 9), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00001f);

            base.draw();
        }
    }
}
