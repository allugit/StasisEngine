using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    using Keys = System.Windows.Forms.Keys;

    public class EditorBoxActor : EditorActor
    {
        private Vector2 _position;
        private float _halfWidth;
        private float _halfHeight;
        private float _angle;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public float halfWidth { get { return _halfWidth; } set { _halfWidth = value; } }
        public float halfHeight { get { return _halfHeight; } set { _halfHeight = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }

        public EditorBoxActor(EditorLevel level)
            : base(level, ActorType.Box, level.controller.getUnusedActorID())
        {
            _position = level.controller.worldMouse;
            initialize();
        }

        public EditorBoxActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            initialize();
        }

        private void initialize()
        {
            _type = ActorType.Box;
            _layerDepth = 0.1f;
            _halfWidth = 1f;
            _halfHeight = 1f;
        }

        public override bool hitTest()
        {
            return _level.controller.hitTestBox(_level.controller.worldMouse, _position, _halfWidth, _halfHeight, _angle);
        }

        public override void rotate(Vector2 anchorPoint, float increment)
        {
            Vector2 relativePosition = _position - anchorPoint;
            _position = anchorPoint + Vector2.Transform(relativePosition, Matrix.CreateRotationZ(increment));
            _angle += increment;
        }

        public override void update()
        {
            Vector2 deltaWorldMouse = _level.controller.worldMouse - _level.controller.oldWorldMouse;
            float angleIncrement = _level.controller.shift ? 0.00005f : 0.0005f;
            float sizeIncrement = _level.controller.shift ? 0.0001f : 0.001f;

            if (selected)
            {
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
            }
            else
            {
            }
        }

        public override void draw()
        {
            _level.controller.view.drawBox(_position, _halfWidth, _halfHeight, _angle, Color.LightBlue, _layerDepth);
        }
    }
}
