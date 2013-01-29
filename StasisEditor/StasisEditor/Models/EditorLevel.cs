using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;
using StasisCore.Resources;
using StasisCore.Models;
using StasisCore.Controllers;

namespace StasisEditor.Models
{
    public class EditorLevel : Level
    {
        private LevelController _controller;
        private List<EditorActor> _actors;

        [Browsable(false)]
        public List<EditorActor> actors { get { return _actors; } }
        public XElement data
        {
            get
            {
                List<XElement> actorControllerData = new List<XElement>();
                foreach (EditorActor actor in _actors)
                    actorControllerData.Add(actor.data);

                XElement d = new XElement("Level",
                    new XAttribute("name", _name),
                    new XAttribute("gravity", _gravity),
                    new XAttribute("wind", _wind),
                    actorControllerData);

                return d;
            }
        }
        [Browsable(false)]
        public LevelController controller { get { return _controller; } }

        // Create new
        public EditorLevel(LevelController levelController, string name) : base(name)
        {
            _controller = levelController;
            _actors = new List<EditorActor>();
        }

        // Load from xml
        public EditorLevel(LevelController levelController, XElement data) : base(data)
        {
            _controller = levelController;
            List<XElement> secondPassData = new List<XElement>();

            // First pass -- load independent actors
            foreach (XElement actorData in data.Elements("Actor"))
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Box":
                        break;

                    case "Circle":
                        break;

                    case "Circuit":
                        secondPassData.Add(actorData);
                        break;

                    case "Fluid":
                        break;

                    case "Item":
                        break;

                    case "MovingPlatform":
                        break;

                    case "PlayerSpawn":
                        break;

                    case "Rope":
                        break;

                    case "Terrain":
                        break;

                    case "Tree":
                        break;
                }
            }

            // Second pass -- load dependent actors
            foreach (XElement actorData in secondPassData)
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Circuit":
                        EditorCircuit circuit = _controller.editorController.circuitController.getCircuit(actorData.Attribute("circuit_uid").Value);
                        break;
                }
            }
        }

        // Get actor by id
        public EditorActor getActor(int id)
        {
            foreach (EditorActor actor in _actors)
            {
                if (actor.id == id)
                    return actor;
            }
            return null;
        }

        // Add an actor
        public void addActor(EditorActor actor)
        {
            _actors.Add(actor);
        }

        // Remove an actor
        public void removeActor(EditorActor actor)
        {
            _actors.Remove(actor);
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

        // Update
        public void update()
        {
            foreach (EditorActor actor in _actors)
            {
                actor.update();
            }
        }

        // Draw
        public void draw()
        {
            foreach (EditorActor actor in _actors)
            {
                actor.draw();
            }
        }
    }
}
