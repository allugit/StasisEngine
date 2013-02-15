using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class TreeSystem : ISystem
    {
        public const float PLANT_CELL_SIZE = 0.8f;
        public const int MARKERS_PER_CELL = 1;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private PhysicsSystem _physicsSystem;
        private Vector2 _gravity = new Vector2(0, 0.005f);
        private Vector2 _brokenGravity = new Vector2(0, 0.02f);
        private Dictionary<int, Dictionary<int, MarkerCell>> _markerGrid;
        private Dictionary<int, Dictionary<int, List<Metamer>>> _metamerGrid;
        public AABB treeAABB;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Tree; } }
        public Dictionary<int, Dictionary<int, MarkerCell>> markerGrid { get { return _markerGrid; } }
        public Dictionary<int, Dictionary<int, List<Metamer>>> metamerGrid { get { return _metamerGrid; } }
        public PhysicsSystem physicsSystem { get { return _physicsSystem; } }

        public TreeSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _physicsSystem = _systemManager.getSystem(SystemType.Physics) as PhysicsSystem;
        }

        public int getPlantGridX(float x) { return (int)Math.Floor(x / PLANT_CELL_SIZE); }
        public int getPlantGridY(float y) { return (int)Math.Floor(y / PLANT_CELL_SIZE); }

        public void update()
        {
        }

        // prepareCollisions
        private void prepareCollisions()
        {
            Dictionary<int, List<Metamer>> gridX;
            List<Metamer> gridY;

            // Query the world using the screen's AABB
            _physicsSystem.world.QueryAABB((FixtureProxy fixtureProxy) =>
            {
                // Skip certain collisions
                /*
                UserData data = fixtureProxy.fixture.GetBody().GetUserData() as UserData;
                if (data.actorType == ActorType.LIMB || data.actorType == ActorType.WALL_GROUP ||
                    data.actorType == ActorType.GROUND || data.actorType == ActorType.GRENADE ||
                    data.actorType == ActorType.PLAYER ||
                    data.actorType == ActorType.GRAVITY_WELL || data.actorType == ActorType.EDGE_GROUP)
                    return true;
                */

                int Ax = getPlantGridX(fixtureProxy.aabb.lowerBound.X);
                int Ay = getPlantGridY(fixtureProxy.aabb.lowerBound.Y);
                int Bx = getPlantGridX(fixtureProxy.aabb.upperBound.X) + 1;
                int By = getPlantGridY(fixtureProxy.aabb.upperBound.Y) + 1;
                for (int i = Ax; i < Bx; i++)
                {
                    for (int j = Ay; j < By; j++)
                    {
                        if (metamerGrid.TryGetValue(i, out gridX) && gridX.TryGetValue(j, out gridY))
                        {
                            for (int n = 0; n < gridY.Count; n++)
                            {
                                Metamer metamer = gridY[n];
                                if (metamer.numFixturesToTest < Metamer.MAX_FIXTURES_TO_TEST)
                                {
                                    metamer.fixturesToTest[metamer.numFixturesToTest] = fixtureProxy.fixture;
                                    metamer.numFixturesToTest++;
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
