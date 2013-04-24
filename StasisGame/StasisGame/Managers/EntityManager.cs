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

        // Create an entity
        public int createEntity()
        {
            int id = newId;
            _entities.Add(id, new List<IComponent>());
            return id;
        }

        // Kill an entity
        public void killEntity(int id)
        {
            _entities.Remove(id);
        }

        // Kill all entities
        public void killAllEntities()
        {
            _entities.Clear();
        }

        // Add a component to an entity
        public void addComponent(int id, IComponent component)
        {
            _entities[id].Add(component);
        }

        // Remove a component from an entity, based on the type of component
        public void removeComponent(int id, ComponentType componentType)
        {
            IComponent component = getComponent(id, componentType);
            removeComponent(id, component);
        }

        // Remove a component from an entity, based on an instance of the component
        public void removeComponent(int id, IComponent component)
        {
            _entities[id].Remove(component);
        }

        // Get a component from an entity
        public IComponent getComponent(int id, ComponentType componentType)
        {
            if (_entities.ContainsKey(id))
            {
                foreach (IComponent component in _entities[id])
                {
                    if (component.componentType == componentType)
                        return component;
                }
            }
            return null;
        }

        // Get all components of a certain type
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

        // Get all entities posessing a type of component
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

        // Get all of an entity's components
        public List<IComponent> getEntityComponents(int entityId)
        {
            if (_entities.ContainsKey(entityId))
                return _entities[entityId];

            return null;
        }
    }
}
