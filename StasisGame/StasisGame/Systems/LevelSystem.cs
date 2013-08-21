using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
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
        public static string currentLevelUid;
        private LoderGame _game;
        private EntityManager _entityManager;
        private SystemManager _systemManager;
        private ScriptManager _scriptManager;
        private RenderSystem _renderSystem;
        private PlayerSystem _playerSystem;
        private bool _paused;
        private bool _singleStep;
        //private Dictionary<int, Goal> _regionGoals;
        //private Dictionary<GameEventType, Dictionary<int, Goal>> _eventGoals;
        //private Dictionary<int, Goal> _completedGoals;
        private AABB _levelBoundary;
        private Vector2 _boundaryMargin;
        private Dictionary<string, XElement> _levelsData;
        private Dictionary<string, List<XElement>> _firstPassEntities;
        private Dictionary<string, List<XElement>> _secondPassEntities;
        private Dictionary<string, int> _numEntities;
        private Dictionary<string, int> _numEntitiesProcessed;
        private Dictionary<string, bool> _finishedLoading;
        private Dictionary<string, Background> _backgrounds;
        private Dictionary<string, Vector2> _spawnPositions;
        private KeyboardState _newKeyState;
        private KeyboardState _oldKeyState;
        private List<string> _loadedLevels;
        private bool _finalized;
        private string _lastLevelUidLoaded;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Level; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public bool finalized { get { return _finalized; } set { _finalized = value; } }
        public string lastLevelUidLoaded { get { return _lastLevelUidLoaded; } }
        public bool isFinishedLoading
        {
            get
            {
                foreach (bool finished in _finishedLoading.Values)
                {
                    if (!finished)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public int totalEntitiesCount
        {
            get
            {
                int sum = 0;

                foreach (List<XElement> actorsData in _firstPassEntities.Values)
                {
                    sum += actorsData.Count;
                }
                foreach (List<XElement> actorsData in _secondPassEntities.Values)
                {
                    sum += actorsData.Count;
                }
                return sum;
            }
        }
        public int totalEntitiesProcessedCount
        {
            get
            {
                int sum = 0;
                foreach (int count in _numEntitiesProcessed.Values)
                {
                    sum += count;
                }
                return sum;
            }
        }

        public LevelSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _scriptManager = _game.scriptManager;
            //_regionGoals = new Dictionary<int, Goal>();
            //_eventGoals = new Dictionary<GameEventType, Dictionary<int, Goal>>();
            //_completedGoals = new Dictionary<int, Goal>();
            _levelBoundary = new AABB();
            _boundaryMargin = new Vector2(50f, 50f);
            _levelsData = new Dictionary<string, XElement>();
            _firstPassEntities = new Dictionary<string, List<XElement>>();
            _secondPassEntities = new Dictionary<string, List<XElement>>();
            _numEntities = new Dictionary<string, int>();
            _numEntitiesProcessed = new Dictionary<string, int>();
            _backgrounds = new Dictionary<string, Background>();
            _finishedLoading = new Dictionary<string, bool>();
            _spawnPositions = new Dictionary<string, Vector2>();
            _loadedLevels = new List<string>();
        }

        // getPlayerSpawn -- Gets a player spawn position for a specific level
        public Vector2 getPlayerSpawn(string levelUid)
        {
            return _spawnPositions[levelUid];
        }

        // expandBoundary
        public void expandBoundary(Vector2 point)
        {
            _levelBoundary.LowerBound = Vector2.Min(point - _boundaryMargin, _levelBoundary.LowerBound);
            _levelBoundary.UpperBound = Vector2.Max(point + _boundaryMargin, _levelBoundary.UpperBound);
        }

        // initializeLevelData -- Takes a level's data and initializes the states needed for the loading process
        private void initializeLevelData(XElement levelData)
        {
            string levelUid = levelData.Attribute("name").Value;
            int levelEntityCount = 0;
            PhysicsSystem physicsSystem = _systemManager.getSystem(SystemType.Physics) as PhysicsSystem;

            _finalized = false;
            _paused = true;
            _levelsData.Add(levelUid, levelData);
            _firstPassEntities.Add(levelUid, new List<XElement>());
            _secondPassEntities.Add(levelUid, new List<XElement>());
            _numEntitiesProcessed.Add(levelUid, 0);
            _finishedLoading.Add(levelUid, false);

            // Create worlds
            physicsSystem.addWorld(levelUid, Loader.loadVector2(levelData.Attribute("gravity"), Vector2.Zero));

            // Parse actors
            foreach (XElement actorData in levelData.Elements("Actor"))
            {
                string type = actorData.Attribute("type").Value;

                // Sort entities into first and second passses
                if (type == "Circuit" || type == "Fluid" || type == "Rope" || type == "Revolute" || type == "Prismatic" || type == "CollisionFilter" || type == "Decal")
                {
                    _secondPassEntities[levelUid].Add(actorData);
                }
                else
                {
                    _firstPassEntities[levelUid].Add(actorData);
                }
                levelEntityCount++;

                // Reserve actor ids as entity ids
                _entityManager.reserveEntityId(levelUid, int.Parse(actorData.Attribute("id").Value));
            }

            _numEntities.Add(levelUid, levelEntityCount);
        }

        // loadAllData
        public void loadAllData(string parentLevelUid)
        {
            List<string> levelDependencies = new List<string>();

            // Load parent level
            _lastLevelUidLoaded = parentLevelUid;
            _loadedLevels.Add(parentLevelUid);
            using (Stream stream = TitleContainer.OpenStream(ResourceManager.levelPath + string.Format("\\{0}.xml", parentLevelUid)))
            {
                XDocument doc = XDocument.Load(stream);
                XElement parentLevelData = doc.Element("Level");

                initializeLevelData(parentLevelData);

                // Discover level dependencies -- TODO: Make this recursive, in case the level dependencies have dependencies of their own
                foreach (XElement actorData in parentLevelData.Elements("Actor"))
                {
                    if (actorData.Attribute("type").Value == "LevelTransition")
                    {
                        string levelDependencyUid = actorData.Attribute("level_uid").Value;

                        _loadedLevels.Add(levelDependencyUid);
                        levelDependencies.Add(levelDependencyUid);
                    }
                }
            }

            // Load level dependencies
            foreach (string levelDependencyUid in levelDependencies)
            {
                using (Stream stream = TitleContainer.OpenStream(ResourceManager.levelPath + string.Format("\\{0}.xml", levelDependencyUid)))
                {
                    XDocument doc = XDocument.Load(stream);
                    XElement levelDependencyData = doc.Element("Level");

                    initializeLevelData(levelDependencyData);
                }
            }
        }

        // Create systems
        public void createLevelSystems()
        {
            _systemManager.add(new InputSystem(_systemManager, _entityManager), -1);
            _systemManager.add(new PhysicsSystem(_systemManager, _entityManager), -1);
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
        public void createOutputGates(string levelUid)
        {
            _entityManager.factory.createOutputGates(levelUid, _levelsData[levelUid]);
        }

        // Create background
        public void createBackgrounds()
        {
            foreach (XElement levelData in _levelsData.Values)
            {
                string backgroundUid;
                XElement backgroundData;
                Background background;

                backgroundUid = Loader.loadString(levelData.Attribute("background_uid"), "default_background");
                backgroundData = ResourceManager.getResource(backgroundUid);
                background = new Background(backgroundData);
                background.loadTextures();
                _backgrounds.Add(levelData.Attribute("name").Value, background);
            }
        }

        // loadNextEntity -- Loads the next entity
        private void loadNextEntity(string levelUid)
        {
            LoadingScreen loadingScreen = (_systemManager.getSystem(SystemType.Screen) as ScreenSystem).getScreen(ScreenType.Loading) as LoadingScreen;
            int firstPassEntitiesCount = _firstPassEntities[levelUid].Count;
            int numEntitiesProcessed = _numEntitiesProcessed[levelUid];

            if (numEntitiesProcessed < firstPassEntitiesCount)
            {
                XElement actorData = _firstPassEntities[levelUid][numEntitiesProcessed];

                switch (actorData.Attribute("type").Value)
                {
                    case "Box":
                        _entityManager.factory.createBox(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Circle":
                        _entityManager.factory.createCircle(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Item":
                        _entityManager.factory.createWorldItem(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "PlayerSpawn":
                        Vector2 spawnPosition = Loader.loadVector2(actorData.Attribute("position"), Vector2.Zero);
                        if (_systemManager.getSystem(SystemType.CharacterMovement) == null)
                            _systemManager.add(new CharacterMovementSystem(_systemManager, _entityManager), -1);

                        _spawnPositions.Add(levelUid, spawnPosition);
                        //(_systemManager.getSystem(SystemType.Camera) as CameraSystem).screenCenter = spawnPosition;
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Terrain":
                        _entityManager.factory.createTerrain(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Tree":
                        _entityManager.factory.createTree(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Goal":
                        _entityManager.factory.createRegionGoal(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "LevelTransition":
                        _entityManager.factory.createLevelTransition(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Tooltip":
                        _entityManager.factory.createTooltip(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Waypoint":
                        _entityManager.factory.createWaypoint(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    default:
                        throw new NotImplementedException("Unhandled actor type in loadNextEntity()");
                }
            }
            else
            {
                XElement actorData = _secondPassEntities[levelUid][numEntitiesProcessed - firstPassEntitiesCount];

                switch (actorData.Attribute("type").Value)
                {
                    case "Circuit":
                        if (_systemManager.getSystem(SystemType.Circuit) == null)
                        {
                            _systemManager.add(new CircuitSystem(_systemManager, _entityManager), -1);
                        }
                        _entityManager.factory.createCircuit(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Fluid":
                        _entityManager.factory.createFluid(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Rope":
                        _entityManager.factory.createRope(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Revolute":
                        _entityManager.factory.createRevoluteJoint(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Prismatic":
                        _entityManager.factory.createPrismaticJoint(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "CollisionFilter":
                        _entityManager.factory.createCollisionFilter(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    case "Decal":
                        _entityManager.factory.createDecal(levelUid, actorData);
                        loadingScreen.elementsLoaded++;
                        break;

                    default:
                        throw new NotImplementedException("Unhandled actor type in loadNextEntity()");
                }
            }

            _numEntitiesProcessed[levelUid]++;

            if (_numEntitiesProcessed[levelUid] == _numEntities[levelUid])
            {
                _finishedLoading[levelUid] = true;
            }
        }

        // load -- Called by LoderGame during the LoadingLevel state
        public void load()
        {
            if (!isFinishedLoading)
            {
                foreach (KeyValuePair<string, bool> levelUidFinishedPair in _finishedLoading)
                {
                    if (!levelUidFinishedPair.Value)
                    {
                        loadNextEntity(levelUidFinishedPair.Key);
                        break;
                    }
                }
            }
        }

        // Relax systems
        public void relax()
        {
            // TODO: Relax physics system
            (_systemManager.getSystem(SystemType.Fluid) as FluidSystem).relaxFluid();
        }

        // Clean states set during the level load process. Called directly after level is loaded -- TODO: rename this?
        public void clean()
        {
            _numEntities.Clear();
            _numEntitiesProcessed.Clear();
            _firstPassEntities.Clear();
            _secondPassEntities.Clear();
            _levelsData.Clear();
            _finishedLoading.Clear();
            // Reset entity manager and entity factory -- TODO: move this to unload() ?
            _entityManager.clearReservedEntityIds();
            _entityManager.factory.reset();
        }

        // Reset states used during the previous level. Called directly before a level is loaded
        public void reset()
        {
            _loadedLevels.Clear();
            _spawnPositions.Clear();
        }

        // switchToLevel -- Switch to a different level (must be loaded!)
        public void switchToLevel(string levelUid, Vector2 playerPosition)
        {
            if (currentLevelUid != null)
            {
                _playerSystem.removePlayerFromLevel(levelUid);
            }
            _playerSystem.addPlayerToLevel(levelUid, playerPosition);
            _renderSystem.setBackground(_backgrounds[levelUid]);
            currentLevelUid = levelUid;

            // Update camera
            (_systemManager.getSystem(SystemType.Camera) as CameraSystem).screenCenter = playerPosition;
        }

        // TODO: Refactor(rename) this crap
        public void callScripts()
        {
            /*
            // Call registerGoals script hook for this level
            _scriptManager.registerGoals(currentLevelUid, this);
            */

            foreach (string levelUid in _loadedLevels)
            {
                _scriptManager.onLevelStart(levelUid);
            }
        }

        // isGoalComplete -- Checks if a goal with a specific id has been completed
        public bool isGoalComplete(int goalId)
        {
            return false;
            //return _completedGoals.ContainsKey(goalId);
        }

        // registerRegionGoal -- Registers a region of space (a polygon defined in the editor) as a goal
        public void registerRegionGoal(Goal goal, int regionEntityId)
        {
            //_regionGoals.Add(regionEntityId, goal);
        }

        // registerEventGoal -- Registers a specific event from a specific entity as a goal
        public void registerEventGoal(Goal goal, GameEventType eventType, int entityId)
        {
            /*
            if (!_eventGoals.ContainsKey(eventType))
                _eventGoals.Add(eventType, new Dictionary<int, Goal>());

            _eventGoals[eventType].Add(entityId, goal);*/
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
            ScreenSystem screenSystem = (ScreenSystem)_systemManager.getSystem(SystemType.Screen);

            foreach (string levelUid in _levelsData.Keys)
            {
                List<int> entitiesToPreserve = new List<int>();

                entitiesToPreserve.Add(_playerSystem.playerId);
                _entityManager.killAllEntities(levelUid, entitiesToPreserve);
                //_regionGoals.Clear();
                //_eventGoals.Clear();
                //_completedGoals.Clear();
            }
            ResourceManager.clearCache();
            _renderSystem = null;
            currentLevelUid = null;
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
            _game.openWorldMap();
            _scriptManager.onReturnToWorldMap(currentLevelUid, this);
        }

        // update
        public void update()
        {
            if (!_paused || _singleStep)
            {
                if (_finalized)
                {
                    PhysicsComponent playerPhysicsComponent = (PhysicsComponent)_entityManager.getComponent(currentLevelUid, _playerSystem.playerId, ComponentType.Physics);
                    List<TooltipComponent> tooltipComponents = _entityManager.getComponents<TooltipComponent>(currentLevelUid, ComponentType.Tooltip);
                    List<LevelTransitionComponent> levelTransitionComponents = _entityManager.getComponents<LevelTransitionComponent>(currentLevelUid, ComponentType.LevelTransition);

                    _oldKeyState = _newKeyState;
                    _newKeyState = Keyboard.GetState();

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
                        for (int i = 0; i < tooltipComponents.Count; i++)
                        {
                            TooltipComponent tooltip = tooltipComponents[i];

                            if ((position - tooltip.position).LengthSquared() <= tooltip.radiusSq)
                            {
                                tooltip.draw = true;
                            }
                        }

                        // Check player's position against level transitions
                        for (int i = 0; i < levelTransitionComponents.Count; i++)
                        {
                            LevelTransitionComponent levelTransition = levelTransitionComponents[i];

                            if ((position - levelTransition.position).LengthSquared() <= levelTransition.radiusSq)
                            {
                                if (levelTransition.requiresActivation)
                                {
                                    if (_newKeyState.IsKeyDown(Keys.E) && _oldKeyState.IsKeyUp(Keys.E))
                                    {
                                        Console.WriteLine("begin level transition to: {0}", levelTransition.levelUid);
                                        switchToLevel(levelTransition.levelUid, levelTransition.positionInLevel);
                                    }
                                }
                                else 
                                {
                                    Console.WriteLine("begin level transition to: {0}", levelTransition.levelUid);
                                }
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
