using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    public class EditorTerrainActor : EditorPolygonActor
    {
        protected override Color polygonFill { get { return Color.Orange * 0.3f; } }

        public EditorTerrainActor(EditorLevel level)
            : base(level, ActorType.Terrain)
        {
        }

        public EditorTerrainActor(EditorLevel level, XElement data)
            : base(level, data)
        {
        }

        public override void draw()
        {
            if (_polygonTexture != null)
            {
                _level.controller.view.spriteBatch.Draw(_polygonTexture, (_polygonPosition + _level.controller.worldOffset) * _level.controller.scale , _polygonTexture.Bounds, polygonFill, 0f, Vector2.Zero, 1f, SpriteEffects.None, _layerDepth + 0.0001f);
            }

            // Draw lines and points
            int count = _headPoint.listCount;
            Color lineColor = count > 2 ? Color.Orange : Color.Red;
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
