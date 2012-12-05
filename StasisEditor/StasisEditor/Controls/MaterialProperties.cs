﻿using System;
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
        private IMaterialController _controller;
        private MaterialResource _material;
        public PropertyGrid PropertyGrid { get { return this.materialPropertyGrid; } }

        public MaterialProperties(IMaterialController controller, MaterialResource material)
        {
            _material = material;
            _controller = controller;

            switch (material.type)
            {
                case MaterialType.Terrain:
                    TerrainLayers terrainLayers = new TerrainLayers(_controller);
                    terrainLayers.populateTreeView((material as TerrainMaterialResource).layers);
                    Controls.Add(terrainLayers);
                    break;
            }

            // Controls
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
    }
}
