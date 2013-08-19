using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorDecalActor : EditorActor, IActorComponent
    {
        private Vector2 _position;
        private string _decalType;
        private string _textureUid;
        private Vector2 _origin;
        private float _angle;
        private Texture2D _texture;

        public string decalType { get { return _decalType; } set { _decalType = value; } }
        public string textureUid
        { 
            get { return _textureUid; } 
            set
            {
                _textureUid = value;
                try
                {
                    _texture = ResourceManager.getTexture(_textureUid);
                }
                catch
                {
                    Console.WriteLine("decal texture not found: {0}", _textureUid);
                }
            } 
        }
        public Vector2 origin { get { return _origin; } set { _origin = value; } }
        public float angle { get { return _angle; } set { _angle = value; } }
        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return _position; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("decal_type", _decalType);
                d.SetAttributeValue("texture_uid", _textureUid);
                d.SetAttributeValue("origin", _origin);
                d.SetAttributeValue("angle", _angle);
                return d;
            }
        }

        public EditorDecalActor(EditorLevel level)
            : base(level, ActorType.Decal, level.controller.getUnusedActorID())
        {
            _position = level.controller.worldMouse;
            _decalType = "default_decal";
            textureUid = "default";
            _origin = Vector2.Zero;
            _angle = 0f;
        }

        public EditorDecalActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _decalType = Loader.loadString(data.Attribute("decal_type"), "default_decal");
            textureUid = Loader.loadString(data.Attribute("texture_uid"), "default");
            _origin = Loader.loadVector2(data.Attribute("origin"), Vector2.Zero);
            _angle = Loader.loadFloat(data.Attribute("angle"), 0f);
        }

        public void rotate(Vector2 anchorPoint, float increment)
        {
            Vector2 relativePosition = _position - anchorPoint;
            _position = anchorPoint + Vector2.Transform(relativePosition, Matrix.CreateRotationZ(increment));
            _angle += increment;
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
            float angleIncrement = _level.controller.isKeyHeld(Keys.LeftShift) ? 0.00005f : 0.0005f;

            if (selected)
            {
                if (!_level.controller.isKeyHeld(Keys.LeftControl))
                    _position += worldDelta;

                if (_level.controller.isKeyHeld(Keys.Q))
                    rotate(_level.controller.worldMouse, -angleIncrement);
                if (_level.controller.isKeyHeld(Keys.E))
                    rotate(_level.controller.worldMouse, angleIncrement);

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            if (_textureUid != "default" && _texture != null)
            {
                _level.controller.view.drawTexture(_texture, _position, _angle, _origin * new Vector2(_texture.Width, _texture.Height), _layerDepth + 0.0001f);
            }
            _level.controller.view.drawIcon(_type, _position, _layerDepth);
        }
    }
}
