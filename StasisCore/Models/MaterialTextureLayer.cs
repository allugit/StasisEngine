using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class MaterialTextureLayer : MaterialLayer
    {
        private string _textureUID;
        private float _scale;
        private float _multiplier;
        private Color _baseColor;
        private LayerBlendType _blendType;

        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        public float scale { get { return _scale; } set { _scale = value; } }
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }
        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }
        public LayerBlendType blendType { get { return _blendType; } set { _blendType = value; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("texture_uid", _textureUID);
                d.SetAttributeValue("scale", _scale);
                d.SetAttributeValue("multiplier", _multiplier);
                d.SetAttributeValue("base_color", _baseColor);
                d.SetAttributeValue("blend_type", _blendType.ToString().ToLower());
                return d;
            }
        }

        // Create new
        public MaterialTextureLayer()
            : base("texture", true)
        {
            _textureUID = "default";
            _scale = 1f;
            _multiplier = 1f;
            _baseColor = Color.White;
            _blendType = LayerBlendType.Opaque;
        }

        // Create from xml
        public MaterialTextureLayer(XElement data) : base(data)
        {
            _textureUID = data.Attribute("texture_uid").Value;
            _scale = Loader.loadFloat(data.Attribute("scale"), 1f);
            _multiplier = Loader.loadFloat(data.Attribute("multiplier"), 1f);
            _baseColor = Loader.loadColor(data.Attribute("base_color"), Color.White);
            _blendType = (LayerBlendType)Loader.loadEnum(typeof(LayerBlendType), data.Attribute("blend_type"), (int)LayerBlendType.Opaque);
        }

        // Clone
        public override MaterialLayer clone()
        {
            return new MaterialTextureLayer(data);
        }
    }
}
