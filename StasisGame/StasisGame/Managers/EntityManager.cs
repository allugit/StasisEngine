using System;
using System.Collections.Generic;
using StasisGame.Components;

namespace StasisGame.Managers
{
    public class EntityManager
    {
        private SystemManager _systemManager;
        private Dictionary<int, List<IComponent>> _entities;
        private EntityFactory _factory;

        public EntityFactory factory { get { return _factory; } }

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
            _factory = new EntityFactory(_systemManager, this);
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

        public List<T> getComponents<T>(ComponentType componentType) where T : IComponent
        {
            List<T> results = new List<T>();
            foreach (List<IComponent> components in _entities.Values)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    if (components[i].componentType == componentType)
                    {
                        results.Add((T)components[i]);
                    }
                }
            }
            return results;
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
