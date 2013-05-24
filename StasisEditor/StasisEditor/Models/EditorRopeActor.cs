using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    using UITypeEditor = System.Drawing.Design.UITypeEditor;

    public class EditorRopeActor : EditorActor, IActorComponent
    {
        private bool _doubleAnchor;
        private string _ropeMaterialUID;
        private PointListNode _nodeA;
        private PointListNode _nodeB;
        private List<PointListNode> _selectedPoints;

        public string ropeMaterialUID { get { return _ropeMaterialUID; } set { _ropeMaterialUID = value; } }
        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return (_nodeA.position + _nodeB.position) / 2; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("double_anchor", _doubleAnchor);
                d.SetAttributeValue("rope_material_uid", _ropeMaterialUID);
                d.SetAttributeValue("point_a", _nodeA.position);
                d.SetAttributeValue("point_b", _nodeB.position);
                return d;
            }
        }

        public EditorRopeActor(EditorLevel level)
            : base(level, ActorType.Rope, level.controller.getUnusedActorID())
        {
            Vector2 worldMouse = _level.controller.worldMouse;

            _nodeA = new PointListNode(worldMouse + new Vector2(0f, -0.5f));
            _nodeB = new PointListNode(worldMouse + new Vector2(0f, 0.5f));
            _selectedPoints = new List<PointListNode>();
            _selectedPoints.Add(_nodeA);
            _selectedPoints.Add(_nodeB);
            _ropeMaterialUID = "default_rope_material";
            _layerDepth = 0.1f;
        }

        public EditorRopeActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _doubleAnchor = Loader.loadBool(data.Attribute("double_anchor"), false);
            _ropeMaterialUID = Loader.loadString(data.Attribute("rope_material_uid"), "default_rope_material");
            _nodeA = new PointListNode(Loader.loadVector2(data.Attribute("point_a"), Vector2.Zero));
            _nodeB = new PointListNode(Loader.loadVector2(data.Attribute("point_b"), Vector2.Zero));
            _selectedPoints = new List<PointListNode>();
        }

        protected override void deselect()
        {
            _selectedPoints.Clear();
            base.deselect();
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
                            _selectedPoints.Add(_nodeA);
                            _selectedPoints.Add(_nodeB);
                            select();
                            return true;
                        }
                        else if (results.Count > 0)
                        {
                            foreach (IActorComponent component in results)
                            {
                                if (component is PointListNode)
                                    _selectedPoints.Add(component as PointListNode);
                            }
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

            // Hit test points
            if (_level.controller.hitTestPoint(testPoint, _nodeA.position))
            {
                results.Add(_nodeA);
                return callback(results);
            }
            else if (_level.controller.hitTestPoint(testPoint, _nodeB.position))
            {
                results.Add(_nodeB);
                return callback(results);
            }

            // Hit test line
            if (_level.controller.hitTestLine(testPoint, _nodeA.position, _nodeB.position))
            {
                results.Add(this);
                return callback(results);
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                if (!_level.controller.isKeyHeld(Keys.LeftControl))
                {
                    foreach (PointListNode node in _selectedPoints)
                        node.position += worldDelta;
                }

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            _level.controller.view.drawLine(_nodeA.position, _nodeB.position, Color.Tan, _layerDepth);
            _level.controller.view.drawPoint(_nodeA.position, Color.Yellow, _layerDepth);
            _level.controller.view.drawPoint(_nodeB.position, Color.Orange, _layerDepth);
        }
    }
}
