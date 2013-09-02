using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorLevelTransitionActor : EditorActor, IActorComponent
    {
        private Vector2 _position;
        private float _halfWidth;
        private float _halfHeight;
        private string _levelUid;
        private Vector2 _positionInLevel;
        private float _angle;
        private bool _requiresActivation;

        [Browsable(false)]
        public Vector2 position { get { return _position; } set { _position = value; } }
        public string levelUid { get { return _levelUid; } set { _levelUid = value; } }
        public Vector2 positionInLevel { get { return _positionInLevel; } set { _positionInLevel = value; } }
        public float halfWidth { get { return _halfWidth; } set { _halfWidth = Math.Max(value, 0.05f); } }
        public float halfHeight { get { return _halfHeight; } set { _halfHeight = Math.Max(value, 0.05f); } }
        public float angle { get { return _angle; } set { _angle = value; } }
        public bool requiresActivation { get { return _requiresActivation; } set { _requiresActivation = value; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;

                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("half_width", _halfWidth);
                d.SetAttributeValue("half_height", _halfHeight);
                d.SetAttributeValue("angle", _angle);
                d.SetAttributeValue("level_uid", _levelUid);
                d.SetAttributeValue("position_in_level", _positionInLevel);
                d.SetAttributeValue("requires_activation", _requiresActivation);
                return d;
            }
        }

        public EditorLevelTransitionActor(EditorLevel level)
            : base(level, ActorType.LevelTransition, level.getUnusedActorId())
        {
            _position = level.controller.worldMouse;
            _halfWidth = 0.5f;
            _halfHeight = 0.5f;
            _angle = 0f;
            _levelUid = "default";
            _positionInLevel = Vector2.Zero;
            _requiresActivation = true;
        }

        public EditorLevelTransitionActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _halfWidth = Loader.loadFloat(data.Attribute("half_width"), 0.5f);
            _halfHeight = Loader.loadFloat(data.Attribute("half_height"), 0.5f);
            _angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            _levelUid = Loader.loadString(data.Attribute("level_uid"), "default");
            _positionInLevel = Loader.loadVector2(data.Attribute("position_in_level"), Vector2.Zero);
            _requiresActivation = Loader.loadBool(data.Attribute("requires_activation"), true);
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
            if (_level.controller.hitTestBox(testPoint, _position, _halfWidth, _halfHeight, _angle))
                results.Add(this);

            return callback(results);
        }

        public void rotate(Vector2 anchorPoint, float increment)
        {
            Vector2 relativePosition = _position - anchorPoint;
            _position = anchorPoint + Vector2.Transform(relativePosition, Matrix.CreateRotationZ(increment));
            _angle += increment;
        }

        public override void update()
        {
            Vector2 deltaWorldMouse = _level.controller.worldMouse - _level.controller.oldWorldMouse;
            float angleIncrement = _level.controller.isKeyHeld(Keys.LeftShift) ? 0.00005f : 0.0005f;
            float sizeIncrement = _level.controller.isKeyHeld(Keys.LeftShift) ? 0.0001f : 0.001f;

            if (selected)
            {
                if (!_level.controller.isKeyHeld(Keys.LeftControl))
                    _position += deltaWorldMouse;

                if (_level.controller.isKeyHeld(Keys.Q))
                    rotate(_level.controller.worldMouse, -angleIncrement);
                if (_level.controller.isKeyHeld(Keys.E))
                    rotate(_level.controller.worldMouse, angleIncrement);

                if (_level.controller.isKeyHeld(Keys.A))
                    halfWidth = _halfWidth + sizeIncrement;
                if (_level.controller.isKeyHeld(Keys.D))
                    halfWidth = _halfWidth - sizeIncrement;

                if (_level.controller.isKeyHeld(Keys.W))
                    halfHeight = _halfHeight + sizeIncrement;
                if (_level.controller.isKeyHeld(Keys.S))
                    halfHeight = _halfHeight - sizeIncrement;

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            _level.controller.view.drawBox(_position, _halfWidth, _halfHeight, _angle, Color.LightGreen * 0.5f, _layerDepth);
        }
    }
}
