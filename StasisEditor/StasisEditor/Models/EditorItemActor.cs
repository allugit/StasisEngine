using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorItemActor : EditorActor
    {
        private Vector2 _position;
        private string _itemUID;
        private int _quantity;

        public string itemUID { get { return _itemUID; } set { _itemUID = value; } }
        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public override Vector2 circuitConnectionPosition { get { return _position; } }
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

        public override bool hitTest(Vector2 testPoint)
        {
            return _level.controller.hitTestPoint(testPoint, _position, 12f);
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
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
