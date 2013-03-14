using System;
using System.Collections.Generic;
using StasisGame.Managers;
using StasisGame.UI;

namespace StasisGame.Systems
{
    public class ScreenSystem : ISystem
    {
        private SystemManager _systemManager;
        private List<Screen> _screens;

        public SystemType systemType { get { return SystemType.Screen; } }
        public int defaultPriority { get { return 0; } }

        public ScreenSystem(SystemManager systemManager)
        {
            _systemManager = systemManager;
            _screens = new List<Screen>();
        }

        public void addScreen(Screen screen)
        {
            _screens.Add(screen);
        }

        public void removeScreen(ScreenType screenType)
        {
            Screen screenToRemove = null;

            for (int i = 0; i < _screens.Count; i++)
            {
                if (_screens[i].screenType == screenType)
                    screenToRemove = _screens[i];
            }

            if (screenToRemove != null)
                removeScreen(screenToRemove);
        }

        public void removeScreen(Screen screen)
        {
            _screens.Remove(screen);
        }

        public void update()
        {
            for (int i = 0; i < _screens.Count; i++)
            {
                _screens[i].update();
            }
        }

        public void draw()
        {
            for (int i = 0; i < _screens.Count; i++)
            {
                _screens[i].draw();
            }
        }
    }
}
