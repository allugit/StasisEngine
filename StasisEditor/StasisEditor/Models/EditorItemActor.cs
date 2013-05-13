using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorItemActor : EditorActor, IActorComponent
    {
        private Vector2 _position;
        private string _itemUID;
        private int _quantity;

        public string itemUID { get { return _itemUID; } set { _itemUID = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return _position; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("item_uid", _itemUID);
                d.SetAttributeValue("quantity", _quantity);
                return d;
            }
        }

        public EditorItemActor(EditorLevel level, string itemUID)
            : base(level, ActorType.Item, level.controller.getUnusedActorID())
        {
            _itemUID = itemUID;
            _quantity = 1;
            _position = level.controller.worldMouse;
        }

        public EditorItemActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _itemUID = data.Attribute("item_uid").Value;
            _quantity = Loader.loadInt(data.Attribute("quantity"), 1);
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

            if (_level.controller.hitTestPoint(testPoint, _position, 12f))
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
                    _position += worldDelta;

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            _level.controller.view.drawIcon(_type, _position, _layerDepth);
        }
    }
}
