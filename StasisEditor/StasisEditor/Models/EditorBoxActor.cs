using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
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

        public override void update()
        {
            Vector2 deltaWorldMouse = _level.controller.worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                _position += deltaWorldMouse;
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
