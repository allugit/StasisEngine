using System;
using System.Diagnostics;
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
    public partial class MaterialLayersControl : UserControl
    {
        private MaterialController _controller;
        private Material _material;

        public MaterialLayersControl(MaterialController controller, Material material)
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
        public void populateTreeView(MaterialGroupLayer rootLayer)
        {
            SuspendLayout();

            List<MaterialLayer> layers = rootLayer.layers;

            // Clear existing nodes
            layersTreeView.Nodes.Clear();

            // Build nodes
            LayerNode rootNode = recursiveBuildNode(rootLayer);

            // Set tree view to root node
            layersTreeView.Nodes.Add(rootNode);

            // Expand all
            layersTreeView.ExpandAll();

            ResumeLayout();
        }

        // recursiveBuildNode
        private LayerNode recursiveBuildNode(MaterialLayer layer)
        {
            LayerNode node = new LayerNode(layer, layer.enabled);
            if (layer.type == "root" || layer.type == "group")
            {
                MaterialGroupLayer groupLayer = layer as MaterialGroupLayer;
                foreach (MaterialLayer childLayer in groupLayer.layers)
                    node.Nodes.Add(recursiveBuildNode(childLayer));
            }
            return node;
        }

        // Select layer
        public void selectLayer(MaterialLayer layer)
        {
            Debug.Assert(layersTreeView.Nodes[0] is LayerNode);
            LayerNode targetNode = recursiveGetNode(layersTreeView.Nodes[0] as LayerNode, layer);
            layersTreeView.SelectedNode = targetNode;
        }

        // recursiveGetNode
        private LayerNode recursiveGetNode(LayerNode startNode, MaterialLayer layer)
        {
            // Check this node's layer
            if (startNode.layer == layer)
                return startNode;

            if (startNode.layer.type == "root" || startNode.layer.type == "group")
            {
                // Spawn more searches, checking results
                foreach (TreeNode node in startNode.Nodes)
                {
                    LayerNode result = recursiveGetNode(node as LayerNode, layer);
                    if (result != null && result.layer == layer)
                        return result;
                }
            }

            return null;
        }

        // Move layer up
        private void upButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);
            LayerNode node = layersTreeView.SelectedNode as LayerNode;

            // Move layer up
            MaterialGroupLayer parent = (node.Parent as LayerNode).layer as MaterialGroupLayer;
            _controller.moveTerrainLayerUp(parent, node.layer);

            // Refresh tree view
            populateTreeView(_material.rootLayer);

            // Select repositioned layer
            selectLayer(node.layer);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Move layer down
        private void downButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);
            LayerNode node = layersTreeView.SelectedNode as LayerNode;

            // Move layer down
            MaterialGroupLayer parent = (node.Parent as LayerNode).layer as MaterialGroupLayer;
            _controller.moveTerrainLayerDown(parent, node.layer);

            // Refresh tree view
            populateTreeView(_material.rootLayer);

            // Select repositioned layer
            selectLayer(node.layer);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Add layer button clicked
        private void addButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);

            SelectMaterialLayerType newLayerForm = new SelectMaterialLayerType();
            if (newLayerForm.ShowDialog() == DialogResult.OK)
            {
                LayerNode node = layersTreeView.SelectedNode as LayerNode;
                MaterialLayer newLayer = MaterialLayer.create(newLayerForm.getSelectedType());

                if (node.layer.type == "root" || node.layer.type == "group")
                {
                    // Insert into group
                    _controller.addTerrainLayer(node.layer as MaterialGroupLayer, newLayer, node.Nodes.Count);
                }
                else
                {
                    // Create new layer
                    LayerNode parent = node.Parent as LayerNode;

                    // Add new layer to parent
                    _controller.addTerrainLayer(parent.layer as MaterialGroupLayer, newLayer, node.Index + 1);
                }

                // Refresh tree
                populateTreeView(_material.rootLayer);

                // Select layer
                selectLayer(newLayer);

                // Refocus on tree
                layersTreeView.Focus();

                // Set changes made
                _controller.setChangesMade(true);
            }
        }

        // Remove layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);
            LayerNode node = layersTreeView.SelectedNode as LayerNode;
            Debug.Assert(node.layer.type != "root");

            // Display a warning that child nodes will be destroyed
            bool doRemove = false;
            if (node.layer.type == "group")
                doRemove = MessageBox.Show("Are you sure you want to remove this node and all its child nodes?", "Remove Nodes", MessageBoxButtons.YesNo) == DialogResult.Yes;
            else
                doRemove = true;

            if (doRemove)
            {
                // Remove layer
                MaterialGroupLayer parent = (node.Parent as LayerNode).layer as MaterialGroupLayer;
                _controller.removeTerrainLayer(parent, node.layer);

                // Refresh tree view
                populateTreeView(_material.rootLayer);

                // Select different layer
                if (parent.layers.Count > 0)
                    selectLayer(parent.layers[parent.layers.Count - 1]);
                else
                    selectLayer(parent);

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
            Debug.Assert(e.Node is LayerNode);
            LayerNode node = e.Node as LayerNode;

            // Remove button's status
            removeLayerButton.Enabled = node.layer.type != "root";

            // Add button's status
            addLayerButton.Enabled = true;

            // Up/down buttons' status
            if (node.layer.type == "root")
            {
                upButton.Enabled = false;
                downButton.Enabled = false;
            }
            else
            {
                upButton.Enabled = e.Node.Index > 0;
                downButton.Enabled = e.Node.Index < e.Node.Parent.Nodes.Count - 1;
            }

            // Set layer's property grid
            layerProperties.SelectedObject = node.layer;
        }

        // Node check changed
        private void layersTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Debug.Assert(e.Node is LayerNode);
            LayerNode node = e.Node as LayerNode;
            Debug.Assert(node.layer.type != "root");

            // Set layer's enabled status
            node.layer.enabled = node.Checked;

            // Recursively update child nodes' enabled status
            // (nodes only, not layers -- the renderer stops at the first disabled layer anyway, so it's effectively the same behavior as disabling the layers)
            foreach (TreeNode childNode in e.Node.Nodes)
                recursiveEnableNode(childNode, node.Checked);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Checkbox validation
        private void layersTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // Prevent root node from being unchecked
            Debug.Assert(e.Node is LayerNode);
            LayerNode node = e.Node as LayerNode;
            if (node.layer.type == "root")
                e.Cancel = true;
        }
    }
}
