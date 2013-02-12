using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using StasisGame.Systems;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame
{
    public class EntityFactory
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public EntityFactory(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void createBox(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
            _entityManager.addComponent(entityId, new PhysicsComponent(world, data));
            _entityManager.addComponent(entityId, new BodyRenderComponent(
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
