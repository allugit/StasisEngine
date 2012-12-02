using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Controllers;

namespace StasisEditor.Controls
{
    public partial class MaterialProperties : UserControl
    {
        private IController _controller;
        private Material _material;
        public PropertyGrid PropertyGrid { get { return this.materialPropertyGrid; } }

        public MaterialProperties(IController controller, Material material)
        {
            _material = material;
            _controller = controller;

            switch (material.type)
            {
                case MaterialType.Terrain:
                    Controls.Add(new TerrainLayers(_controller, (material as TerrainMaterial).layers));
                    break;
            }

            // Controls
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
    }
}
