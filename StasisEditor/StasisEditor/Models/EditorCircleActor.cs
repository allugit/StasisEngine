using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    public class EditorCircleActor : EditorActor, IActorComponent
    {
        private Vector2 _position;
        private float _radius;

        public float radius { get { return _radius; } set { _radius = value; } }
        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return _position; } }
        [Browsable(false)]
        public override Vector2 revoluteConnectionPosition { get { return _position; } }
        [Browsable(false)]
        public override Vector2 prismaticConnectionPosition { get { return _position; } }
        [Browsable(false)]
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

        public override void handleSelectedClick(System.Windows.Forms.MouseButtons button)
        {
            deselect();
        }

        public override bool handleUnselectedClick(System.Windows.Forms.MouseButtons button)
        {
            if (button == System.Windows.Forms.MouseButtons.Left)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                {
                    if (results.Count == 1 && results[0] == this)
                    {
                        select();
                        return true;
                    }
                    return false;
                });
            }
            else if (button == System.Windows.Forms.MouseButtons.Right)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                {
                    if (results.Count == 1)
                    {
                        _level.controller.openActorProperties(results[0]);
                        return true;
                    }
                    return false;
                });
            }
            return false;
        }

        public override bool hitTest(Vector2 testPoint, HitTestCallback callback)
        {
            List<IActorComponent> results = new List<IActorComponent>();

            if (_level.controller.hitTestCircle(testPoint, _position, _radius))
            {
                results.Add(this);
                return callback(results);
            }

            return false;
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
