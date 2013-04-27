using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Components;
using StasisGame.Systems;
using StasisCore;
using Box2D.XNA;

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

        // Initialize player inventory from xml data
        public void initializePlayerInventory(int playerId, XElement data)
        {
            if (data == null)
            {
                addComponent(playerId, new InventoryComponent(32));
            }
            else
            {
                int slots = int.Parse(data.Attribute("slots").Value);
                InventoryComponent inventoryComponent = new InventoryComponent(slots);
                foreach (XElement itemData in data.Elements("Item"))
                {
                    string itemUID = itemData.Attribute("item_uid").Value;
                    XElement itemResource = ResourceManager.getResource(itemUID);
                    ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), itemResource.Attribute("type").Value);
                    Texture2D inventoryTexture = ResourceManager.getTexture(itemResource.Attribute("inventory_texture_uid").Value);
                    int quantity = int.Parse(itemData.Attribute("quantity").Value);
                    bool inWorld = false;
                    bool hasAiming = bool.Parse(itemResource.Attribute("adds_reticle").Value);
                    int maxRange = int.Parse(itemResource.Attribute("range").Value);
                    ItemComponent itemComponent = new ItemComponent(itemUID, itemType, inventoryTexture, quantity, inWorld, hasAiming, maxRange);
                    inventoryComponent.addItem(itemComponent);
                }
                addComponent(playerId, inventoryComponent);
            }
        }

        // Initialize player toolbar from xml data
        public void initializePlayerToolbar(int playerId, InventoryComponent inventoryComponent, XElement data)
        {
            if (data != null)
            {
                int slots = int.Parse(data.Attribute("slots").Value);
                ToolbarComponent toolbarComponent = new ToolbarComponent(slots, playerId);
                foreach (XElement slotData in data.Elements("Slot"))
                {
                    int slotId = int.Parse(slotData.Attribute("id").Value);
                    int inventorySlot = int.Parse(slotData.Attribute("inventory_slot").Value);
                    ItemComponent itemComponent = inventoryComponent.getItem(inventorySlot);
                    toolbarComponent.inventory[slotId] = itemComponent;
                }
            }
            else
            {
                addComponent(playerId, new ToolbarComponent(4, playerId));
            }
        }

        // Add components to the entity player that are needed to play in a level
        public void addLevelComponentsToPlayer(PlayerSystem playerSystem)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            Body body;
            BodyDef bodyDef = new BodyDef();
            PolygonShape bodyShape = new PolygonShape();
            FixtureDef bodyFixtureDef = new FixtureDef();
            CircleShape feetShape = new CircleShape();
            FixtureDef feetFixtureDef = new FixtureDef();
            Fixture feetFixture;
            int playerId = playerSystem.playerId;

            bodyDef.bullet = true;
            bodyDef.fixedRotation = true;
            bodyDef.position = playerSystem.spawnPosition;
            bodyDef.type = BodyType.Dynamic;
            bodyDef.userData = playerId;
            bodyShape.SetAsBox(0.18f, 0.27f);
            bodyFixtureDef.density = 1f;
            bodyFixtureDef.friction = 0f;
            bodyFixtureDef.restitution = 0f;
            bodyFixtureDef.shape = bodyShape;
            bodyFixtureDef.filter.categoryBits = (ushort)CollisionCategory.Player;
            bodyFixtureDef.filter.maskBits =
                (ushort)CollisionCategory.DynamicGeometry |
                (ushort)CollisionCategory.Item |
                (ushort)CollisionCategory.Rope |
                (ushort)CollisionCategory.StaticGeometry;

            feetShape._radius = 0.18f;
            feetShape._p = new Vector2(0, 0.27f);
            feetFixtureDef.density = 0.1f;
            feetFixtureDef.friction = 0.1f;
            feetFixtureDef.shape = feetShape;
            feetFixtureDef.filter.categoryBits = bodyFixtureDef.filter.categoryBits;
            feetFixtureDef.filter.maskBits = bodyFixtureDef.filter.maskBits;

            body = world.CreateBody(bodyDef);
            body.CreateFixture(bodyFixtureDef);
            feetFixture = body.CreateFixture(feetFixtureDef);

            addComponent(playerId, new PhysicsComponent(body));
            addComponent(playerId, new InputComponent());
            addComponent(playerId, new CharacterMovementComponent(feetFixture));
            addComponent(playerId, new CharacterRenderComponent());
            addComponent(playerId, new BodyFocusPointComponent(body, new Vector2(0, -7f), FocusType.Multiple));
            addComponent(playerId, new IgnoreTreeCollisionComponent());
            addComponent(playerId, new IgnoreRopeRaycastComponent());
            addComponent(playerId, new WorldPositionComponent(body.GetPosition()));
        }

        // Remove level-related components from player entity
        public void removeLevelComponentsFromPlayer(int playerId)
        {
            removeComponent(playerId, ComponentType.Physics);
            removeComponent(playerId, ComponentType.Input);
            removeComponent(playerId, ComponentType.CharacterMovement);
            removeComponent(playerId, ComponentType.CharacterRender);
            removeComponent(playerId, ComponentType.BodyFocusPoint);
            removeComponent(playerId, ComponentType.IgnoreTreeCollision);
            removeComponent(playerId, ComponentType.IgnoreRopeRaycast);
            removeComponent(playerId, ComponentType.WorldPosition);
        }
    }
}
