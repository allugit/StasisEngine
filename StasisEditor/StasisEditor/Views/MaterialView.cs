using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;
using StasisEditor.Models;
using StasisCore;

namespace StasisEditor.Views
{
    public partial class MaterialView : UserControl
    {
        private MaterialController _controller;

        public EditorMaterial selectedMaterial { get { return materialsListBox.SelectedItem as EditorMaterial; } }
        public MaterialLayer selectedLayer { get { return layersTreeView.SelectedNode == null ? null : (layersTreeView.SelectedNode as LayerNode).layer; } }

        public MaterialView()
        {
            InitializeComponent();
            upButton.Text = char.ConvertFromUtf32(0x000002c4);
            downButton.Text = char.ConvertFromUtf32(0x000002c5);
        }

        // getController
        public MaterialController getController()
        {
            return _controller;
        }

        // setController
        public void setController(MaterialController controller)
        {
            _controller = controller;
            materialsListBox.DataSource = controller.materials;
        }

        // Refresh material list
        public void refreshMaterialList()
        {
            materialsListBox.RefreshItems();
        }

        // Selected materials changed
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enable/disable remove button
            removeButton.Enabled = selectedMaterial != null;

            // Enable/disable clone button
            cloneButton.Enabled = selectedMaterial != null;

            // Populate layers tree
            populateTreeView(selectedMaterial.rootLayer);

            if (selectedMaterial != null)
            {
                layersTreeView.SelectedNode = layersTreeView.Nodes[0];
                materialPropertyGrid.SelectedObject = selectedMaterial;
            }
        }

        // Material property changed
        private void materialProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Refresh the materials list
            (materialsListBox as RefreshingListBox).RefreshItems();
        }

        // Material save button
        private void saveButton_Click(object sender, EventArgs e)
        {
            _controller.saveMaterials();
        }

        // Add material clicked
        private void addButton_Click(object sender, EventArgs e)
        {
            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                _controller.createMaterial(createResourceView.uid);
                materialsListBox.RefreshItems();
            }
        }

        // Remove material clicked
        private void removeButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(selectedMaterial != null);

            try
            {
                _controller.removeMaterial(selectedMaterial.uid, true);
                removeButton.Enabled = false;
                materialsListBox.RefreshItems();
            }
            catch (InvalidResourceException exception)
            {
                MessageBox.Show(exception.Message, "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ResourceNotFoundException exception)
            {
                MessageBox.Show(exception.Message, "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clone button clicked
        private void cloneButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(selectedMaterial != null);

            _controller.cloneMaterial(selectedMaterial);
            materialsListBox.RefreshItems();
        }

        // Material property changed
        private void materialPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Refresh materials list
            refreshMaterialList();

            // Set changes to true
            _controller.setChangesMade(true);
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
            populateTreeView(selectedMaterial.rootLayer);

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
            populateTreeView(selectedMaterial.rootLayer);

            // Select repositioned layer
            selectLayer(node.layer);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Add layer button clicked
        private void addLayerButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);

            SelectMaterialLayerType newLayerForm = new SelectMaterialLayerType();
            if (newLayerForm.ShowDialog() == DialogResult.OK)
            {
                LayerNode node = layersTreeView.SelectedNode as LayerNode;
                MaterialLayer newLayer = EditorMaterialLayer.create(newLayerForm.getSelectedType());
                addNewLayer(newLayer);
            }
        }

        // Add layer
        private void addNewLayer(MaterialLayer newLayer)
        {
            Debug.Assert(selectedLayer != null);
            LayerNode selectedNode = layersTreeView.SelectedNode as LayerNode;

            if (selectedLayer.type == "root" || selectedLayer.type == "group")
            {
                // Insert into group
                _controller.addTerrainLayer(selectedLayer as MaterialGroupLayer, newLayer, selectedNode.Nodes.Count);
            }
            else
            {
                // Create new layer
                LayerNode parent = selectedNode.Parent as LayerNode;

                // Add new layer to parent
                _controller.addTerrainLayer(parent.layer as MaterialGroupLayer, newLayer, selectedNode.Index + 1);
            }

            // Refresh tree
            populateTreeView(selectedMaterial.rootLayer);

            // Select layer
            selectLayer(newLayer);

            // Refocus on tree
            layersTreeView.Focus();

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Remove layer button clicked
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
                populateTreeView(selectedMaterial.rootLayer);

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

            // Copy/paste button's status
            layerCopyButton.Enabled = true;
            layerPasteButton.Enabled = _controller.copiedMaterialLayer != null;

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

        // Copy layer button clicked
        private void layerCopyButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(selectedLayer != null);
            _controller.copiedMaterialLayer = selectedLayer.clone();
            layerPasteButton.Enabled = true;
        }

        // Paste layer button clicked
        private void layerPasteButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(selectedLayer != null);
            Debug.Assert(_controller.copiedMaterialLayer != null);
            addNewLayer(_controller.copiedMaterialLayer.clone());
        }
    }
}
