using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    public class EditorCircleActor : EditorActor
    {
        private Vector2 _position;
        private float _radius;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public override Vector2 circuitWorldAnchor { get { return _position; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("radius", _radius);
                return d;
            }
        }

        public EditorCircleActor(EditorLevel level)
            : base(level, ActorType.Circle, level.controller.getUnusedActorID())
        {
            _position = level.controller.worldMouse;
            _radius = 1f;
            _layerDepth = 0.1f;
        }

        public EditorCircleActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _radius = Loader.loadFloat(data.Attribute("radius"), 1f);
        }

        public override bool hitTest(Vector2 testPoint)
        {
            return _level.controller.hitTestCircle(testPoint, _position, _radius);
        }

        public override void update()
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            Vector2 worldDelta = worldMouse - _level.controller.oldWorldMouse;
            float radiusIncrement = _level.controller.shift ? 0.00005f : 0.0005f;

            if (selected)
            {
                if (!_level.controller.ctrl)
                    _position += worldDelta;

                if (_level.controller.isKeyHeld(Keys.A) || _level.controller.isKeyHeld(Keys.W))
                    _radius = Math.Max(1f, _radius + radiusIncrement);
                if (_level.controller.isKeyHeld(Keys.D) || _level.controller.isKeyHeld(Keys.S))
                    _radius = Math.Max(1f, _radius - radiusIncrement);

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            _level.controller.view.drawCircle(_position, _radius, Color.LightBlue, _layerDepth);
        }
    }
}
