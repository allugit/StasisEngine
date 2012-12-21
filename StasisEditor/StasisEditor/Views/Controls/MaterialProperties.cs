using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Resources;
using StasisCore.Models;
using StasisEditor.Controllers;

namespace StasisEditor.Views.Controls
{
    public partial class MaterialProperties : UserControl
    {
        private MaterialController _controller;
        private MaterialView _materialView;
        public PropertyGrid PropertyGrid { get { return this.materialPropertyGrid; } }

        public MaterialProperties(MaterialView materialView, Material material)
        {
            _materialView = materialView;
            _controller = materialView.getController();

            MaterialLayersControl terrainLayers = new MaterialLayersControl(_controller, material);
            terrainLayers.populateTreeView(material.rootLayer);
            Controls.Add(terrainLayers);

            // Controls
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        // Property value changed
        private void materialPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Refresh materials list
            _materialView.refreshMaterialList();

            // Set changes to true
            _controller.setChangesMade(true);
        }
    }
}
