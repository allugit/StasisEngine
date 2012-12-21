﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class MaterialRootLayer : MaterialLayer
    {
        private List<MaterialLayer> _layers;

        public List<MaterialLayer> layers { get { return _layers; } set { _layers = value; } }

        public MaterialRootLayer(XElement data) : base(data)
        {
            _layers = new List<MaterialLayer>();
            foreach (XElement layerXml in data.Elements("Layer"))
                _layers.Add(MaterialLayer.create(layerXml));
        }
    }
}
