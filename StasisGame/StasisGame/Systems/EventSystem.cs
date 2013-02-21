using System;
using System.Collections.Generic;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class EventSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public int defaultPriority { get { return 50; } }
        public SystemType systemType { get { return SystemType.Event; } }

        public EventSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
        }
    }
}
