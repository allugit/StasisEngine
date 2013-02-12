using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;
using StasisEditor.Controllers;
using Box2D.XNA;

namespace StasisEditor.Models
{
    public class EditorBoxActor : EditorActor, IActorComponent
    {
        private Vector2 _position;
        private BodyType _bodyType;
        private float _halfWidth;
        private float _halfHeight;
        private float _angle;
        private float _density;
        private float _friction;
        private float _restitution;
        private string _materialUID;

        public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }
        public float halfWidth { get { return _halfWidth; } set { _halfWidth = value; } }
        public float halfHeight { get { return _halfHeight; } set { _halfHeight = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }
        public float density { get { return _density; } set { _density = value; } }
        public float friction { get { return _friction; } set { _friction = value; } }
        public float restitution { get { return _restitution; } set { _restitution = value; } }
        public string materialUID { get { return _materialUID; } set { _materialUID = value; } }
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
                d.SetAttributeValue("body_type", _bodyType);
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("half_width", _halfWidth);
                d.SetAttributeValue("half_height", _halfHeight);
                d.SetAttributeValue("angle", _angle);
                d.SetAttributeValue("density", _density);
                d.SetAttributeValue("friction", _friction);
                d.SetAttributeValue("restitution", _restitution);
                d.SetAttributeValue("material_uid", _materialUID);
                return d;
            }
        }

        public EditorBoxActor(EditorLevel level)
            : base(level, ActorType.Box, level.controller.getUnusedActorID())
        {
            _bodyType = BodyType.Static;
            _position = level.controller.worldMouse;
            _halfWidth = 1f;
            _halfHeight = 1f;
            _angle = 0f;
            _density = 0.5f;
            _friction = 1f;
            _restitution = 0f;
            _layerDepth = 0.1f;
            _materialUID = "default";
        }

        public EditorBoxActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _bodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _halfWidth = Loader.loadFloat(data.Attribute("half_width"), 1f);
            _halfHeight = Loader.loadFloat(data.Attribute("half_height"), 1f);
            _angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            _density = Loader.loadFloat(data.Attribute("density"), 0.5f);
            _friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            _restitution = Loader.loadFloat(data.Attribute("restitution"), 0f);
            _materialUID = Loader.loadString(data.Attribute("material_uid"), "default");
        }

        public void rotate(Vector2 anchorPoint, float increment)
        {
            Vector2 relativePosition = _position - anchorPoint;
            _position = anchorPoint + Vector2.Transform(relativePosition, Matrix.CreateRotationZ(increment));
            _angle += increment;
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
                        _level.controller.selectedActor = this;
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
            if (_level.controller.hitTestBox(testPoint, _position, _halfWidth, _halfHeight, _angle))
                results.Add(this);

            return callback(results);
        }

        public override void update()
        {
            Vector2 deltaWorldMouse = _level.controller.worldMouse - _level.controller.oldWorldMouse;
            float angleIncrement = _level.controller.shift ? 0.00005f : 0.0005f;
            float sizeIncrement = _level.controller.shift ? 0.0001f : 0.001f;

            if (selected)
            {
                if (!_level.controller.ctrl)
                    _position += deltaWorldMouse;

                if (_level.controller.isKeyHeld(Keys.Q))
                    rotate(_level.controller.worldMouse, -angleIncrement);
                if (_level.controller.isKeyHeld(Keys.E))
                    rotate(_level.controller.worldMouse, angleIncrement);

                if (_level.controller.isKeyHeld(Keys.A))
                    _halfWidth = Math.Max(1f, _halfWidth + sizeIncrement);
                if (_level.controller.isKeyHeld(Keys.D))
                    _halfWidth = Math.Max(1f, _halfWidth - sizeIncrement);

                if (_level.controller.isKeyHeld(Keys.W))
                    _halfHeight = Math.Max(1f, _halfHeight + sizeIncrement);
                if (_level.controller.isKeyHeld(Keys.S))
                    _halfHeight = Math.Max(1f, _halfHeight - sizeIncrement);

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            _level.controller.view.drawBox(_position, _halfWidth, _halfHeight, _angle, Color.LightBlue, _layerDepth);
        }
    }
}
