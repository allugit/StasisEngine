using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorRopeActor : EditorActor
    {
        private enum SelectedPoints
        {
            None,
            A,
            B,
            AB
        };

        private bool _doubleAnchor;
        private Vector2 _pointA;
        private Vector2 _pointB;
        private SelectedPoints _selectedPoints;

        public bool doubleAnchor;
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("double_anchor", _doubleAnchor);
                return d;
            }
        }

        public EditorRopeActor(EditorLevel level)
            : base(level, ActorType.Rope, level.controller.getUnusedActorID())
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            _pointA = worldMouse + new Vector2(0f, -0.5f);
            _pointB = worldMouse + new Vector2(0f, 0.5f);
            _selectedPoints = SelectedPoints.AB;
            _layerDepth = 0.1f;
        }

        public EditorRopeActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _pointA = Loader.loadVector2(data.Attribute("point_a"), Vector2.Zero);
            _pointB = Loader.loadVector2(data.Attribute("point_b"), Vector2.Zero);
            _selectedPoints = SelectedPoints.None;
        }

        public override void deselect()
        {
            _selectedPoints = SelectedPoints.None;
            base.deselect();
        }

        public override bool hitTest()
        {
            Vector2 worldMouse = _level.controller.worldMouse;

            // Hit test points
            if (_level.controller.hitTestPoint(worldMouse, _pointA))
            {
                _selectedPoints = SelectedPoints.A;
                return true;
            }
            else if (_level.controller.hitTestPoint(worldMouse, _pointB))
            {
                _selectedPoints = SelectedPoints.B;
                return true;
            }

            // Hit test line
            if (_level.controller.hitTestLine(worldMouse, _pointA, _pointB))
            {
                _selectedPoints = SelectedPoints.AB;
                return true;
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                if (_selectedPoints == SelectedPoints.A)
                    _pointA += worldDelta;
                else if (_selectedPoints == SelectedPoints.B)
                    _pointB += worldDelta;
                else if (_selectedPoints == SelectedPoints.AB)
                {
                    _pointA += worldDelta;
                    _pointB += worldDelta;
                }

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            _level.controller.view.drawLine(_pointA, _pointB, Color.Tan, _layerDepth);
            _level.controller.view.drawPoint(_pointA, Color.Yellow, _layerDepth);
            _level.controller.view.drawPoint(_pointB, Color.Orange, _layerDepth);
        }
    }
}
