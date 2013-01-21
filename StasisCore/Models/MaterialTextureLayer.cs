using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class MaterialTextureLayer : MaterialLayer
    {
        private string _textureUID;
        private float _scale;
        private float _multiplier;
        private LayerBlendType _blendType;

        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        public float scale { get { return _scale; } set { _scale = value; } }
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }
        public LayerBlendType blendType { get { return _blendType; } set { _blendType = value; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("texture_uid", _textureUID);
                d.SetAttributeValue("scale", _scale);
                d.SetAttributeValue("multiplier", _multiplier);
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
        }

        // Create from xml
        public MaterialTextureLayer(XElement data) : base(data)
        {
            _textureUID = data.Attribute("texture_uid").Value;
            _scale = float.Parse(data.Attribute("scale").Value);
            _multiplier = float.Parse(data.Attribute("multiplier").Value);
            _blendType = (LayerBlendType)Enum.Parse(typeof(LayerBlendType), data.Attribute("blend_type").Value, true);
        }

        // Clone
        public override MaterialLayer clone()
        {
            return new MaterialTextureLayer(data);
        }
    }
}
