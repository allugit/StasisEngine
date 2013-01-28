using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;
using StasisCore;

namespace StasisEditor.Models
{
    abstract public class EditorActor
    {
        protected LevelController _levelController;

        protected ActorType _type;
        protected int _id;
        protected float _layerDepth;

        public ActorType type { get { return _type; } set { _type = value; } }
        public int id { get { return _id; } set { _id = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
        virtual public XElement data
        {
            get
            {
                return new XElement("Actor",
                    new XAttribute("type", _type),
                    new XAttribute("id", _id),
                    new XAttribute("layer_depth", _layerDepth));
            }
        }

        public EditorActor(LevelController levelController, ActorType type, int id)
        {
            _levelController = levelController;
            _type = type;
            _id = id;
        }

        public EditorActor(LevelController levelController, XElement data)
        {
            _levelController = levelController;

            _type = (ActorType)Loader.loadEnum(typeof(ActorType), data.Attribute("type"), (int)ActorType.Box);
            _id = int.Parse(data.Attribute("id").Value);
            _layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0f);
        }

        abstract public void draw();
    }
}
