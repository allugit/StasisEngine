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
        virtual public Vector2 circuitConnectionPosition { get { return Vector2.Zero; } }
        virtual public Vector2 revoluteConnectionPosition { get { return Vector2.Zero; } }
        virtual public Vector2 prismaticConnectionPosition { get { return Vector2.Zero; } }
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

        virtual public void select()
        {
            System.Diagnostics.Debug.Assert(_level.controller.selectedActor == null);
            _level.controller.selectedActor = this;
        }

        virtual public void deselect()
        {
            _level.controller.selectedActor = null;
        }

        virtual public void delete()
        {
            if (selected)
                deselect();

            _level.removeActor(this);
        }

        virtual public void handleMouseDown()
        {
            if (selected)
                deselect();
            else
                select();
        }

        abstract public bool hitTest(Vector2 testPoint);

        virtual public void update()
        {
        }

        abstract public void draw();
    }
}
