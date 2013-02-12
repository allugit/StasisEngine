using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using StasisCore;
using StasisCore.Models;
using StasisCore.Controllers;
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

        private BodyRenderComponent createBodyRenderComponent(XElement data, PhysicsComponent physicsComponent)
        {
            RenderSystem renderSystem = (RenderSystem)_systemManager.getSystem(SystemType.Render);
            Material material = new Material(ResourceController.getResource(data.Attribute("material_uid").Value));
            Texture texture = renderSystem.materialRenderer.renderMaterial(material, polygonPoints, 1f);

            BodyRenderComponent renderComponent = new BodyRenderComponent(texture, vertices, worldMatrix, primitiveCount, layerDepth);
        }

        public void createBox(XElement data)
        {
            World world = (_systemManager.getSystem(SystemType.Physics) as PhysicsSystem).world;
            int entityId = _entityManager.createEntity();
            PhysicsComponent physicsComponent = new PhysicsComponent(world, data);
            _entityManager.addComponent(entityId, physicsComponent);
            _entityManager.addComponent(entityId, createBodyRenderComponent(data, physicsComponent));
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
