using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialTextureLayer : MaterialTextureLayer
    {
        [Browsable(false)]
        public override string type { get { return base.type; } set { base.type = value; } }

        [Browsable(false)]
        public override bool enabled { get { return base.enabled; } set { base.enabled = value; } }

        [Browsable(false)]
        public override XElement data { get { return base.data; } }

        public EditorMaterialTextureLayer()
            : base()
        {
        }

        public EditorMaterialTextureLayer(XElement data)
            : base(data)
        {
        }

        public override MaterialLayer clone()
        {
            return new EditorMaterialTextureLayer(data);
        }
    }
}
