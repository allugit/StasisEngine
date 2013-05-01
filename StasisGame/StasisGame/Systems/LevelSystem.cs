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
using StasisGame.UI;

namespace StasisGame.Systems
{
    public class LevelSystem : ISystem
    {
        private LoderGame _game;
        private string _uid;
        private EntityManager _entityManager;
        private RenderSystem _renderSystem;
        private SystemManager _systemManager;
        private ScriptManager _scriptManager;
        private bool _isActive;
        private bool _paused;
        private bool _singleStep;
        private List<LevelGoalComponent> _registeredGoals;
        private List<LevelGoalComponent> _completedGoals;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Level; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public string uid { get { return _uid; } }
        public bool isActive { get { return _isActive; } }

        public LevelSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _scriptManager = _game.scriptManager;
            _registeredGoals = new List<LevelGoalComponent>();
            _completedGoals = new List<LevelGoalComponent>();
        }

        public void load(string levelUID)
        {
            _uid = levelUID;

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

            _isActive = true;

            // Reset factory
            _entityManager.factory.reset();

            // Script hook
            _scriptManager.onLevelStart(levelUID);
        }

        public void unload()
        {
            List<int> entitiesToPreserve = new List<int>();
            PlayerSystem playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);
            ScreenSystem screenSystem = (ScreenSystem)_systemManager.getSystem(SystemType.Screen);

            _isActive = false;
            entitiesToPreserve.Add(playerSystem.playerId);
            _entityManager.removeLevelComponentsFromPlayer(playerSystem.playerId);
            _entityManager.killAllEntities(entitiesToPreserve);
            screenSystem.removeScreen(ScreenType.Level);
            _systemManager.remove(SystemType.Input);
            _systemManager.remove(SystemType.Physics);
            _systemManager.remove(SystemType.Camera);
            _systemManager.remove(SystemType.Event);
            _systemManager.remove(SystemType.Render);
            _systemManager.remove(SystemType.Rope);
            _systemManager.remove(SystemType.Fluid);
            _systemManager.remove(SystemType.Tree);
            _systemManager.remove(SystemType.CharacterMovement);
        }

        public bool areAllGoalsComplete()
        {
            return _completedGoals.Count >= _registeredGoals.Count;
        }

        public void registerGoal(LevelGoalComponent goal)
        {
            _registeredGoals.Add(goal);
        }

        public void completeGoal(LevelGoalComponent goal)
        {
            if (!_completedGoals.Contains(goal))
            {
                Console.WriteLine("Completed a goal: {0}", goal);
                _completedGoals.Add(goal);
            }
        }

        public void update()
        {
            if (_isActive)
            {
                if (areAllGoalsComplete())
                {
                    Console.WriteLine("All goals complete!");

                    unload();
                    _game.openWorldMap();
                    _scriptManager.onReturnToWorldMap(_uid, this);
                }
            }
        }

        public void draw()
        {
            _renderSystem.draw();
        }
    }
}
