using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Box2D.XNA;
using StasisGame.Systems;
using StasisGame.Components;

namespace StasisGame.Managers
{
    public class TemplateManager
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public TemplateManager(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void createBox(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
            _entityManager.addComponent(entityId, new PhysicsComponent(world, data));
        }

        public void createCircle(XElement data)
        {
        }

        public void createItem(XElement data)
        {
        }

        public void createRope(XElement data)
        {
        }

        public void createTerrain(XElement data)
        {
        }

        public void createTree(XElement data)
        {
        }
    }
}
