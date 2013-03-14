using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StasisGame.UI
{
    public delegate void UIComponentAction(IUIComponent component);
    public enum UIComponentAlignment
    {
        TopLeft,
        TopCenter,
        Center
    };
    public enum ScreenType
    {
        MainMenu,
        OptionsMenu
    };

    abstract public class Screen
    {
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

        public Screen(ScreenType screenType)
        {
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
                         _selectedComponent.onDeselect();
                        _selectedComponent = _UIComponents[index] as ISelectableUIComponent;
                        _selectedComponent.onSelect();
                    }
                }
            }
        }

        virtual public void update()
        {
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
