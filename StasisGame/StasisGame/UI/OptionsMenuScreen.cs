using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Managers;

namespace StasisGame.UI
{
    public class DisplayDimensions
    {
        public int width;
        public int height;
        public DisplayDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(DisplayDimensions))
            {
                DisplayDimensions o = (DisplayDimensions)obj;
                return o.width == width && o.height == height;
            }
            else if (obj.GetType() == typeof(DisplayMode))
            {
                DisplayMode o = (DisplayMode)obj;
                return o.Width == width && o.Height == height;
            }

            return base.Equals(obj);
        }
    };

    public class OptionsMenuScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _background;
        private Texture2D _logo;
        private Texture2D _optionsContainer;
        private SpriteFont _santaBarbaraNormal;
        private SpriteFont _arial;
        private ContentManager _content;
        private List<DisplayDimensions> _displayDimensions;
        private DisplayDimensions _selectedDimensions;
        private TextButton _displayModeButton;
        private TextButton _fullscreenButton;
        private bool _selectedFullscreen;
        private ControllerType _selectedControllerType;
        private TextButton _controllerTypeButton;

        public OptionsMenuScreen(LoderGame game) : base(ScreenType.OptionsMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _background = _content.Load<Texture2D>("main_menu/bg");
            _logo = _content.Load<Texture2D>("main_menu/logo");
            _optionsContainer = _content.Load<Texture2D>("options_menu/options_container");
            _santaBarbaraNormal = _content.Load<SpriteFont>("santa_barbara_normal");
            _arial = _content.Load<SpriteFont>("arial");
            _displayDimensions = new List<DisplayDimensions>();
            _selectedFullscreen = DataManager.gameSettings.fullscreen;
            _selectedControllerType = DataManager.gameSettings.controllerType;
            _selectedDimensions = new DisplayDimensions(DataManager.gameSettings.screenWidth, DataManager.gameSettings.screenHeight);
            _displayDimensions.Add(_selectedDimensions);

            foreach (DisplayMode displayMode in _game.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                if (!_selectedDimensions.Equals(displayMode))
                    _displayDimensions.Add(new DisplayDimensions(displayMode.Width, displayMode.Height));
            }

            TextureButton saveButton = new TextureButton(
                _game.spriteBatch,
                0,
                690,
                100,
                75,
                _content.Load<Texture2D>("options_menu/save_selected"),
                _content.Load<Texture2D>("options_menu/save_unselected"),
                UIComponentAlignment.TopCenter,
                (component) => { saveOptions(); });

            Label controllerLabel = new Label(
                _game.spriteBatch,
                _santaBarbaraNormal,
                "Controller",
                -220,
                280,
                UIComponentAlignment.TopCenter);

            Label keyboardLabel = new Label(
                _game.spriteBatch,
                _arial,
                "Keyboard",
                -200,
                320,
                UIComponentAlignment.TopCenter);

            TextButton redefineKeyboardButton = new TextButton(
                _game.spriteBatch,
                _arial,
                Color.LightGreen,
                140,
                320,
                "Redefine Keys",
                UIComponentAlignment.TopCenter,
                (component) => { });

            Label gamepadLabel = new Label(
                _game.spriteBatch,
                _arial,
                "Gamepad",
                -200,
                340,
                UIComponentAlignment.TopCenter);

            TextButton redefineGamepadButton = new TextButton(
                _game.spriteBatch,
                _arial,
                Color.LightGreen,
                140,
                340,
                "Redefine Buttons",
                UIComponentAlignment.TopCenter,
                (component) => { });

            Label controllerTypeLabel = new Label(
                _game.spriteBatch,
                _arial,
                "Selected Controller",
                -200,
                360,
                UIComponentAlignment.TopCenter);

            _controllerTypeButton = new TextButton(
                _game.spriteBatch,
                _arial,
                Color.LightGreen,
                140,
                360,
                getControllerTypeText(_selectedControllerType),
                UIComponentAlignment.TopCenter,
                (component) => { selectNextControllerType(); });

            Label displayLabel = new Label(
                _game.spriteBatch,
                _santaBarbaraNormal,
                "Display",
                -220,
                400,
                UIComponentAlignment.TopCenter);

            Label resolutionLabel = new Label(
                _game.spriteBatch,
                _arial,
                "Resolution",
                -200,
                440,
                UIComponentAlignment.TopCenter);

            _displayModeButton = new TextButton(
                _game.spriteBatch,
                _arial,
                Color.LightGreen,
                140,
                440,
                String.Format("{0} x {1}", _selectedDimensions.width, _selectedDimensions.height),
                UIComponentAlignment.TopCenter,
                (component) => { selectNextDisplayMode(); });

            Label fullscreenLabel = new Label(
                _game.spriteBatch,
                _arial,
                "Fullscreen",
                -200,
                460,
                UIComponentAlignment.TopCenter);

            _fullscreenButton = new TextButton(
                _game.spriteBatch,
                _arial,
                Color.LightGreen,
                140,
                460,
                _selectedFullscreen ? "True" : "False",
                UIComponentAlignment.TopCenter,
                (component) => { switchFullscreen(); });

            _UIComponents.Add(controllerLabel);
            _UIComponents.Add(keyboardLabel);
            _UIComponents.Add(gamepadLabel);
            _UIComponents.Add(redefineKeyboardButton);
            _UIComponents.Add(redefineGamepadButton);
            _UIComponents.Add(controllerTypeLabel);
            _UIComponents.Add(_controllerTypeButton);

            _UIComponents.Add(displayLabel);
            _UIComponents.Add(resolutionLabel);
            _UIComponents.Add(_displayModeButton);
            _UIComponents.Add(fullscreenLabel);
            _UIComponents.Add(_fullscreenButton);

            _UIComponents.Add(saveButton);
        }

        ~OptionsMenuScreen()
        {
            _content.Unload();
        }

        public string getControllerTypeText(ControllerType type)
        {
            switch (type)
            {
                case ControllerType.Gamepad: return "Gamepad";
                case ControllerType.KeyboardAndMouse: return "Keyboard and Mouse";
            }

            return "";
        }

        public void selectNextControllerType()
        {
            List<ControllerType> values = new List<ControllerType>((ControllerType[])Enum.GetValues(typeof(ControllerType)));
            int index = values.IndexOf(_selectedControllerType);

            index++;

            if (index >= values.Count)
                index = 0;

            _selectedControllerType = values[index];
            _controllerTypeButton.text = getControllerTypeText(_selectedControllerType);
        }

        public void selectNextDisplayMode()
        {
            int index = _displayDimensions.IndexOf(_selectedDimensions);

            index++;

            if (index >= _displayDimensions.Count)
                index = 0;

            _selectedDimensions = _displayDimensions[index];
            _displayModeButton.text = String.Format("{0} x {1}", _selectedDimensions.width, _selectedDimensions.height);
        }

        public void switchFullscreen()
        {
            _selectedFullscreen = !_selectedFullscreen;
            _fullscreenButton.text = _selectedFullscreen ? "True" : "False";
        }

        public void saveOptions()
        {
            // Apply settings
            DataManager.gameSettings.screenWidth = _selectedDimensions.width;
            DataManager.gameSettings.screenHeight = _selectedDimensions.height;
            DataManager.gameSettings.fullscreen = _selectedFullscreen;
            _game.graphics.PreferredBackBufferWidth = DataManager.gameSettings.screenWidth;
            _game.graphics.PreferredBackBufferHeight = DataManager.gameSettings.screenHeight;
            _game.graphics.IsFullScreen = DataManager.gameSettings.fullscreen;
            _game.graphics.ApplyChanges();
            DataManager.gameSettings.controllerType = _selectedControllerType;

            // Save settings
            DataManager.saveGameSettings();

            _game.closeOptionsMenu();
        }

        override public void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            // Mouse input
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                IUIComponent component = _UIComponents[i];

                if (component.selectable)
                {
                    ISelectableUIComponent selectableComponent = component as ISelectableUIComponent;
                    if (selectableComponent.hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
                    {
                        if (_oldMouseState.X - _newMouseState.X != 0 || _oldMouseState.Y - _newMouseState.Y != 0)
                            select(selectableComponent);

                        if (_oldMouseState.LeftButton == ButtonState.Released && _newMouseState.LeftButton == ButtonState.Pressed)
                            selectableComponent.activate();
                    }
                }
            }

            // Gamepad input
            if (_newGamepadState.IsConnected)
            {
                bool movingUp = (_oldGamepadState.ThumbSticks.Left.Y < 0.25f && _newGamepadState.ThumbSticks.Left.Y > 0.25f) ||
                    (_oldGamepadState.DPad.Up == ButtonState.Released && _newGamepadState.DPad.Up == ButtonState.Pressed);
                bool movingDown = (_oldGamepadState.ThumbSticks.Left.Y > -0.25f && _newGamepadState.ThumbSticks.Left.Y < -0.25f) ||
                    (_oldGamepadState.DPad.Down == ButtonState.Released && _newGamepadState.DPad.Down == ButtonState.Pressed);
                bool activate = _oldGamepadState.Buttons.A == ButtonState.Released && _newGamepadState.Buttons.A == ButtonState.Pressed;

                if (movingUp)
                    selectPreviousComponent();
                else if (movingDown)
                    selectNextComponent();

                if (activate && _selectedComponent != null)
                {
                    _selectedComponent.activate();
                }
            }

            base.update();
        }

        override public void draw()
        {
            float scale = (float)_game.GraphicsDevice.Viewport.Height / (float)_background.Height;
            _game.spriteBatch.Draw(_background, Vector2.Zero, _background.Bounds, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _game.spriteBatch.Draw(_logo, new Vector2((int)(_game.GraphicsDevice.Viewport.Width / 2f), 100f), _logo.Bounds, Color.White, 0, new Vector2(_logo.Width, _logo.Height) / 2, 0.75f, SpriteEffects.None, 0);
            _game.spriteBatch.Draw(_optionsContainer, new Vector2((int)(_game.GraphicsDevice.Viewport.Width / 2f), 150f), _optionsContainer.Bounds, Color.White, 0f, new Vector2((int)(_optionsContainer.Width / 2f), 0), 1f, SpriteEffects.None, 0f);

            base.draw();
        }
    }
}
