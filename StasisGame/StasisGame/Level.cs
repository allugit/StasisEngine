﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisGame.Systems;
using StasisGame.Managers;

namespace StasisGame
{
    public class Level
    {
        private LoderGame _game;
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public Level(LoderGame game, string filePath)
        {
            _game = game;
            _systemManager = new SystemManager();
            _entityManager = new EntityManager(_systemManager);

            // Load xml
            XElement data = null;
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                XDocument doc = XDocument.Load(stream);
                data = doc.Element("Level");
            }

            // Create systems
            _systemManager.add(new PhysicsSystem(_systemManager, _entityManager, data));
            _systemManager.add(new RenderSystem(_game, _systemManager, _entityManager));

            // Create entities
            foreach (XElement actorData in data.Elements("Actor"))
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Box":
                        _entityManager.templates.createBox(actorData);
                        break;

                    case "Circle":
                        _entityManager.templates.createCircle(actorData);
                        break;

                    case "Circuit":
                        break;

                    case "Fluid":
                        break;

                    case "Item":
                        _entityManager.templates.createItem(actorData);
                        break;

                    case "PlayerSpawn":
                        break;

                    case "Rope":
                        _entityManager.templates.createRope(actorData);
                        break;

                    case "Terrain":
                        _entityManager.templates.createTerrain(actorData);
                        break;

                    case "Tree":
                        _entityManager.templates.createTree(actorData);
                        break;

                    case "Revolute":
                        break;

                    case "Prismatic":
                        break;
                }
            }
        }

        public void update(GameTime gameTime)
        {
            _systemManager.process();
        }
    }
}
