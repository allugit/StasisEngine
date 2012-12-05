using System;
using System.Diagnostics;
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

        // Select layer
        public void selectLayer(TerrainLayerResource layer)
        {
            LayerNode targetNode = recursiveGetNode(rootNode, layer);
            layersTreeView.SelectedNode = targetNode;
        }

        // recursiveGetLayer
        private LayerNode recursiveGetNode(TreeNode startNode, TerrainLayerResource layer)
        {
            // Check this node's layer
            if (startNode != rootNode)
            {
                LayerNode layerNode = startNode as LayerNode;
                if (layerNode.layer == layer)
                    return layerNode;
            }

            // Spawn more searches, checking the resulting layers
            foreach (TreeNode node in startNode.Nodes)
            {
                LayerNode result = recursiveGetNode(node, layer);
                if (result != null && result.layer == layer)
                    return result;
            }

            return null;
        }

        // Move layer up
        private void upButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = layersTreeView.SelectedNode;
            Debug.Assert(selectedNode != null);
            Debug.Assert(selectedNode != rootNode);
            TerrainLayerResource layer = (selectedNode as LayerNode).layer;

            // Determine parent
            TerrainLayerResource parent = selectedNode.Parent == rootNode ? null : (selectedNode.Parent as LayerNode).layer;

            // Move terrain layer up
            _controller.moveTerrainLayerUp(_material, parent, layer);

            // Refresh tree view
            populateTreeView(_material.layers);

            // Select repositioned layer
            selectLayer(layer);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Move layer down
        private void downButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = layersTreeView.SelectedNode;
            Debug.Assert(selectedNode != null);
            Debug.Assert(selectedNode != rootNode);
            TerrainLayerResource layer = (selectedNode as LayerNode).layer;

            // Determine parent
            TerrainLayerResource parent = selectedNode.Parent == rootNode ? null : (selectedNode.Parent as LayerNode).layer;

            // Move terrain layer up
            _controller.moveTerrainLayerDown(_material, parent, layer);

            // Refresh tree view
            populateTreeView(_material.layers);

            // Select repositioned layer
            selectLayer(layer);

            // Set changes made
            _controller.setChangesMade(true);
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
                TerrainLayerResource newLayer = TerrainLayerResource.create(newLayerForm.getSelectedType());
                _controller.addTerrainLayer(_material, newLayer, parent);

                // Refresh the tree view
                populateTreeView(_material.layers);

                // Select new layer
                selectLayer(newLayer);

                // Refocus on tree view
                layersTreeView.Focus();

                // Set changes made
                _controller.setChangesMade(true);
            }
        }

        // Remove layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = layersTreeView.SelectedNode;
            Debug.Assert(selectedNode != null);
            Debug.Assert(selectedNode != rootNode);

            // Display a warning that child nodes will be destroyed
            if (MessageBox.Show("Are you sure you want to remove this node and all its child nodes?", "Remove Nodes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Determine parent layer
                TerrainLayerResource parent = selectedNode.Parent == rootNode ? null : (selectedNode.Parent as LayerNode).layer;

                // Remove layer
                _controller.removeTerrainLayer(_material, parent, (selectedNode as LayerNode).layer);

                // Refresh tree view
                populateTreeView(_material.layers);

                // Set changes made
                _controller.setChangesMade(true);
            }
        }

        // recursiveEnableNode
        private void recursiveEnableNode(TreeNode startNode, bool enabled)
        {
            // This only fakes a disabled node by changing its forecolor
            startNode.ForeColor = enabled ? Color.Black : Color.Gray;

            // Call the chillens'
            foreach (TreeNode childNode in startNode.Nodes)
                recursiveEnableNode(childNode, enabled);
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

            // Enable/disable add child button
            addChildButton.Enabled = e.Node != null;

            if (e.Node == rootNode)
            {
                // Disable both up and down buttons
                upButton.Enabled = false;
                downButton.Enabled = false;
                return;
            }
            else
            {
                // Enable/disable move up button
                upButton.Enabled = e.Node.Index > 0;

                // Enable/disable move down button
                downButton.Enabled = e.Node.Index < e.Node.Parent.Nodes.Count - 1;
            }

            // Set layer's property grid
            layerProperties.SelectedObject = (e.Node as LayerNode).layer.properties;
        }

        // Node check changed
        private void layersTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Debug.Assert(e.Node != rootNode);

            // Enable/disable layer
            LayerNode node = (e.Node as LayerNode);
            node.layer.enabled = node.Checked;

            // Enable/disable child nodes
            // (nodes only, not layers -- the renderer stops at the first disabled layer anyway, so it's effectively the same behavior as disabling the layers)
            foreach (TreeNode childNode in e.Node.Nodes)
                recursiveEnableNode(childNode, node.Checked);

            // Set changes made
            _controller.setChangesMade(true);
        }

        private void layersTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // Prevent root from being unchecked
            if (e.Node == rootNode)
                e.Cancel = true;
        }
    }
}
