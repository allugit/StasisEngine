using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.UI
{
    public class Screen
    {
        private List<IUIComponent> _UIComponents;
        private int _selectedIndex;

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
            if (_UIComponents.Contains(component))
            {
                _selectedIndex = _UIComponents.IndexOf(component);
                component.onSelect();
            }
        }

        public void selectNextComponent()
        {
            int newIndex = _selectedIndex + 1;

            if (newIndex >= _UIComponents.Count)
                newIndex = 0;

            _selectedIndex = newIndex;

            _UIComponents[_selectedIndex].onSelect();
        }

        public void selectPreviousComponent()
        {
            int newIndex = _selectedIndex - 1;

            if (newIndex < 0)
                newIndex = _UIComponents.Count - 1;

            _selectedIndex = newIndex;

            _UIComponents[_selectedIndex].onSelect();
        }

        public void update()
        {
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                _UIComponents[i].UIUpdate();
            }
        }

        public void draw()
        {
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                _UIComponents[i].UIDraw();
            }
        }

    }
}
