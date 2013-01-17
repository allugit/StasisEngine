using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Console.WriteLine("save level to: {0}", ResourceController.levelPath + "\\" + _name + ".xml");
        }
    }
}
