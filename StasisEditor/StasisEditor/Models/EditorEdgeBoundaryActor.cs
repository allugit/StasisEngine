using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorEdgeBoundaryActor : EditorActor, IActorComponent
    {
        public enum EdgeBoundaryType
        {
            LowerBound,
            UpperBound
        };

        private Vector2 _position;
        private EdgeBoundaryType _edgeBoundaryType;

        public EdgeBoundaryType edgeBoundaryType { get { return _edgeBoundaryType; } set { _edgeBoundaryType = value; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;

                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("edge_boundary_type", _edgeBoundaryType);

                return d;
            }
        }

        public EditorEdgeBoundaryActor(EditorLevel level)
            : base(level, ActorType.EdgeBoundary, level.getUnusedActorId())
        {
            _position = _level.controller.worldMouse;
            _edgeBoundaryType = EdgeBoundaryType.LowerBound;
        }

        public EditorEdgeBoundaryActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _edgeBoundaryType = (EdgeBoundaryType)Loader.loadEnum(typeof(EdgeBoundaryType), data.Attribute("edge_boundary_type"), 0);
        }

        public override void draw()
        {
            if (_edgeBoundaryType == EdgeBoundaryType.LowerBound)
            {
                _level.controller.view.drawLine(_position, _position + new Vector2(1000f, 0f), Color.DarkRed * 0.5f, 0.05f);
                _level.controller.view.drawLine(_position, _position + new Vector2(0f, 1000f), Color.DarkRed * 0.5f, 0.05f);
                _level.controller.view.drawLine(_position, _position + new Vector2(1f, 0f), Color.Red, 0.04f);
                _level.controller.view.drawLine(_position, _position + new Vector2(0f, 1f), Color.Red, 0.04f);
            }
            else if (_edgeBoundaryType == EdgeBoundaryType.UpperBound)
            {
                _level.controller.view.drawLine(_position, _position + new Vector2(-1000f, 0f), Color.DarkRed * 0.5f, 0.05f);
                _level.controller.view.drawLine(_position, _position + new Vector2(0f, -1000f), Color.DarkRed * 0.5f, 0.05f);
                _level.controller.view.drawLine(_position, _position + new Vector2(-1f, 0f), Color.Red, 0.04f);
                _level.controller.view.drawLine(_position, _position + new Vector2(0f, -1f), Color.Red, 0.04f);
            }
            _level.controller.view.drawPoint(_position, Color.Yellow, 0.03f);
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                if (!_level.controller.isKeyHeld(Keys.LeftControl))
                {
                    _position += worldDelta;
                }

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
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
                    select();
                    return true;
                });
            }
            else if (button == System.Windows.Forms.MouseButtons.Right)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                {
                    _level.controller.openActorProperties(this);
                    return true;
                });
            }
            return false;
        }

        public override bool hitTest(Vector2 testPoint, HitTestCallback callback)
        {
            List<IActorComponent> results = new List<IActorComponent>();

            // Hit test point
            if (_level.controller.hitTestPoint(testPoint, _position))
            {
                results.Add(this);
                return callback(results);
            }

            return false;
        }
    }
}
