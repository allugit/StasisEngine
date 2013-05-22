using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using StasisGame.Components;
using StasisGame.Managers;
using StasisCore;

namespace StasisGame.Systems
{
    public class PlayerSystem : ISystem
    {
        public const int PLAYER_ID = 9999;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private int _playerId;
        private bool _paused;
        private bool _singleStep;
        private Vector2 _spawnPosition;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Player; } }
        public int playerId { get { return _playerId; } set { _playerId = value; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public EntityManager entityManager { get { return _entityManager; } }
        public Vector2 spawnPosition { get { return _spawnPosition; } set { _spawnPosition = value; } }

        public PlayerSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        // createPlayer
        public void createPlayer()
        {
            playerId = _entityManager.createEntity(PLAYER_ID);
        }

        // initializePlayerInventory -- Creates the player's inventory components
        public void initializePlayerInventory()
        {
            XElement inventoryData = DataManager.playerData.inventoryData;
            XElement toolbarData = DataManager.playerData.toolbarData;
            InventoryComponent inventoryComponent;

            // Player inventory
            if (inventoryData == null)
            {
                inventoryComponent = new InventoryComponent(32);
                _entityManager.addComponent(playerId, inventoryComponent);
            }
            else
            {
                int slots = int.Parse(inventoryData.Attribute("slots").Value);
                inventoryComponent = new InventoryComponent(slots);
                foreach (XElement itemData in inventoryData.Elements("Item"))
                {
                    string itemUID = itemData.Attribute("item_uid").Value;
                    XElement itemResource = ResourceManager.getResource(itemUID);
                    ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), itemResource.Attribute("type").Value);
                    Texture2D inventoryTexture = ResourceManager.getTexture(itemResource.Attribute("inventory_texture_uid").Value);
                    int quantity = int.Parse(itemData.Attribute("quantity").Value);
                    bool inWorld = false;
                    bool hasAiming = Loader.loadBool(itemResource.Attribute("adds_reticle"), false);
                    int maxRange = Loader.loadInt(itemResource.Attribute("range"), 0);

                    ItemComponent itemComponent = new ItemComponent(itemUID, itemType, inventoryTexture, quantity, inWorld, hasAiming, maxRange);
                    inventoryComponent.addItem(itemComponent);
                }
                _entityManager.addComponent(playerId, inventoryComponent);
            }

            // Player toolbar
            if (toolbarData != null)
            {
                int slots = int.Parse(toolbarData.Attribute("slots").Value);
                ToolbarComponent toolbarComponent = new ToolbarComponent(slots, playerId);
                EquipmentSystem equipmentSystem = (EquipmentSystem)_systemManager.getSystem(SystemType.Equipment);

                foreach (XElement slotData in toolbarData.Elements("Slot"))
                {
                    int slotId = int.Parse(slotData.Attribute("id").Value);
                    int inventorySlot = int.Parse(slotData.Attribute("inventory_slot").Value);
                    ItemComponent itemComponent = inventoryComponent.getItem(inventorySlot);

                    equipmentSystem.assignItemToToolbar(itemComponent, toolbarComponent, slotId);
                }
                _entityManager.addComponent(playerId, toolbarComponent);
            }
            else
            {
                _entityManager.addComponent(playerId, new ToolbarComponent(4, playerId));
            }
        }

        // Add components to the entity player that are needed to play in a level
        public void addLevelComponents()
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            Body body;
            //BodyDef bodyDef = new BodyDef();
            //PolygonShape bodyShape = new PolygonShape();
            //FixtureDef bodyFixtureDef = new FixtureDef();
            //CircleShape feetShape = new CircleShape();
            //FixtureDef feetFixtureDef = new FixtureDef();
            Fixture fixture;
            Fixture feetFixture;
            PolygonShape polygonShape = new PolygonShape(1f);
            CircleShape feetShape = new CircleShape(0.18f, 0.1f);

            body = BodyFactory.CreateBody(world, _spawnPosition, _playerId);
            body.IsBullet = true;
            body.FixedRotation = true;
            body.BodyType = BodyType.Dynamic;
            polygonShape.SetAsBox(0.18f, 0.27f);
            fixture = body.CreateFixture(polygonShape);
            fixture.Friction = 0f;
            fixture.Restitution = 0f;
            fixture.CollisionCategories = (Category)CollisionCategory.Player;
            fixture.CollidesWith = 
                (Category)CollisionCategory.DynamicGeometry |
                (Category)CollisionCategory.Item |
                (Category)CollisionCategory.Rope |
                (Category)CollisionCategory.StaticGeometry |
                (Category)CollisionCategory.Explosion;

            feetShape.Position = new Vector2(0, 0.27f);
            feetShape.Density = 0.1f;
            feetFixture = body.CreateFixture(feetShape);
            feetFixture.Friction = 0.1f;
            feetFixture.CollisionCategories = fixture.CollisionCategories;
            feetFixture.CollidesWith = fixture.CollidesWith;

            /*bodyDef.bullet = true;
            bodyDef.fixedRotation = true;
            bodyDef.position = spawnPosition;
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
                (ushort)CollisionCategory.StaticGeometry |
                (ushort)CollisionCategory.Explosion;

            feetShape._radius = 0.18f;
            feetShape._p = new Vector2(0, 0.27f);
            feetFixtureDef.density = 0.1f;
            feetFixtureDef.friction = 0.1f;
            feetFixtureDef.shape = feetShape;
            feetFixtureDef.filter.categoryBits = bodyFixtureDef.filter.categoryBits;
            feetFixtureDef.filter.maskBits = bodyFixtureDef.filter.maskBits;

            body = world.CreateBody(bodyDef);
            body.CreateFixture(bodyFixtureDef);
            feetFixture = body.CreateFixture(feetFixtureDef);*/

            _entityManager.addComponent(playerId, new PhysicsComponent(body));
            _entityManager.addComponent(playerId, new InputComponent());
            _entityManager.addComponent(playerId, new CharacterMovementComponent(feetFixture));
            _entityManager.addComponent(playerId, new CharacterRenderComponent());
            _entityManager.addComponent(playerId, new BodyFocusPointComponent(body, new Vector2(0, -7f), FocusType.Multiple));
            _entityManager.addComponent(playerId, new IgnoreTreeCollisionComponent());
            _entityManager.addComponent(playerId, new IgnoreRopeRaycastComponent());
            _entityManager.addComponent(playerId, new WorldPositionComponent(body.Position));
            _entityManager.addComponent(playerId, new SkipFluidResolutionComponent());
            _entityManager.addComponent(playerId, new ParticleInfluenceComponent(ParticleInfluenceType.Character));
        }

        // removeLevelComponents -- Remove level components from the player
        public void removeLevelComponents()
        {
            List<IComponent> components = new List<IComponent>(_entityManager.getEntityComponents(_playerId));  // create a copy of the list since we'll need to modify the original

            for (int i = 0; i < components.Count; i++)
            {
                IComponent component = components[i];

                // Exclude certain components here if they need to persist through levels, otherwise remove them.
                if (component.componentType == ComponentType.Inventory ||
                    component.componentType == ComponentType.Toolbar)
                {
                    continue;
                }

                _entityManager.removeComponent(_playerId, components[i]);
            }
        }

        // softKillPlayer -- Doesn't "kill" the player entity, just resets certain aspects of the entity to the last saved state
        public void softKillPlayer()
        {
            _entityManager.removeComponent(_playerId, ComponentType.Inventory);
            _entityManager.removeComponent(_playerId, ComponentType.Toolbar);
            initializePlayerInventory();
        }

        // update
        public void update()
        {
            if (!_paused || _singleStep)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(_playerId, ComponentType.Physics);

                if (physicsComponent != null)   // If there is a physics component, then we're in a level.
                {
                    InputComponent inputComponent = _entityManager.getComponent(_playerId, ComponentType.Input) as InputComponent;
                    CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(_playerId, ComponentType.CharacterMovement) as CharacterMovementComponent;
                    RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(_playerId, ComponentType.RopeGrab) as RopeGrabComponent;

                    if (inputComponent.usingGamepad)
                    {
                        if (inputComponent.newGamepadState.ThumbSticks.Left.X < 0)
                        {
                            characterMovementComponent.walkSpeedModifier = Math.Abs(inputComponent.newGamepadState.ThumbSticks.Left.X);
                            characterMovementComponent.walkLeft = true;
                        }
                        else if (inputComponent.newGamepadState.DPad.Left == ButtonState.Pressed)
                            characterMovementComponent.walkLeft = true;
                        else
                            characterMovementComponent.walkLeft = false;

                        if (inputComponent.newGamepadState.ThumbSticks.Left.X > 0)
                        {
                            characterMovementComponent.walkSpeedModifier = inputComponent.newGamepadState.ThumbSticks.Left.X;
                            characterMovementComponent.walkRight = true;
                        }
                        else if (inputComponent.newGamepadState.DPad.Right == ButtonState.Pressed)
                            characterMovementComponent.walkRight = true;
                        else
                            characterMovementComponent.walkRight = false;

                        characterMovementComponent.climbAmount = 0f;
                        characterMovementComponent.climbDown = false;
                        characterMovementComponent.climbUp = false;
                        if (inputComponent.newGamepadState.ThumbSticks.Left.Y > 0)
                        {
                            characterMovementComponent.climbUp = true;
                            characterMovementComponent.climbAmount = Math.Abs(inputComponent.newGamepadState.ThumbSticks.Left.Y);
                        }
                        else if (inputComponent.newGamepadState.ThumbSticks.Left.Y < 0)
                        {
                            characterMovementComponent.climbDown = true;
                            characterMovementComponent.climbAmount = Math.Abs(inputComponent.newGamepadState.ThumbSticks.Left.Y);
                        }

                        characterMovementComponent.jump = inputComponent.newGamepadState.Buttons.A == ButtonState.Pressed;
                    }
                    else
                    {
                        characterMovementComponent.walkSpeedModifier = 1f;
                        characterMovementComponent.walkLeft = inputComponent.newKeyState.IsKeyDown(Keys.A) || inputComponent.newKeyState.IsKeyDown(Keys.Left);
                        characterMovementComponent.walkRight = inputComponent.newKeyState.IsKeyDown(Keys.D) || inputComponent.newKeyState.IsKeyDown(Keys.Right);
                        characterMovementComponent.jump = inputComponent.newKeyState.IsKeyDown(Keys.Space);
                        characterMovementComponent.climbUp = inputComponent.newKeyState.IsKeyDown(Keys.W);
                        characterMovementComponent.climbDown = inputComponent.newKeyState.IsKeyDown(Keys.S);
                        characterMovementComponent.climbAmount = 1f;
                        characterMovementComponent.doRopeGrab = inputComponent.newKeyState.IsKeyDown(Keys.E);
                        characterMovementComponent.allowRopeGrab = characterMovementComponent.allowRopeGrab ? true : (inputComponent.newKeyState.IsKeyUp(Keys.E) && inputComponent.oldKeyState.IsKeyDown(Keys.E));

                    }
                }
            }
            _singleStep = false;
        }
    }
}
