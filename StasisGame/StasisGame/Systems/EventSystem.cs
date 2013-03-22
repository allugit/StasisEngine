using System;
using System.Collections.Generic;
using StasisCore;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class EventSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private Dictionary<GameEventType, Dictionary<int, List<IEventHandler>>> _handlers;
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 50; } }
        public SystemType systemType { get { return SystemType.Event; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public EventSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _handlers = new Dictionary<GameEventType,Dictionary<int,List<IEventHandler>>>();
        }

        public void addHandler(GameEventType gameEventType, int listeningToEntityId, IEventHandler handler)
        {
            if (!_handlers.ContainsKey(gameEventType))
                _handlers.Add(gameEventType, new Dictionary<int,List<IEventHandler>>());
            if (!_handlers[gameEventType].ContainsKey(listeningToEntityId))
                _handlers[gameEventType].Add(listeningToEntityId, new List<IEventHandler>());
            if (!_handlers[gameEventType][listeningToEntityId].Contains(handler))
            {
                _handlers[gameEventType][listeningToEntityId].Add(handler);
                Console.WriteLine("event handler added: [{0}][{1}][{2}]", gameEventType, listeningToEntityId, handler);
            }
        }

        public void postEvent(GameEvent e)
        {
            Console.WriteLine("Event posted. origin: {0}, type: {1}", e.originEntityId, e.type);
            Dictionary<int, List<IEventHandler>> row;
            List<IEventHandler> handlers;
            if (_handlers.TryGetValue(e.type, out row))
            {
                if (row.TryGetValue(e.originEntityId, out handlers))
                {
                    for (int i = 0; i < handlers.Count; i++)
                        handlers[i].trigger(e);
                }
            }
        }

        public void update()
        {
        }
    }
}
