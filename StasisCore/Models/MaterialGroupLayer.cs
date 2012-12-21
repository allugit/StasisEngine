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

        public LayerBlendType blendType { get { return _blendType; } }
        public float multiplier { get { return _multiplier; } }
        public List<MaterialLayer> layers { get { return _layers; } }

        public MaterialGroupLayer(XElement data)
            : base(data)
        {
            _blendType = (LayerBlendType)Enum.Parse(typeof(LayerBlendType), data.Attribute("blend_type").Value, true);
            _layers = new List<MaterialLayer>();
            foreach (XElement layerXml in data.Elements("Layer"))
                _layers.Add(MaterialLayer.create(layerXml));
        }
    }
}
