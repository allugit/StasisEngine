using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Components;
using StasisGame.Systems;
using StasisCore;

namespace StasisGame.Managers
{
    public class EntityManager
    {
        private SystemManager _systemManager;
        private Dictionary<string, Dictionary<int, List<IComponent>>> _entities;
        private EntityFactory _factory;
        private Dictionary<string, List<int>> _reservedIds;

        public EntityFactory factory { get { return _factory; } }

        public EntityManager(SystemManager systemManager)
        {
            _systemManager = systemManager;
            _entities = new Dictionary<string, Dictionary<int, List<IComponent>>>();
            _factory = new EntityFactory(_systemManager, this);
            _reservedIds = new Dictionary<string, List<int>>();
        }

        public int getNewId(string levelUid)
        {
            int i = 0;
            while (_entities[levelUid].ContainsKey(i) || _reservedIds[levelUid].Contains(i))
                i++;
            return i;
        }

        // Create an entity
        public int createEntity(string levelUid)
        {
            int id = getNewId(levelUid);
            _entities[levelUid].Add(id, new List<IComponent>());
            return id;
        }

        // Create an entity with a specific id -- can use reserved ids
        public int createEntity(string levelUid, int id)
        {
            _entities[levelUid].Add(id, new List<IComponent>());
            return id;
        }

        // Reserve entity id
        public void reserveEntityId(string levelUid, int id)
        {
            Debug.Assert(!_reservedIds[levelUid].Contains(id));
            _reservedIds[levelUid].Add(id);
        }

        // Clear reserved entity ids
        public void clearReservedEntityIds()
        {
            _reservedIds.Clear();
        }

        // Kill an entity
        public void killEntity(string levelUid, int id)
        {
            _entities[levelUid].Remove(id);
        }

        // Kill all entities
        public void killAllEntities(string levelUid)
        {
            killAllEntities(levelUid, new List<int>());
        }
        public void killAllEntities(string levelUid, List<int> excludedIds)
        {
            List<int> entitiesToRemove = new List<int>();

            foreach (KeyValuePair<int, List<IComponent>> pair in _entities[levelUid])
            {
                if (!excludedIds.Contains(pair.Key))
                    entitiesToRemove.Add(pair.Key);
            }

            foreach (int id in entitiesToRemove)
            {
                _entities[levelUid].Remove(id);
            }
        }

        // Add a component to an entity
        public void addComponent(string levelUid, int id, IComponent component)
        {
            _entities[levelUid][id].Add(component);
        }

        // Remove a component from an entity, based on the type of component
        public void removeComponent(string levelUid, int id, ComponentType componentType)
        {
            IComponent component = getComponent(levelUid, id, componentType);
            removeComponent(levelUid, id, component);
        }

        // Remove a component from an entity, based on an instance of the component
        public void removeComponent(string levelUid, int id, IComponent component)
        {
            _entities[levelUid][id].Remove(component);
        }

        // Get a component from an entity
        public IComponent getComponent(string levelUid, int id, ComponentType componentType)
        {
            if (_entities[levelUid].ContainsKey(id))
            {
                foreach (IComponent component in _entities[levelUid][id])
                {
                    if (component.componentType == componentType)
                        return component;
                }
            }
            return null;
        }

        // Tries to get a component from an entity, and returns true on success
        public bool tryGetComponent(string levelUid, int id, ComponentType componentType, out IComponent component)
        {
            component = null;

            if (_entities[levelUid].ContainsKey(id))
            {
                foreach (IComponent c in _entities[levelUid][id])
                {
                    if (c.componentType == componentType)
                    {
                        component = c;
                        return true;
                    }
                }
            }
            return false;
        }

        // Get all components of a certain type
        public List<T> getComponents<T>(string levelUid, ComponentType componentType) where T : IComponent
        {
            List<T> results = new List<T>();
            foreach (List<IComponent> components in _entities[levelUid].Values)
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
        public List<int> getEntitiesPosessing(string levelUid, ComponentType componentType)
        {
            List<int> results = new List<int>();
            foreach (int e in _entities[levelUid].Keys)
            {
                foreach (IComponent component in _entities[levelUid][e])
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
        public List<IComponent> getEntityComponents(string levelUid, int entityId)
        {
            if (_entities[levelUid].ContainsKey(entityId))
                return _entities[levelUid][entityId];

            return null;
        }
    }
}
