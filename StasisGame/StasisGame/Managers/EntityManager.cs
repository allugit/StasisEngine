using System;
using System.Collections.Generic;
using StasisGame.Components;

namespace StasisGame.Managers
{
    public class EntityManager
    {
        private SystemManager _systemManager;
        private Dictionary<int, List<IComponent>> _entities;
        private EntityFactory _templates;

        public EntityFactory templates { get { return _templates; } }

        public int newId
        {
            get
            {
                int i = 0;
                while (_entities.ContainsKey(i))
                    i++;
                return i;
            }
        }

        public EntityManager(SystemManager systemManager)
        {
            _systemManager = systemManager;
            _entities = new Dictionary<int, List<IComponent>>();
            _templates = new EntityFactory(_systemManager, this);
        }

        public int createEntity()
        {
            int id = newId;
            _entities.Add(id, new List<IComponent>());
            return id;
        }

        public void killEntity(int id)
        {
            _entities.Remove(id);
        }

        public void addComponent(int id, IComponent component)
        {
            _entities[id].Add(component);
        }

        public void removeComponent(int id, IComponent component)
        {
            _entities[id].Remove(component);
        }

        public IComponent getComponent(int id, ComponentType componentType)
        {
            foreach (IComponent component in _entities[id])
            {
                if (component.componentType == componentType)
                    return component;
            }
            return null;
        }

        public List<int> getEntitiesPosessing(ComponentType componentType)
        {
            List<int> results = new List<int>();
            foreach (int e in _entities.Keys)
            {
                foreach (IComponent component in _entities[e])
                {
                    if (component.componentType == componentType)
                    {
                        results.Add(e);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
