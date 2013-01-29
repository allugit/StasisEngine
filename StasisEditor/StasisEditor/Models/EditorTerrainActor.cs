using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    using Keys = System.Windows.Forms.Keys;

    public class EditorTerrainActor : EditorActor
    {
        private List<Vector2> _points;
        private List<int> _selectedIndices;

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                foreach (Vector2 point in _points)
                    d.Add(new XElement("Point", point));
                return d;
            }
        }

        public EditorTerrainActor(EditorLevel level)
            : base(level, ActorType.Terrain, level.controller.getUnusedActorID())
        {
            _selectedIndices = new List<int>();
            _points = new List<Vector2>();
            _layerDepth = 0.1f;

            Vector2 worldMouse = level.controller.worldMouse;
            _points.Add(worldMouse + new Vector2(0f, -0.5f));
            _points.Add(worldMouse + new Vector2(-0.5f, 0.5f));
            _points.Add(worldMouse + new Vector2(0.5f, 0.5f));
            _selectedIndices.Add(0);
            _selectedIndices.Add(1);
            _selectedIndices.Add(2);
        }

        public EditorTerrainActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _points = new List<Vector2>();
            foreach (XElement pointData in data.Elements("Point"))
                _points.Add(Loader.loadVector2(pointData, Vector2.Zero));
        }

        public override void deselect()
        {
            _selectedIndices.Clear();

            base.deselect();
        }

        public override bool hitTest()
        {
            Vector2 worldMouse = _level.controller.worldMouse;

            // Test points
            for (int i = 0; i < _points.Count; i++)
            {
                if (_level.controller.hitTestPoint(worldMouse, _points[i]))
                {
                    _selectedIndices.Add(i);
                    return true;
                }
            }

            // Test line segments
            for (int i = 0; i < _points.Count; i++)
            {
                int a = i;
                int b = i == _points.Count - 1 ? 0 : i + 1;
                Vector2 pointA = _points[a];
                Vector2 pointB = _points[b];
                if (_level.controller.hitTestLine(worldMouse, pointA, pointB))
                {
                    _selectedIndices.Add(a);
                    _selectedIndices.Add(b);
                    return true;
                }
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            Vector2 worldDelta = worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                foreach (int i in _selectedIndices)
                    _points[i] += worldDelta;

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            Color lineColor = _points.Count > 2 ? Color.Orange : Color.Red;

            for (int i = 0; i < _points.Count - 1; i++)
            {
                Vector2 pointA = _points[i];
                Vector2 pointB = _points[i + 1];
                _level.controller.view.drawLine(pointA, pointB, lineColor, _layerDepth);
            }
            _level.controller.view.drawLine(_points[_points.Count - 1], _points[0], Color.Purple, _layerDepth);

            foreach (Vector2 point in _points)
                _level.controller.view.drawPoint(point, Color.Yellow, _layerDepth);
        }
    }
}
