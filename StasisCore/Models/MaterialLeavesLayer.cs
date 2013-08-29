using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class MaterialLeavesLayer : MaterialScatterLayer
    {
        private Color _baseColor;

        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;

                d.SetAttributeValue("base_color", _baseColor);

                return d;
            }
        }

        public MaterialLeavesLayer()
            : base("leaves")
        {
            _baseColor = Color.White;
        }

        public MaterialLeavesLayer(XElement data)
            : base(data)
        {
            _baseColor = Loader.loadColor(data.Attribute("base_color"), Color.White);
        }

        public override MaterialLayer clone()
        {
            return new MaterialLeavesLayer(data);
        }
    }
}
