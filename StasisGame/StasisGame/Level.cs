using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisGame.Systems;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame
{
    public class Level
    {
        private LoderGame _game;
        private string _uid;
        private EntityManager _entityManager;
        private RenderSystem _renderSystem;
        private SystemManager _systemManager;
        private ScriptManager _scriptManager;

        public EntityManager entityManager { get { return _entityManager; } }
        public SystemManager systemManager { get { return _systemManager; } }
        public string uid { get { return _uid; } }

        public Level(LoderGame game, string levelUID)
        {
            _game = game;
            _uid = levelUID;
            _systemManager = _game.systemManager;
            _entityManager = _game.entityManager;
            _scriptManager = _game.scriptManager;

            XElement data = null;
            List<XElement> secondPassData = new List<XElement>();
            string backgroundUID;
            XElement backgroundData;
            Background background;

            // Load xml
            using (Stream stream = TitleContainer.OpenStream(ResourceManager.levelPath + string.Format("\\{0}.xml", levelUID)))
            {
                XDocument doc = XDocument.Load(stream);
                data = doc.Element("Level");
            }

            // Create systems
            _systemManager.add(new InputSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new PhysicsSystem(_systemManager, _entityManager, data), -1);
            _systemManager.add(new LevelGoalSystem(_game, _systemManager, _entityManager), -1);
            _systemManager.add(new CameraSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new EventSystem(_systemManager, _entityManager), -1);
            _renderSystem = new RenderSystem(_game, _systemManager, _entityManager);
            _systemManager.add(new RopeSystem(_systemManager, _entityManager), -1);
            _systemManager.add(_renderSystem, -1);

            // Create background
            backgroundUID = Loader.loadString(data.Attribute("background_uid"), "default_background");
            backgroundData = ResourceManager.getResource(backgroundUID);
            background = new Background(backgroundData);
            background.loadTextures();
            _renderSystem.setBackground(background);

            // Create output gate entities
            _entityManager.factory.createOutputGates(data);

            // Create entities
            foreach (XElement actorData in data.Elements("Actor"))
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Box":
                        _entityManager.factory.createBox(actorData);
                        break;

                    case "Circle":
                        _entityManager.factory.createCircle(actorData);
                        break;

                    case "Circuit":
                        secondPassData.Add(actorData);
                        break;

                    case "Fluid":
                        if (_systemManager.getSystem(SystemType.Fluid) == null)
                            _systemManager.add(new FluidSystem(_systemManager, _entityManager), -1);
                        secondPassData.Add(actorData);
                        break;

                    case "Item":
                        _entityManager.factory.createWorldItem(actorData);
                        break;

                    case "PlayerSpawn":
                        if (_systemManager.getSystem(SystemType.CharacterMovement) == null)
                        {
                            _systemManager.add(new CharacterMovementSystem(_systemManager, _entityManager), -1);
                            (_systemManager.getSystem(SystemType.Camera) as CameraSystem).enableManualMovement = false;
                        }
                        (_systemManager.getSystem(SystemType.Player) as PlayerSystem).spawnPosition = Loader.loadVector2(actorData.Attribute("position"), Vector2.Zero);
                        //_systemManager.add(new EquipmentSystem(_systemManager, _entityManager), -1);
                        break;

                    case "Rope":
                        secondPassData.Add(actorData);
                        break;

                    case "Terrain":
                        _entityManager.factory.createTerrain(actorData);
                        break;

                    case "Tree":
                        if (_systemManager.getSystem(SystemType.Tree) == null)
                        {
                            _systemManager.add(new TreeSystem(_systemManager, _entityManager), -1);
                        }
                        _entityManager.factory.createTree(actorData);
                        break;

                    case "Revolute":
                        secondPassData.Add(actorData);
                        break;

                    case "Prismatic":
                        secondPassData.Add(actorData);
                        break;

                    case "CollisionFilter":
                        secondPassData.Add(actorData);
                        break;

                    case "Goal":
                        _entityManager.factory.createGoal(actorData);
                        break;
                }
            }

            // Second pass
            foreach (XElement actorData in secondPassData)
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Circuit":
                        if (_systemManager.getSystem(SystemType.Circuit) == null)
                        {
                            _systemManager.add(new CircuitSystem(_systemManager, _entityManager), -1);
                        }
                        _entityManager.factory.createCircuit(actorData);
                        break;

                    case "Fluid":
                        _entityManager.factory.createFluid(actorData);
                        break;

                    case "Rope":
                        _entityManager.factory.createRope(actorData);
                        break;

                    case "Revolute":
                        _entityManager.factory.createRevoluteJoint(actorData);
                        break;

                    case "Prismatic":
                        _entityManager.factory.createPrismaticJoint(actorData);
                        break;

                    case "CollisionFilter":
                        _entityManager.factory.createCollisionFilter(actorData);
                        break;
                }
            }

            // Reset factory
            _entityManager.factory.reset();

            // Script hook
            _scriptManager.onLevelStart(levelUID);
        }

        public void removeSystems()
        {
            _systemManager.remove(SystemType.Input);
            _systemManager.remove(SystemType.Physics);
            _systemManager.remove(SystemType.LevelGoal);
            _systemManager.remove(SystemType.Camera);
            _systemManager.remove(SystemType.Event);
            _systemManager.remove(SystemType.Render);
            _systemManager.remove(SystemType.Rope);
            _systemManager.remove(SystemType.Fluid);
            _systemManager.remove(SystemType.Tree);
            _systemManager.remove(SystemType.CharacterMovement);
        }

        public void update(GameTime gameTime)
        {
        }

        public void draw(GameTime gameTime)
        {
            _renderSystem.draw();
        }
    }
}
