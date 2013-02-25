using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using StasisCore;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    public class EditorTerrainActor : EditorPolygonActor
    {
        private BodyType _bodyType;
        private bool _destructible;
        private float _chunkSpacingX;
        private float _chunkSpacingY;
        private int _destructibleSeed;
        private float _density;
        private float _friction;
        private float _restitution;
        private string _materialUID;

        public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }
        public bool destructible { get { return _destructible; } set { _destructible = value; } }
        public float chunkSpacingX { get { return _chunkSpacingX; } set { _chunkSpacingX = value; } }
        public float chunkSpacingY { get { return _chunkSpacingY; } set { _chunkSpacingY = value; } }
        public int destructibleSeed { get { return _destructibleSeed; } set { _destructibleSeed = value; } }
        public float density { get { return _density; } set { _density = value; } }
        public float friction { get { return _friction; } set { _friction = value; } }
        public float restitution { get { return _restitution; } set { _restitution = value; } }
        public string materialUID { get { return _materialUID; } set { _materialUID = value; } }
        [Browsable(false)]
        protected override Color polygonFill { get { return Color.Orange * 0.3f; } }
        [Browsable(false)]
        public override Vector2 prismaticConnectionPosition { get { return _headPoint.position; } }
        [Browsable(false)]
        public override Vector2 revoluteConnectionPosition { get { return _headPoint.position; } }
        [Browsable(false)]
        public override Vector2 collisionFilterConnectionPosition { get { return _headPoint.position; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("body_type", _bodyType);
                d.SetAttributeValue("destructible", _destructible);
                d.SetAttributeValue("chunk_spacing_x", _chunkSpacingX);
                d.SetAttributeValue("chunk_spacing_y", _chunkSpacingY);
                d.SetAttributeValue("destructible_seed", _destructibleSeed);
                d.SetAttributeValue("density", _density);
                d.SetAttributeValue("friction", _friction);
                d.SetAttributeValue("restitution", _restitution);
                d.SetAttributeValue("material_uid", _materialUID);
                return d;
            }
        }

        public EditorTerrainActor(EditorLevel level)
            : base(level, ActorType.Terrain)
        {
            _bodyType = BodyType.Static;
            _destructible = false;
            _chunkSpacingX = 1f;
            _chunkSpacingY = 1f;
            _destructibleSeed = 12345;
            _density = 0.5f;
            _friction = 1f;
            _restitution = 0f;
            _materialUID = "default";
        }

        public EditorTerrainActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _bodyType = (BodyType)Loader.loadEnum(typeof(BodyType), data.Attribute("body_type"), (int)BodyType.Static);
            _destructible = Loader.loadBool(data.Attribute("destructible"), false);
            _chunkSpacingX = Loader.loadFloat(data.Attribute("chunk_spacing_x"), 1f);
            _chunkSpacingY = Loader.loadFloat(data.Attribute("chunk_spacing_y"), 1f);
            _destructibleSeed = Loader.loadInt(data.Attribute("destructible_seed"), 12345);
            _density = Loader.loadFloat(data.Attribute("density"), 0.5f);
            _friction = Loader.loadFloat(data.Attribute("friction"), 1f);
            _restitution = Loader.loadFloat(data.Attribute("restitution"), 0f);
            _materialUID = Loader.loadString(data.Attribute("material_uid"), "default");
        }

        public override void draw()
        {
            if (_polygonTexture != null)
                _level.controller.view.spriteBatch.Draw(_polygonTexture, (_polygonPosition + _level.controller.worldOffset) * _level.controller.scale, _polygonTexture.Bounds, polygonFill, 0f, Vector2.Zero, _level.controller.scale / EditorController.ORIGINAL_SCALE, SpriteEffects.None, _layerDepth + 0.0001f);

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
