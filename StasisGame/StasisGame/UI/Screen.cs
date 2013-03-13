using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.UI
{
    abstract public class Screen
    {
        private List<IUIComponent> _UIComponents;
        private int _selectedIndex = -1;

        public Screen()
        {
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

        public void select(IUIComponent component)
        {
            if (component.selectable && _UIComponents.Contains(component))
            {
                _selectedIndex = _UIComponents.IndexOf(component);
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
                int index = _selectedIndex;

                while (!foundNextSelectableComponent)
                {
                    index++;

                    if (index >= _UIComponents.Count)
                        index = 0;

                    if (_UIComponents[index].selectable)
                    {
                        foundNextSelectableComponent = true;
                        _selectedIndex = index;
                        _UIComponents[index].onSelect();
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
                bool foundNextSelectableComponent = false;
                int index = _selectedIndex;

                while (!foundNextSelectableComponent)
                {
                    index--;

                    if (index < 0)
                        index = _UIComponents.Count - 1;

                    if (_UIComponents[index].selectable)
                    {
                        foundNextSelectableComponent = true;
                        _selectedIndex = index;
                        _UIComponents[index].onSelect();
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
