using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision;
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
        private SystemManager _systemManager;
        private ScriptManager _scriptManager;
        private RenderSystem _renderSystem;
        private PlayerSystem _playerSystem;
        private bool _paused;
        private bool _singleStep;
        private Dictionary<int, Goal> _regionGoals;
        private Dictionary<GameEventType, Dictionary<int, Goal>> _eventGoals;
        private Dictionary<int, Goal> _completedGoals;
        private AABB _levelBoundary;
        private Vector2 _boundaryMargin;
        private int _numSecondPassEntitiesLoaded;
        private int _numEntitiesProcessed;
        private bool _firstPassDone;
        private bool _secondPassDone;
        private bool _fullyLoaded;
        private XElement _data;
        private List<XElement> _actorData;
        private List<XElement> _secondPassData;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Level; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public string uid { get { return _uid; } }
        public bool firstPassDone { get { return _firstPassDone; } }
        public bool secondPassDone { get { return _secondPassDone; } }
        public bool fullyLoaded { get { return _fullyLoaded; } set { _fullyLoaded = value; } }
        public int numEntitiesToLoad { get { return _actorData.Count; } }

        public LevelSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _scriptManager = _game.scriptManager;
            _regionGoals = new Dictionary<int, Goal>();
            _eventGoals = new Dictionary<GameEventType, Dictionary<int, Goal>>();
            _completedGoals = new Dictionary<int, Goal>();
            _levelBoundary = new AABB();
            _boundaryMargin = new Vector2(50f, 50f);
        }

        public void expandBoundary(Vector2 point)
        {
            _levelBoundary.LowerBound = Vector2.Min(point - _boundaryMargin, _levelBoundary.LowerBound);
            _levelBoundary.UpperBound = Vector2.Max(point + _boundaryMargin, _levelBoundary.UpperBound);
        }

        // Load data
        public void loadData(string levelUID)
        {
            _numEntitiesProcessed = 0;
            _numSecondPassEntitiesLoaded = 0;
            _firstPassDone = false;
            _secondPassDone = false;
            _fullyLoaded = false;
            _paused = true;
            _uid = levelUID;

            using (Stream stream = TitleContainer.OpenStream(ResourceManager.levelPath + string.Format("\\{0}.xml", levelUID)))
            {
                XDocument doc = XDocument.Load(stream);

                _data = doc.Element("Level");
                _actorData = new List<XElement>(_data.Elements("Actor"));
                _secondPassData = new List<XElement>();

                // Reserve actor ids as entity ids
                foreach (XElement actorData in _actorData)
                    _entityManager.reserveEntityId(int.Parse(actorData.Attribute("id").Value));
            }
        }

        // Create systems
        public void createLevelSystems()
        {
            _systemManager.add(new InputSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new PhysicsSystem(_systemManager, _entityManager, Loader.loadVector2(_data.Attribute("gravity"), new Vector2(0, 32))), -1);
            _systemManager.add(new CameraSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new EventSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new RopeSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new ExplosionSystem(_systemManager, _entityManager), -1);
            _renderSystem = new RenderSystem(_game, _systemManager, _entityManager);
            _systemManager.add(_renderSystem, -1);
            _systemManager.add(new TreeSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new FluidSystem(_systemManager, _entityManager), -1);
            _playerSystem = (PlayerSystem)_systemManager.getSystem(SystemType.Player);
        }

        // Create output gates
        public void createOutputGates()
        {
            _entityManager.factory.createOutputGates(_data);
        }

        // Create background
        public void createBackgroundRenderer()
        {
            string backgroundUID;
            XElement backgroundData;
            Background background;

            backgroundUID = Loader.loadString(_data.Attribute("background_uid"), "default_background");
            backgroundData = ResourceManager.getResource(backgroundUID);
            background = new Background(backgroundData);
            background.loadTextures();
            _renderSystem.setBackground(background);
        }

        // loadEntity -- Loads a level
        public void loadEntity()
        {
            XElement actorData = _actorData[_numEntitiesProcessed];
            LoadingScreen loadingScreen = (_systemManager.getSystem(SystemType.Screen) as ScreenSystem).getScreen(ScreenType.Loading) as LoadingScreen;

            switch (actorData.Attribute("type").Value)
            {
                case "Box":
                    _entityManager.factory.createBox(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Circle":
                    _entityManager.factory.createCircle(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Circuit":
                    _secondPassData.Add(actorData);
                    break;

                case "Fluid":
                    _secondPassData.Add(actorData);
                    break;

                case "Item":
                    _entityManager.factory.createWorldItem(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "PlayerSpawn":
                    Vector2 spawnPosition = Loader.loadVector2(actorData.Attribute("position"), Vector2.Zero);
                    if (_systemManager.getSystem(SystemType.CharacterMovement) == null)
                        _systemManager.add(new CharacterMovementSystem(_systemManager, _entityManager), -1);

                    (_systemManager.getSystem(SystemType.Player) as PlayerSystem).spawnPosition = spawnPosition;
                    (_systemManager.getSystem(SystemType.Camera) as CameraSystem).screenCenter = spawnPosition;
                    _playerSystem.addLevelComponents();
                    loadingScreen.elementsLoaded++;
                    break;

                case "Rope":
                    _secondPassData.Add(actorData);
                    break;

                case "Terrain":
                    _entityManager.factory.createTerrain(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Tree":
                    _entityManager.factory.createTree(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Revolute":
                    _secondPassData.Add(actorData);
                    break;

                case "Prismatic":
                    _secondPassData.Add(actorData);
                    break;

                case "CollisionFilter":
                    _secondPassData.Add(actorData);
                    break;

                case "Goal":
                    _entityManager.factory.createRegionGoal(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Decal":
                    _secondPassData.Add(actorData);
                    break;
            }

            _numEntitiesProcessed++;

            if (_numEntitiesProcessed == _actorData.Count)
            {
                _firstPassDone = true;
                loadingScreen.message = "Loading entities, second pass...";
            }
        }

        public void loadSecondPassEntity()
        {
            XElement actorData = _secondPassData[_numSecondPassEntitiesLoaded];
            LoadingScreen loadingScreen = (_systemManager.getSystem(SystemType.Screen) as ScreenSystem).getScreen(ScreenType.Loading) as LoadingScreen;

            // Second pass
            switch (actorData.Attribute("type").Value)
            {
                case "Circuit":
                    if (_systemManager.getSystem(SystemType.Circuit) == null)
                    {
                        _systemManager.add(new CircuitSystem(_systemManager, _entityManager), -1);
                    }
                    _entityManager.factory.createCircuit(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Fluid":
                    _entityManager.factory.createFluid(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Rope":
                    _entityManager.factory.createRope(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Revolute":
                    _entityManager.factory.createRevoluteJoint(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Prismatic":
                    _entityManager.factory.createPrismaticJoint(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "CollisionFilter":
                    _entityManager.factory.createCollisionFilter(actorData);
                    loadingScreen.elementsLoaded++;
                    break;

                case "Decal":
                    _entityManager.factory.createDecal(actorData);
                    loadingScreen.elementsLoaded++;
                    break;
            }

            _numSecondPassEntitiesLoaded++;

            if (_numSecondPassEntitiesLoaded == _secondPassData.Count)
            {
                _secondPassDone = true;
                loadingScreen.message = "Relaxing fluid, tree, and physics systems...";
            }
        }

        public void relax()
        {
            // TODO: Relax physics system
            (_systemManager.getSystem(SystemType.Fluid) as FluidSystem).relaxFluid();
        }

        public void clean()
        {
            // Reset entity manager and entity factory -- TODO: move this to unload() ?
            _entityManager.clearReservedEntityIds();
            _entityManager.factory.reset();
        }

        public void callScripts()
        {
            // Call registerGoals script hook for this level
            _scriptManager.registerGoals(_uid, this);

            // Call onLevelStart script hook for this level
            _scriptManager.onLevelStart(_uid);
        }

        // unload -- Unloads a level
        public void unload()
        {
            List<int> entitiesToPreserve = new List<int>();
            ScreenSystem screenSystem = (ScreenSystem)_systemManager.getSystem(SystemType.Screen);

            ResourceManager.clearCache();
            entitiesToPreserve.Add(_playerSystem.playerId);
            _playerSystem.removeLevelComponents();
            _entityManager.killAllEntities(entitiesToPreserve);

            _renderSystem = null;

            _regionGoals.Clear();
            _eventGoals.Clear();
            _completedGoals.Clear();

            screenSystem.removeScreen(_game.levelScreen);

            _systemManager.remove(SystemType.Input);
            _systemManager.remove(SystemType.Physics);
            _systemManager.remove(SystemType.Camera);
            _systemManager.remove(SystemType.Event);
            _systemManager.remove(SystemType.Render);
            _systemManager.remove(SystemType.Rope);
            _systemManager.remove(SystemType.Fluid);
            _systemManager.remove(SystemType.Tree);
            _systemManager.remove(SystemType.CharacterMovement);
            _systemManager.remove(SystemType.Explosion);
        }

        // isGoalComplete -- Checks if a goal with a specific id has been completed
        public bool isGoalComplete(int goalId)
        {
            return _completedGoals.ContainsKey(goalId);
        }

        // registerRegionGoal -- Registers a region of space (a polygon defined in the editor) as a goal
        public void registerRegionGoal(Goal goal, int regionEntityId)
        {
            _regionGoals.Add(regionEntityId, goal);
        }

        // registerEventGoal -- Registers a specific event from a specific entity as a goal
        public void registerEventGoal(Goal goal, GameEventType eventType, int entityId)
        {
            if (!_eventGoals.ContainsKey(eventType))
                _eventGoals.Add(eventType, new Dictionary<int, Goal>());

            _eventGoals[eventType].Add(entityId, goal);
        }

        // completeRegionGoal -- Handles completion of a region goal
        public void completeRegionGoal(int regionEntityId)
        {
            Console.WriteLine("TODO: Implement completeRegionGoal() in LevelSystem.");
            /*
            WorldMapSystem worldMapSystem = (WorldMapSystem)_systemManager.getSystem(SystemType.WorldMap);
            Goal goal = null;
            ScriptBase script = null;

            if (_regionGoals.TryGetValue(regionEntityId, out goal))
            {
                if (!_completedGoals.ContainsKey(goal.id))
                {
                    _completedGoals.Add(goal.id, goal);
                    if (_scriptManager.scripts.TryGetValue(_uid, out script))
                    {
                        script.onGoalComplete(worldMapSystem, this, goal);
                    }
                }
            }*/
        }

        // tryCompleteEventGoal -- Tries to handle the completion of an event goal
        public void tryCompleteEventGoal(GameEvent e)
        {
            Console.WriteLine("TODO: Reimplement tryCompleteEventGoal() in LevelSystem");
            /*
            WorldMapSystem worldMapSystem = (WorldMapSystem)_systemManager.getSystem(SystemType.WorldMap);
            Goal goal = null;
            ScriptBase script = null;
            Dictionary<int, Goal> entityGoalMap;

            if (_eventGoals.TryGetValue(e.type, out entityGoalMap))
            {
                if (entityGoalMap.TryGetValue(e.originEntityId, out goal))
                {
                    _completedGoals.Add(goal.id, goal);
                    if (_scriptManager.scripts.TryGetValue(_uid, out script))
                    {
                        script.onGoalComplete(worldMapSystem, this, goal);
                    }
                }
            }*/
        }

        // endLevel
        public void endLevel()
        {
            unload();
            _game.openWorldMap();
            _scriptManager.onReturnToWorldMap(_uid, this);
        }

        // update
        public void update()
        {
            if (!_paused || _singleStep)
            {
                if (_firstPassDone && _secondPassDone)
                {
                    PhysicsComponent playerPhysicsComponent = (PhysicsComponent)_entityManager.getComponent(_playerSystem.playerId, ComponentType.Physics);
                    List<int> tooltipEntities = _entityManager.getEntitiesPosessing(ComponentType.Tooltip);

                    if (playerPhysicsComponent != null)
                    {
                        Vector2 position = playerPhysicsComponent.body.Position;

                        // Check player's position against the level boundary
                        if (position.X < _levelBoundary.LowerBound.X || position.X > _levelBoundary.UpperBound.X ||
                            position.Y < _levelBoundary.LowerBound.Y || position.Y > _levelBoundary.UpperBound.Y)
                        {
                            endLevel();
                            _playerSystem.reloadInventory();
                        }

                        // Check player's position against tooltips
                        for (int i = 0; i < tooltipEntities.Count; i++)
                        {
                            TooltipComponent tooltip = _entityManager.getComponent(tooltipEntities[i], ComponentType.Tooltip) as TooltipComponent;

                            if ((position - tooltip.position).LengthSquared() <= tooltip.radiusSq)
                            {
                                tooltip.draw = true;
                            }
                        }
                    }
                }
            }
            _singleStep = false;
        }

        // draw
        public void draw()
        {
            _renderSystem.draw();
        }
    }
}
