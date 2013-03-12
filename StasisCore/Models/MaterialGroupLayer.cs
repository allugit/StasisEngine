using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class MaterialGroupLayer : MaterialLayer
    {
        protected LayerBlendType _blendType;
        protected float _multiplier;
        protected Color _baseColor;
        protected List<MaterialLayer> _layers;

        public LayerBlendType blendType { get { return _blendType; } set { _blendType = value; } }
        public float multiplier { get { return _multiplier; } set { _multiplier = value; } }
        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }
        virtual public List<MaterialLayer> layers { get { return _layers; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("blend_type", _blendType.ToString().ToLower());
                d.SetAttributeValue("multiplier", _multiplier);
                d.SetAttributeValue("base_color", _baseColor);
                foreach (MaterialLayer layer in _layers)
                    d.Add(layer.data);
                return d;
            }
        }

        // Create new
        public MaterialGroupLayer(string type, bool enabled)
            : base(type, enabled)
        {
            _blendType = LayerBlendType.Opaque;
            _multiplier = 1f;
            _baseColor = Color.White;
            _layers = new List<MaterialLayer>();
        }

        // Create from xml
        public MaterialGroupLayer(XElement data)
            : base(data)
        {
            _blendType = (LayerBlendType)Loader.loadEnum(typeof(LayerBlendType), data.Attribute("blend_type"), (int)LayerBlendType.Opaque);
            _multiplier = Loader.loadFloat(data.Attribute("multiplier"), 1f);
            _baseColor = Loader.loadColor(data.Attribute("base_color"), Color.White);
            loadLayers(data);
        }

        // Load layers
        virtual protected void loadLayers(XElement data)
        {
            _layers = new List<MaterialLayer>();
            foreach (XElement layerXml in data.Elements("Layer"))
                _layers.Add(MaterialLayer.load(layerXml));
        }

        // Clone
        public override MaterialLayer clone()
        {
            MaterialGroupLayer layer = new MaterialGroupLayer(data);
            layer.type = "group";
            return layer;
        }
    }
}
