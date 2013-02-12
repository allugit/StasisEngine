using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisCore;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class PhysicsSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private World _world;
        private float _dt = 1f / 60f;

        public World world { get { return _world; } }
        public int defaultPriority { get { return 10; } }
        public SystemType systemType { get { return SystemType.Physics; } }

        public PhysicsSystem(SystemManager systemManager, EntityManager entityManager, XElement data)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;

            // Create world
            _world = new World(Loader.loadVector2(data.Attribute("gravity"), new Vector2(0, 32)), true);
        }

        public void update()
        {
            _world.Step(_dt, 12, 8);
        }
    }
}
