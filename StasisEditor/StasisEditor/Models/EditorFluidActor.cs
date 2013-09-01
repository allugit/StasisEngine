using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    public class EditorFluidActor : EditorPolygonActor
    {
        [Browsable(false)]
        protected override Color polygonFill { get { return Color.Blue * 0.3f; } }

        public EditorFluidActor(EditorLevel level)
            : base(level, ActorType.Fluid)
        {
        }

        public EditorFluidActor(EditorLevel level, XElement data)
            : base(level, data)
        {
        }

        public override void draw()
        {
            if (_polygonTexture != null)
                _level.controller.view.spriteBatch.Draw(_polygonTexture, (_polygonPosition + _level.controller.worldOffset) * _level.controller.scale, _polygonTexture.Bounds, polygonFill, 0f, Vector2.Zero, _level.controller.scale / LevelController.ORIGINAL_SCALE, SpriteEffects.None, _layerDepth + 0.0001f);

            // Draw points and lines
            int count = _headPoint.listCount;
            Color lineColor = count > 2 ? Color.Blue : Color.Red;
            PointListNode current = _headPoint;
            while (current != null)
            {
                if (current.next != null)
                    _level.controller.view.drawLine(current.position, current.next.position, lineColor, _layerDepth);
                _level.controller.view.drawPoint(current.position, Color.LightBlue, _layerDepth);
                current = current.next;
            }
            if (count > 2)
            {
                _level.controller.view.drawLine(_headPoint.position, _headPoint.tail.position, Color.Purple, _layerDepth);
            }
        }
    }
}
