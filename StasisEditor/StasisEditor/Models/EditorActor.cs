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
        protected EditorLevel _level;

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
        public bool selected { get { return _level.controller.selectedActor == this; } }

        public EditorActor(EditorLevel level, ActorType type, int id)
        {
            _level = level;
            _type = type;
            _id = id;
        }

        public EditorActor(EditorLevel level, XElement data)
        {
            _level = level;

            _type = (ActorType)Loader.loadEnum(typeof(ActorType), data.Attribute("type"), (int)ActorType.Box);
            _id = int.Parse(data.Attribute("id").Value);
            _layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0f);
        }

        virtual public void handleMouseDown()
        {
            if (selected)
            {
                _level.controller.deselectActor();
            }
            else
            {
                _level.controller.selectActor(this);
            }
        }

        virtual public void rotate(Vector2 anchorPoint, float increment)
        {
        }

        abstract public bool hitTest();

        virtual public void update()
        {
        }

        abstract public void draw();
    }
}
