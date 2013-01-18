using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;
using StasisEditor.Controllers.Actors;
using StasisCore.Resources;
using StasisCore.Models;
using StasisCore.Controllers;

namespace StasisEditor.Models
{
    public class EditorLevel : Level
    {
        private LevelController _levelController;
        private List<ActorController> _actorControllers;

        [Browsable(false)]
        public List<ActorController> actorControllers { get { return _actorControllers; } }
        public XElement data
        {
            get
            {
                List<XElement> actorControllerData = new List<XElement>();
                foreach (ActorController actorController in _actorControllers)
                    actorControllerData.Add(actorController.data);

                XElement d = new XElement("Level",
                    new XAttribute("name", _name),
                    new XAttribute("gravity", _gravity),
                    new XAttribute("wind", _wind),
                    actorControllerData);

                return d;
            }
        }

        // Create new
        public EditorLevel(LevelController levelController, string name) : base(name)
        {
            _levelController = levelController;
            _actorControllers = new List<ActorController>();
        }

        // Load from xml
        public EditorLevel(LevelController levelController, XElement data) : base(data)
        {
            _levelController = levelController;
            _actorControllers = new List<ActorController>();
            foreach (XElement actorData in data.Elements("Actor"))
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Box":
                        _actorControllers.Add(new BoxActorController(_levelController, actorData));
                        break;

                    case "Circle":
                        _actorControllers.Add(new CircleActorController(_levelController, actorData));
                        break;

                    case "Circuit":
                        throw new NotImplementedException();
                        break;

                    case "Fluid":
                        _actorControllers.Add(new FluidActorController(_levelController, actorData));
                        break;

                    case "Item":
                        _actorControllers.Add(new ItemActorController(_levelController, actorData));
                        break;

                    case "MovingPlatform":
                        _actorControllers.Add(new PlatformActorController(_levelController, actorData));
                        break;

                    case "PlayerSpawn":
                        _actorControllers.Add(new PlayerSpawnActorController(_levelController, actorData));
                        break;

                    case "Rope":
                        _actorControllers.Add(new RopeActorController(_levelController, actorData));
                        break;

                    case "Terrain":
                        _actorControllers.Add(new TerrainActorController(_levelController, actorData));
                        break;

                    case "Tree":
                        _actorControllers.Add(new TreeActorController(_levelController, actorData));
                        break;
                }
            }
        }

        // Save
        public void save()
        {
            string filePath = ResourceController.levelPath + "\\" + _name + ".xml";
            if (File.Exists(filePath))
            {
                string backupFilePath = filePath + ".bak";
                if (File.Exists(backupFilePath))
                    File.Delete(backupFilePath);
                File.Move(filePath, backupFilePath);
            }

            XDocument doc = new XDocument();
            doc.Add(data);
            doc.Save(filePath);
        }
    }
}
