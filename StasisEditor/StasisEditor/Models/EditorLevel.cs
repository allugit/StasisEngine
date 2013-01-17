using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers.Actors;
using StasisCore.Resources;
using StasisCore.Models;
using StasisCore.Controllers;

namespace StasisEditor.Models
{
    public class EditorLevel : Level
    {
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
        public EditorLevel(string name) : base(name)
        {
            _actorControllers = new List<ActorController>();
        }

        // Load actors
        protected override void loadActors(XElement data)
        {
            throw new NotImplementedException();
        }

        // Save
        public void save()
        {
            string filePath = ResourceController.levelPath + "\\" + _name + ".xml";
            if (File.Exists(filePath))
                File.Move(filePath, filePath + ".bak");

            XDocument doc = new XDocument();
            doc.Add(data);
            doc.Save(filePath);
        }
    }
}
