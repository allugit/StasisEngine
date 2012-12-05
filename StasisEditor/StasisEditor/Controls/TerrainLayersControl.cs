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
        private TreeNode rootNode;
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

            // Add root node to tree view
            rootNode = new TreeNode("Root");
            rootNode.Checked = true;
            layersTreeView.Nodes.Add(rootNode);
        }

        // populate tree view
        public void populateTreeView(List<TerrainLayerResource> layers)
        {
            // Clear existing nodes
            rootNode.Nodes.Clear();

            // Build root nodes
            List<LayerNode> rootNodes = new List<LayerNode>();
            foreach (TerrainLayerResource layer in layers)
                rootNodes.Add(new LayerNode(layer, layer.enabled));

            // Recursively populate root nodes
            foreach (LayerNode node in rootNodes)
                recursiveNodePopulate(node);

            // Set root nodes
            foreach (LayerNode node in rootNodes)
                rootNode.Nodes.Add(node);

            // Expand all
            rootNode.ExpandAll();
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
            TreeNode selectedNode = layersTreeView.SelectedNode;
            if (selectedNode == null)
                return;

            // Show new terrain layer select box
            NewTerrainLayerForm newLayerForm = new NewTerrainLayerForm();
            if (newLayerForm.ShowDialog() == DialogResult.OK)
            {
                // Determine parent layer
                TerrainLayerResource parent = selectedNode == rootNode ? null : (selectedNode as LayerNode).layer;

                // Add new layer to material
                _controller.addTerrainLayerToMaterial(_material, newLayerForm.getSelectedType(), parent, null);

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
            // Enable/disable remove button
            removeLayerButton.Enabled = e.Node != rootNode;

            // Check for root node selection
            if (e.Node == rootNode)
                return;

            // Enable/disable add child button
            addChildButton.Enabled = e.Node != null;

            // Set layer's property grid
            layerProperties.SelectedObject = (e.Node as LayerNode).layer.properties;
        }

        // Node check changed
        private void layersTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node == rootNode)
                return;

            LayerNode node = (e.Node as LayerNode);
            node.layer.enabled = node.Checked;
        }

        private void layersTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // Prevent root from being unchecked
            if (e.Node == rootNode)
                e.Cancel = true;
        }
    }
}
