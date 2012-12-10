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

namespace StasisEditor.Views.Controls
{
    public partial class MaterialProperties : UserControl
    {
        private MaterialController _controller;
        private MaterialResource _material;
        public PropertyGrid PropertyGrid { get { return this.materialPropertyGrid; } }

        public MaterialProperties(MaterialController controller, MaterialResource material)
        {
            _material = material;
            _controller = controller;

            switch (material.type)
            {
                case MaterialType.Terrain:
                    TerrainLayersControl terrainLayers = new TerrainLayersControl(_controller, _material as TerrainMaterialResource);
                    terrainLayers.populateTreeView((material as TerrainMaterialResource).rootLayer);
                    Controls.Add(terrainLayers);
                    break;
            }

            // Controls
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
    }
}
