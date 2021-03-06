﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class TreeSystem : ISystem
    {
        public const int NUM_LEAF_VARIATIONS = 3;
        public const int NUM_LEAF_GROUPS = 14;
        public const float PLANT_CELL_SIZE = 0.8f;
        public const int MARKERS_PER_CELL = 1;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private PhysicsSystem _physicsSystem;
        private RenderSystem _renderSystem;
        private Vector2 _gravity = new Vector2(0, 0.005f);
        private Vector2 _brokenGravity = new Vector2(0, 0.02f);
        private Dictionary<string, Dictionary<int, Dictionary<int, MarkerCell>>> _markerGrid;
        private Dictionary<string, Dictionary<int, Dictionary<int, List<Metamer>>>> _metamerGrid;
        private bool _paused;
        private bool _singleStep;
        public AABB treeAABB;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Tree; } }
        public Dictionary<string, Dictionary<int, Dictionary<int, MarkerCell>>> markerGrid { get { return _markerGrid; } }
        public Dictionary<string, Dictionary<int, Dictionary<int, List<Metamer>>>> metamerGrid { get { return _metamerGrid; } }
        public PhysicsSystem physicsSystem { get { return _physicsSystem; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public LevelSystem levelSystem { get { return (LevelSystem)_systemManager.getSystem(SystemType.Level); } }
        public EntityManager entityManager { get { return _entityManager; } }
        public SystemManager systemManager { get { return _systemManager; } }

        public TreeSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _physicsSystem = _systemManager.getSystem(SystemType.Physics) as PhysicsSystem;
            _renderSystem = _systemManager.getSystem(SystemType.Render) as RenderSystem;
            _markerGrid = new Dictionary<string, Dictionary<int, Dictionary<int, MarkerCell>>>();
            _metamerGrid = new Dictionary<string, Dictionary<int, Dictionary<int, List<Metamer>>>>();
        }

        public int getPlantGridX(float x) { return (int)Math.Floor(x / PLANT_CELL_SIZE); }
        public int getPlantGridY(float y) { return (int)Math.Floor(y / PLANT_CELL_SIZE); }

        public Metamer findMetamer(string levelUid, Vector2 position)
        {
            Dictionary<int, Dictionary<int, List<Metamer>>> levelMetamerGrid;
            Dictionary<int, List<Metamer>> gridX;
            List<Metamer> gridY;
            int i = getPlantGridX(position.X);
            int j = getPlantGridY(position.Y);
            int padding = 2;
            float shortestDistanceSq = 99999f;
            Metamer result = null;

            if (metamerGrid.TryGetValue(levelUid, out levelMetamerGrid))
            {
                for (int x = i - padding; x < i + padding; x++)
                {
                    for (int y = j - padding; y < j + padding; y++)
                    {
                        if (levelMetamerGrid.TryGetValue(x, out gridX) && gridX.TryGetValue(y, out gridY))
                        {
                            for (int n = 0; n < gridY.Count; n++)
                            {
                                if (gridY[n].isBranchingPoint() || gridY[n].isTail)
                                {
                                    float distanceSq = (gridY[n].position - position).LengthSquared();
                                    if (distanceSq < shortestDistanceSq)
                                    {
                                        shortestDistanceSq = distanceSq;
                                        result = gridY[n];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void update(GameTime gameTime)
        {
            if (!_paused || _singleStep)
            {
                string levelUid = LevelSystem.currentLevelUid;
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    List<int> treeEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Tree);

                    // Update treeAABB
                    Vector2 screenCenter = _renderSystem.screenCenter;
                    float halfWidth = ((float)_renderSystem.screenWidth / _renderSystem.scale) / 2f;
                    float halfHeight = ((float)_renderSystem.screenHeight / _renderSystem.scale) / 2f;
                    treeAABB.LowerBound.X = screenCenter.X - halfWidth;
                    treeAABB.UpperBound.X = screenCenter.X + halfWidth;
                    treeAABB.LowerBound.Y = screenCenter.Y - halfHeight;
                    treeAABB.UpperBound.Y = screenCenter.Y + halfHeight;

                    prepareCollisions();

                    for (int i = 0; i < treeEntities.Count; i++)
                    {
                        TreeComponent treeComponent = (TreeComponent)_entityManager.getComponent(levelUid, treeEntities[i], ComponentType.Tree);
                        treeComponent.tree.update();
                    }
                }
            }
            _singleStep = false;
        }

        // prepareCollisions
        private void prepareCollisions()
        {
            string levelUid = LevelSystem.currentLevelUid;
            Dictionary<int, Dictionary<int, List<Metamer>>> levelMetamerGrid;
            Dictionary<int, List<Metamer>> gridX;
            List<Metamer> gridY;

            // Query the world using the screen's AABB
            _physicsSystem.getWorld(levelUid).QueryAABB((Fixture fixture) =>
            {
                // Skip certain collisions
                int entityId = (int)fixture.Body.UserData;
                if (_entityManager.getComponent(levelUid, entityId, ComponentType.IgnoreTreeCollision) != null)
                    return true;

                AABB aabb;
                Transform transform;
                fixture.Body.GetTransform(out transform);
                fixture.Shape.ComputeAABB(out aabb, ref transform, 0);
                int Ax = getPlantGridX(aabb.LowerBound.X);
                int Ay = getPlantGridY(aabb.LowerBound.Y);
                int Bx = getPlantGridX(aabb.UpperBound.X) + 1;
                int By = getPlantGridY(aabb.UpperBound.Y) + 1;
                if (_metamerGrid.TryGetValue(levelUid, out levelMetamerGrid))
                {
                    for (int i = Ax; i < Bx; i++)
                    {
                        for (int j = Ay; j < By; j++)
                        {
                            if (levelMetamerGrid.TryGetValue(i, out gridX) && gridX.TryGetValue(j, out gridY))
                            {
                                for (int n = 0; n < gridY.Count; n++)
                                {
                                    Metamer metamer = gridY[n];
                                    if (metamer.numFixturesToTest < Metamer.MAX_FIXTURES_TO_TEST)
                                    {
                                        metamer.fixturesToTest[metamer.numFixturesToTest] = fixture;
                                        metamer.numFixturesToTest++;
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            },
                ref treeAABB);
        }
    }
}
