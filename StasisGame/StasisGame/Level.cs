﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisCore.Models;
using StasisCore.Controllers;
using StasisGame.Systems;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame
{
    public class Level
    {
        private LoderGame _game;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private RenderSystem _renderSystem;

        public Level(LoderGame game, string filePath)
        {
            _game = game;
            _systemManager = new SystemManager();
            _entityManager = new EntityManager(_systemManager);

            XElement data = null;
            List<XElement> secondPassData = new List<XElement>();

            // Load xml
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                XDocument doc = XDocument.Load(stream);
                data = doc.Element("Level");
            }

            // Create systems
            _systemManager.add(new InputSystem(_systemManager, _entityManager));
            _systemManager.add(new PhysicsSystem(_systemManager, _entityManager, data));
            _systemManager.add(new CameraSystem(_systemManager, _entityManager));
            _systemManager.add(new EventSystem(_systemManager, _entityManager));
            _renderSystem = new RenderSystem(_game, _systemManager, _entityManager);
            _systemManager.add(_renderSystem);

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
                            _systemManager.add(new FluidSystem(_systemManager, _entityManager));
                        secondPassData.Add(actorData);
                        break;

                    case "Item":
                        _entityManager.factory.createWorldItem(actorData);
                        break;

                    case "PlayerSpawn":
                        if (_systemManager.getSystem(SystemType.CharacterMovement) == null)
                        {
                            _systemManager.add(new CharacterMovementSystem(_systemManager, _entityManager));
                        }
                        _systemManager.add(new PlayerSystem(_systemManager, _entityManager));
                        _entityManager.factory.createPlayer(actorData);
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
                            _systemManager.add(new TreeSystem(_systemManager, _entityManager));
                        }
                        _entityManager.factory.createTree(actorData);
                        break;

                    case "Revolute":
                        secondPassData.Add(actorData);
                        break;

                    case "Prismatic":
                        secondPassData.Add(actorData);
                        break;
                }
            }

            // Second pass
            foreach (XElement actorData in secondPassData)
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Circuit":
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
                }
            }
        }

        public void update(GameTime gameTime)
        {
            _systemManager.process();
        }

        public void draw(GameTime gameTime)
        {
            _renderSystem.draw();
        }
    }
}
