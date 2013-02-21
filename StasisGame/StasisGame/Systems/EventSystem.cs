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
        private Dictionary<GameEventType, Dictionary<int, List<int>>> _subscriptions;

        public int defaultPriority { get { return 50; } }
        public SystemType systemType { get { return SystemType.Event; } }

        public EventSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _handlers = new Dictionary<GameEventType, Dictionary<int, List<IEventHandler>>>();
            _subscriptions = new Dictionary<GameEventType, Dictionary<int, List<int>>>();
        }

        public void addHandler(GameEventType gameEventType, int handlerEntityId, IEventHandler handler)
        {
            if (!_handlers.ContainsKey(gameEventType))
                _handlers.Add(gameEventType, new Dictionary<int, List<IEventHandler>>());
            if (!_handlers[gameEventType].ContainsKey(handlerEntityId))
                _handlers[gameEventType].Add(handlerEntityId, new List<IEventHandler>());
            if (!_handlers[gameEventType][handlerEntityId].Contains(handler))
                _handlers[gameEventType][handlerEntityId].Add(handler);
        }

        public void subscribe(GameEventType gameEventType, int handlerEntityId, int senderEntityId)
        {
            if (!_subscriptions.ContainsKey(gameEventType))
                _subscriptions.Add(gameEventType, new Dictionary<int, List<int>>());
            if (!_subscriptions[gameEventType].ContainsKey(handlerEntityId))
                _subscriptions[gameEventType].Add(handlerEntityId, new List<int>());
            if (!_subscriptions[gameEventType][handlerEntityId].Contains(senderEntityId))
                _subscriptions[gameEventType][handlerEntityId].Add(senderEntityId);
        }

        public void update()
        {
        }
    }
}
