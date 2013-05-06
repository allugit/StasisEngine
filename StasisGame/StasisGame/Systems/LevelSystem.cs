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
        private Dictionary<int, Goal> _regionGoals;
        private Dictionary<GameEventType, Dictionary<int, Goal>> _eventGoals;
        private List<Goal> _completedGoals;
        private List<Goal> _requiredGoals;

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
            _regionGoals = new Dictionary<int, Goal>();
            _eventGoals = new Dictionary<GameEventType, Dictionary<int, Goal>>();
            _completedGoals = new List<Goal>();
            _requiredGoals = new List<Goal>();
        }

        // load -- Loads a level
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

            // Reserve actor ids as entity ids
            foreach (XElement actorData in data.Elements("Actor"))
                _entityManager.reserveEntityId(int.Parse(actorData.Attribute("id").Value));

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
                        _entityManager.factory.createRegionGoal(actorData);
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

            // Reset entity manager and entity factory -- TODO: move this to unload() ?
            _entityManager.clearReservedEntityIds();
            _entityManager.factory.reset();

            // Call registerGoals script hook for this level
            _scriptManager.registerGoals(levelUID, this);

            // Call onLevelStart script hook for this level
            _scriptManager.onLevelStart(levelUID);
        }

        // unload -- Unloads a level
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

        // areAllGoalsComplete -- Determines whether all the required goals are complete
        public bool areAllGoalsComplete()
        {
            return false;
        }

        // registerRegionGoal -- Registers a region of space (a polygon defined in the editor) as a goal
        public void registerRegionGoal(Goal goal, int regionEntityId)
        {
            _regionGoals.Add(regionEntityId, goal);

            if (goal.required)
                _requiredGoals.Add(goal);
        }

        // registerEventGoal -- Registers a specific event from a specific entity as a goal
        public void registerEventGoal(Goal goal, GameEventType eventType, int entityId)
        {
            if (!_eventGoals.ContainsKey(eventType))
                _eventGoals.Add(eventType, new Dictionary<int, Goal>());

            _eventGoals[eventType].Add(entityId, goal);

            if (goal.required)
                _requiredGoals.Add(goal);
        }

        // completeRegionGoal -- Handles completion of a region goal
        public void completeRegionGoal(int regionEntityId)
        {
            Goal goal = null;
            ScriptBase script = null;

            if (_regionGoals.TryGetValue(regionEntityId, out goal))
            {
                if (!_completedGoals.Contains(goal))
                {
                    _completedGoals.Add(goal);
                    if (_scriptManager.scripts.TryGetValue(_uid, out script))
                    {
                        script.onGoalComplete(goal);
                    }
                }
            }
        }

        // tryCompleteEventGoal -- Tries to handle the completion of an event goal
        public void tryCompleteEventGoal(GameEvent e)
        {
            Goal goal = null;
            ScriptBase script = null;
            Dictionary<int, Goal> entityGoalMap;

            if (_eventGoals.TryGetValue(e.type, out entityGoalMap))
            {
                if (entityGoalMap.TryGetValue(e.originEntityId, out goal))
                {
                    _completedGoals.Add(goal);
                    if (_scriptManager.scripts.TryGetValue(_uid, out script))
                    {
                        script.onGoalComplete(goal);
                    }
                }
            }
        }

        // update
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

        // draw
        public void draw()
        {
            _renderSystem.draw();
        }
    }
}
