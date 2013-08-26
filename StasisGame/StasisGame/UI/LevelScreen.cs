using System;
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
        private bool _displayInventory;
        private int _playerId;
        private InventoryDisplay _inventoryDisplay;
        private ToolbarDisplay _toolbarDisplay;
        private EquipmentSystem _equipmentSystem;
        private SpriteFont _arial;
        private LargeHealthBar _healthBar;
        private List<InteractiveDialoguePane> _dialogePanes;
        private SpriteFont _dialogueFont;
        private SpriteFont _dialogueOptionFont;

        public LevelScreen(LoderGame game, SystemManager systemManager, EntityManager entityManager)
            : base(game.screenSystem, ScreenType.Level)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _levelSystem = (LevelSystem)_systemManager.getSystem(SystemType.Level);
            _content = new ContentManager(_game.Services);
            _content.RootDirectory = "Content";
            _equipmentSystem = (EquipmentSystem)_systemManager.getSystem(SystemType.Equipment);
            _playerId = (_systemManager.getSystem(SystemType.Player) as PlayerSystem).playerId;
            _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
            _arial = _content.Load<SpriteFont>("arial");
            _dialogePanes = new List<InteractiveDialoguePane>();
            _dialogueFont = _content.Load<SpriteFont>("shared_ui/dialogue_font");
            _dialogueOptionFont = _content.Load<SpriteFont>("shared_ui/dialogue_option_font");

            ToolbarComponent toolbarComponent = (ToolbarComponent)_entityManager.getComponent(LevelSystem.currentLevelUid, _playerId, ComponentType.Toolbar);

            _toolbarDisplay = new ToolbarDisplay(_game.spriteBatch, _equipmentSystem, toolbarComponent);
            _inventoryDisplay = new InventoryDisplay(_game.spriteBatch, _equipmentSystem, (InventoryComponent)_entityManager.getComponent(LevelSystem.currentLevelUid, _playerId, ComponentType.Inventory), toolbarComponent);
            _inventoryDisplay.inFocus = false;
            _toolbarDisplay.inFocus = true;

            _healthBar = new LargeHealthBar(_game.spriteBatch);
        }

        ~LevelScreen()
        {
            _content.Unload();
        }

        public void addDialoguePane(CharacterDialogueComponent dialogueComponent)
        {
            InteractiveDialoguePane pane = new InteractiveDialoguePane(
                    this,
                    UIAlignment.MiddleCenter,
                    0,
                    0,
                    600,
                    300,
                    _dialogueFont,
                    _dialogueOptionFont,
                    dialogueComponent);

            pane.scale = 0f;
            _dialogePanes.Add(pane);
            _transitions.Add(new ScaleTransition(pane, 0.1f, 1f, false, 0.1f, null, () => { pane.showText = true; }));
        }

        public void removeDialoguePane(CharacterDialogueComponent dialogueComponent)
        {
            InteractiveDialoguePane pane = null;

            for (int i = 0; i < _dialogePanes.Count; i++)
            {
                if (_dialogePanes[i].dialogueComponent == dialogueComponent)
                {
                    pane = _dialogePanes[i];
                }
            }

            _transitions.Add(new ScaleTransition(pane, 1f, 0.1f, false, 0.1f, () => { pane.showText = false; }, () => { _dialogePanes.Remove(pane); }));
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

            for (int i = 0; i < _dialogePanes.Count; i++)
            {
                _dialogePanes[i].update();
            }

            base.update();
        }

        public override void draw()
        {
            PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(LevelSystem.currentLevelUid, PlayerSystem.PLAYER_ID, ComponentType.Physics);

            if (_displayInventory)
            {
                _inventoryDisplay.draw();
            }
            _toolbarDisplay.draw();

            for (int i = 0; i < _dialogePanes.Count; i++)
            {
                _dialogePanes[i].draw();
            }

            base.draw();
        }
    }
}
