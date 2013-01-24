using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialNoiseLayer : MaterialNoiseLayer
    {
        [Browsable(false)]
        public override string type { get { return base.type; } set { base.type = value; } }

        [Browsable(false)]
        public override bool enabled { get { return base.enabled; } set { base.enabled = value; } }

        [Browsable(false)]
        public override XElement data { get { return base.data; } }

        public EditorMaterialNoiseLayer()
            : base()
        {
        }

        public EditorMaterialNoiseLayer(XElement data)
            : base(data)
        {
        }

        public override MaterialLayer clone()
        {
            return new EditorMaterialNoiseLayer(data);
        }
    }
}
