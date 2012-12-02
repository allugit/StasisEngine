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
    public partial class TerrainLayers : UserControl
    {
        private IController _controller;
        private List<TerrainLayer> _layers;

        public TerrainLayers(IController controller, List<TerrainLayer> layers)
        {
            _controller = controller;
            _layers = layers;

            // Controls
            InitializeComponent();
            Dock = DockStyle.Fill;
            upButton.Text = char.ConvertFromUtf32(0x000002c4);
            downButton.Text = char.ConvertFromUtf32(0x000002c5);
            layersListBox.DataSource = _layers;
        }

        // Add new layer
        public void addNewLayer(TerrainLayerType layerType)
        {
            // Inform controller that changes are being made
            _controller.setChangesMade(true);

            SuspendLayout();

            _layers.Add(TerrainLayer.create(layerType));
            layersListBox.DataSource = null;
            layersListBox.DataSource = _layers;

            ResumeLayout();
        }

        // Move layer up
        private void upButton_Click(object sender, EventArgs e)
        {
            // Return if selected index is already at top
            int selectedIndex = layersListBox.SelectedIndex;
            if (selectedIndex == 0)
                return;

            // Inform controller that changes are being made
            _controller.setChangesMade(true);

            SuspendLayout();

            TerrainLayer selectedLayer = _layers[selectedIndex];
            _layers.Remove(selectedLayer);
            _layers.Insert(selectedIndex - 1, selectedLayer);
            layersListBox.DataSource = null;
            layersListBox.DataSource = _layers;
            layersListBox.SelectedItem = selectedLayer;

            ResumeLayout();
        }

        // Move layer down
        private void downButton_Click(object sender, EventArgs e)
        {
            // Return if selected index is already at bottom
            int selectedIndex = layersListBox.SelectedIndex;
            if (selectedIndex == _layers.Count - 1)
                return;

            // Inform controller that changes are being made
            _controller.setChangesMade(true);

            SuspendLayout();

            TerrainLayer selectedLayer = _layers[selectedIndex];
            _layers.Remove(selectedLayer);
            _layers.Insert(selectedIndex + 1, selectedLayer);
            layersListBox.DataSource = null;
            layersListBox.DataSource = _layers;
            layersListBox.SelectedItem = selectedLayer;

            ResumeLayout();
        }

        // Add new layer
        private void addLayerButton_Click(object sender, EventArgs e)
        {
            TerrainLayerSelectBox selectBox = new TerrainLayerSelectBox();
            selectBox.StartPosition = FormStartPosition.CenterParent;
            if (selectBox.ShowDialog() == DialogResult.OK)
                addNewLayer(selectBox.getSelectedType());
        }

        // Remove layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            // Return if there are no layers to remove
            if (layersListBox.Items.Count == 0)
                return;

            // Inform controller of changes
            _controller.setChangesMade(true);

            SuspendLayout();
            _layers.RemoveAt(layersListBox.SelectedIndex);
            layersListBox.DataSource = null;
            layersListBox.DataSource = _layers;
            ResumeLayout();
        }

        // Selected layer changed
        private void layersListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            layerProperties.SelectedObject = (layersListBox.SelectedItem as TerrainLayer).properties;
        }
    }
}
