using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialNoiseLayer : MaterialNoiseLayer
    {
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
