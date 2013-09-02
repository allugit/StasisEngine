using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{

    public class EditorRegionActor : EditorPolygonActor
    {
        private string _uid;

        public string uid { get { return _uid; } set { _uid = value; } }

        [Browsable(false)]
        protected override Color polygonFill { get { return Color.Green * 0.3f; } }

        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;

                d.SetAttributeValue("uid", _uid);

                return d;
            }
        }

        public EditorRegionActor(EditorLevel level)
            : base(level, ActorType.Region)
        {
            _uid = "";
        }

        public EditorRegionActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _uid = Loader.loadString(data.Attribute("uid"), "");
        }

        public override void draw()
        {
            if (_polygonTexture != null)
                _level.controller.view.spriteBatch.Draw(_polygonTexture, (_polygonPosition + _level.controller.worldOffset) * _level.controller.scale, _polygonTexture.Bounds, polygonFill, 0f, Vector2.Zero, _level.controller.scale / LevelController.ORIGINAL_SCALE, SpriteEffects.None, _layerDepth + 0.0001f);

            // Draw lines and points
            int count = _headPoint.listCount;
            Color lineColor = count > 2 ? Color.LightGreen : Color.Red;
            PointListNode current = _headPoint;
            while (current != null)
            {
                if (current.next != null)
                    _level.controller.view.drawLine(current.position, current.next.position, lineColor, _layerDepth);
                _level.controller.view.drawPoint(current.position, Color.Yellow, _layerDepth);
                current = current.next;
            }
            if (count > 2)
            {
                _level.controller.view.drawLine(_headPoint.position, _headPoint.tail.position, Color.Purple, _layerDepth);
            }
        }
    }
}
