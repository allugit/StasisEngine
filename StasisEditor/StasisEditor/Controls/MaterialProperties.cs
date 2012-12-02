using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Controls
{
    public partial class MaterialProperties : UserControl
    {
        private Material _material;
        public PropertyGrid PropertyGrid { get { return this.materialPropertyGrid; } }

        public MaterialProperties(Material material)
        {
            _material = material;

            switch (material.type)
            {
                case MaterialType.Terrain:
                    Controls.Add(new TerrainLayers((material as TerrainMaterial).layers));
                    break;
            }

            // Controls
            InitializeComponent();
            Dock = DockStyle.Top;
        }
    }
}
