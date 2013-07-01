using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public delegate void UIComponentAction(IUIComponent component);
    public enum UIComponentAlignment
    {
        TopLeft,
        TopCenter
    };
    public enum ScreenType
    {
        MainMenu,
        OptionsMenu,
        WorldMap,
        Level,
        LoadGameMenu,
        PlayerCreation
    };

    abstract public class Screen
    {
        protected SpriteBatch _spriteBatch;
        protected List<IUIComponent> _UIComponents;
        protected ISelectableUIComponent _selectedComponent;
        protected GamePadState _newGamepadState;
        protected GamePadState _oldGamepadState;
        protected KeyboardState _newKeyState;
        protected KeyboardState _oldKeyState;
        protected MouseState _newMouseState;
        protected MouseState _oldMouseState;
        protected ScreenType _screenType;

        public ScreenType screenType { get { return _screenType; } set { _screenType = value; } }
        public SpriteBatch spriteBatch { get { return _spriteBatch; } }
        public KeyboardState newKeyState { get { return _newKeyState; } }
        public KeyboardState oldKeyState { get { return _oldKeyState; } }
        public MouseState newMouseState { get { return _newMouseState; } }
        public MouseState oldMouseState { get { return _oldMouseState; } }

        public Screen(SpriteBatch spriteBatch, ScreenType screenType)
        {
            _spriteBatch = spriteBatch;
            _screenType = screenType;
            _UIComponents = new List<IUIComponent>();
        }

        public void addComponent(IUIComponent component)
        {
            _UIComponents.Add(component);
        }

        public void removeComponent(IUIComponent component)
        {
            _UIComponents.Remove(component);
        }

        public void select(ISelectableUIComponent component)
        {
            if (component.selectable && _UIComponents.Contains(component))
            {
                if (_selectedComponent != null)
                    _selectedComponent.onDeselect();

                _selectedComponent = component;
                component.onSelect();
            }
        }

        public void selectNextComponent()
        {
            bool selectableComponentExists = false;

            for (int i = 0; i < _UIComponents.Count; i++)
            {
                if (_UIComponents[i].selectable)
                {
                    selectableComponentExists = true;
                    break;
                }
            }

            if (selectableComponentExists)
            {
                bool foundNextSelectableComponent = false;
                int index = _selectedComponent == null ? -1 : _UIComponents.IndexOf(_selectedComponent);

                while (!foundNextSelectableComponent)
                {
                    index++;

                    if (index >= _UIComponents.Count)
                        index = 0;

                    if (_UIComponents[index].selectable)
                    {
                        foundNextSelectableComponent = true;
                        if (_selectedComponent != null)
                            _selectedComponent.onDeselect();
                        _selectedComponent = _UIComponents[index] as ISelectableUIComponent;
                        _selectedComponent.onSelect();
                    }
                }
            }
        }

        public void selectPreviousComponent()
        {
            bool selectableComponentExists = false;

            for (int i = 0; i < _UIComponents.Count; i++)
            {
                if (_UIComponents[i].selectable)
                {
                    selectableComponentExists = true;
                    break;
                }
            }

            if (selectableComponentExists)
            {
                bool foundPreviousSelectableComponent = false;
                int index = _selectedComponent == null ? 1 : _UIComponents.IndexOf(_selectedComponent);

                while (!foundPreviousSelectableComponent)
                {
                    index--;

                    if (index < 0)
                        index = _UIComponents.Count - 1;

                    if (_UIComponents[index].selectable)
                    {
                        foundPreviousSelectableComponent = true;
                        if (_selectedComponent != null)
                            _selectedComponent.onDeselect();
                        _selectedComponent = _UIComponents[index] as ISelectableUIComponent;
                        _selectedComponent.onSelect();
                    }
                }
            }
        }

        public string wrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ', '\n');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        virtual public void update()
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
            if (InputSystem.usingGamepad)
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

            for (int i = 0; i < _UIComponents.Count; i++)
            {
                _UIComponents[i].UIUpdate();
            }
        }

        virtual public void draw()
        {
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                _UIComponents[i].UIDraw();
            }
        }

    }
}
