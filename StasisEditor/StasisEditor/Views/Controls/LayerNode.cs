using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public class LayerNode : TreeNode
    {
        private MaterialLayer _layer;
        public MaterialLayer layer { get { return _layer; } }

        public LayerNode(MaterialLayer layer, bool enabled)
            : base()
        {
            _layer = layer;
            Text = _layer.ToString();
            Checked = enabled;
        }
    }
}
