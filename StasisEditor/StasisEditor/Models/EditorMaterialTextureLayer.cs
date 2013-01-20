using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialTextureLayer : MaterialTextureLayer
    {
        public EditorMaterialTextureLayer()
            : base()
        {
        }

        public EditorMaterialTextureLayer(XElement data)
            : base(data)
        {
        }
    }
}
