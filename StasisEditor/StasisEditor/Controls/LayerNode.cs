using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Controls
{
    public class LayerNode : TreeNode
    {
        private TerrainLayerResource _layer;
        public TerrainLayerResource layer { get { return _layer; } }

        public LayerNode(TerrainLayerResource layer)
            : base()
        {
            _layer = layer;
            Text = _layer.ToString();
            Checked = true;
        }
    }
}
