using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;
using StasisCore;

namespace StasisEditor.Models
{
    abstract public class EditorActor
    {
        protected EditorLevel _level;
        public delegate bool HitTestCallback(List<IActorComponent> results);

        protected ActorType _type;
        protected int _id;
        protected float _layerDepth;

        [Browsable(false)]
        public ActorType type { get { return _type; } set { _type = value; } }
        [Browsable(false)]
        public int id { get { return _id; } set { _id = value; } }
        public float layerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
        [Browsable(false)]
        virtual public Vector2 circuitConnectionPosition { get { return Vector2.Zero; } }
        [Browsable(false)]
        virtual public Vector2 revoluteConnectionPosition { get { return Vector2.Zero; } }
        [Browsable(false)]
        virtual public Vector2 prismaticConnectionPosition { get { return Vector2.Zero; } }
        [Browsable(false)]
        virtual public Vector2 collisionFilterConnectionPosition { get { return Vector2.Zero; } }
        [Browsable(false)]
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
        [Browsable(false)]
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

        virtual protected void select() 
        {
            _level.controller.selectedActor = this;
        }

        virtual protected void deselect()
        {
            _level.controller.selectedActor = null;
        }

        virtual protected void delete()
        {
            if (selected)
                deselect();

            _level.removeActor(this);
        }

        abstract public void handleSelectedClick(System.Windows.Forms.MouseButtons button);

        abstract public bool handleUnselectedClick(System.Windows.Forms.MouseButtons button);

        abstract public bool hitTest(Vector2 testPoint, HitTestCallback callback);

        virtual public void update() { }

        abstract public void draw();

        virtual public EditorActor clone() { return null; }
    }
}
