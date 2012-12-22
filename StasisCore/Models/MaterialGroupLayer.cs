using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class MaterialGroupLayer : MaterialLayer
    {
        private LayerBlendType _blendType;
        private float _multiplier;
        private List<MaterialLayer> _layers;

        public LayerBlendType blendType { get { return _blendType; } set { _blendType = value; } }
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }
        public List<MaterialLayer> layers { get { return _layers; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("blend_type", _blendType.ToString().ToLower());
                d.SetAttributeValue("multiplier", _multiplier);
                foreach (MaterialLayer layer in _layers)
                    d.Add(layer.data);
                return d;
            }
        }

        // Create new
        public MaterialGroupLayer(string type = "group", bool enabled = true)
            : base(type, enabled)
        {
            _blendType = LayerBlendType.Opaque;
            _multiplier = 1f;
            _layers = new List<MaterialLayer>();
        }

        // Create from xml
        public MaterialGroupLayer(XElement data)
            : base(data)
        {
            _blendType = (LayerBlendType)Enum.Parse(typeof(LayerBlendType), data.Attribute("blend_type").Value, true);
            _multiplier = float.Parse(data.Attribute("multiplier").Value);
            _layers = new List<MaterialLayer>();
            foreach (XElement layerXml in data.Elements("Layer"))
                _layers.Add(MaterialLayer.load(layerXml));
        }
    }
}
