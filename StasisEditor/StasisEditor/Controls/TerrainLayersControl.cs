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
    public partial class TerrainLayersControl : UserControl
    {
        private IMaterialController _controller;
        private TerrainMaterialResource _material;
        //private List<TerrainLayerResource> _layers;

        public TerrainLayersControl(IMaterialController controller, TerrainMaterialResource material)
        {
            _controller = controller;
            _material = material;

            // Controls
            InitializeComponent();
            Dock = DockStyle.Fill;
            upButton.Text = char.ConvertFromUtf32(0x000002c4);
            downButton.Text = char.ConvertFromUtf32(0x000002c5);
        }

        // populate tree view
        public void populateTreeView(List<TerrainLayerResource> layers)
        {
            // Clear existing nodes
            layersTreeView.Nodes.Clear();

            // Build root nodes
            List<LayerNode> rootNodes = new List<LayerNode>();
            foreach (TerrainLayerResource layer in layers)
                rootNodes.Add(new LayerNode(layer, layer.enabled));

            // Recursively populate root nodes
            foreach (LayerNode node in rootNodes)
                recursiveNodePopulate(node);

            // Set root nodes
            foreach (LayerNode node in rootNodes)
                layersTreeView.Nodes.Add(node);

            //Console.WriteLine("nodes: {0}", rootNodes);
        }

        // recursiveNodePopulate
        private void recursiveNodePopulate(LayerNode node)
        {
            foreach (TerrainLayerResource layer in node.layer.layers)
            {
                LayerNode childNode = new LayerNode(layer, layer.enabled);
                node.Nodes.Add(childNode);
                recursiveNodePopulate(childNode);
            }
        }

        // Move layer up
        private void upButton_Click(object sender, EventArgs e)
        {
        }

        // Move layer down
        private void downButton_Click(object sender, EventArgs e)
        {
        }

        // Add new sibling layer
        private void addSiblingButton_Click(object sender, EventArgs e)
        {

        }

        // Add child node clicked
        private void addChildButton_Click(object sender, EventArgs e)
        {
            LayerNode node = (layersTreeView.SelectedNode as LayerNode);
            if (node == null)
                return;

            // Show new terrain layer select box
            NewTerrainLayerForm newLayerForm = new NewTerrainLayerForm();
            if (newLayerForm.ShowDialog() == DialogResult.OK)
            {
                // Add new layer
                _controller.addTerrainLayerToMaterial(_material, newLayerForm.getSelectedType(), node.layer, null);

                // Refresh the tree view
                populateTreeView(_material.layers);
            }
        }

        // Remove layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
        }

        // Property value changed
        private void layerProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _controller.setChangesMade(true);
        }

        // Selected node changed
        private void layersTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            layerProperties.SelectedObject = (e.Node as LayerNode).layer.properties;
        }

        // Node check changed
        private void layersTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            LayerNode node = (e.Node as LayerNode);
            node.layer.enabled = node.Checked;
        }
    }
}
