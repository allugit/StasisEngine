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

        // Move layer up
        private void upButton_Click(object sender, EventArgs e)
        {
            // Return if selected index is already at top
            int selectedIndex = layersListBox.SelectedIndex;
            if (selectedIndex == 0)
                return;

            SuspendLayout();

            TerrainLayer selectedLayer = _layers[selectedIndex];
            _layers.Remove(selectedLayer);
            _layers.Insert(selectedIndex - 1, selectedLayer);
            layersListBox.DataSource = null;
            layersListBox.DataSource = _layers;
            layersListBox.SelectedItem = selectedLayer;

            ResumeLayout();

            // Inform controller that changes were made
            _controller.setChangesMade(true);
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
            {
                SuspendLayout();

                _layers.Add(TerrainLayer.create(selectBox.getSelectedType()));
                layersListBox.DataSource = null;
                layersListBox.DataSource = _layers;
                layersListBox.SelectedItem = _layers[_layers.Count - 1];

                ResumeLayout();

                // Inform controller that changes were made
                _controller.setChangesMade(true);
            }
        }

        // Remove layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            // Return if there are no layers to remove
            if (layersListBox.Items.Count == 0)
                return;

            int selectedIndex = layersListBox.SelectedIndex;

            SuspendLayout();
            _layers.RemoveAt(selectedIndex);
            layersListBox.DataSource = null;
            layersListBox.DataSource = _layers;

            // Change layer selection
            if (_layers.Count == 0)
                layersListBox.SelectedItem = null;
            else if (selectedIndex < _layers.Count)
                layersListBox.SelectedItem = _layers[selectedIndex];
            else if (selectedIndex - 1 >= 0)
                layersListBox.SelectedItem = _layers[selectedIndex - 1];

            ResumeLayout();

            // Inform controller of changes
            _controller.setChangesMade(true);
        }

        // Selected layer changed
        private void layersListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (layersListBox.DataSource != null)
                layerProperties.SelectedObject = (layersListBox.SelectedItem as TerrainLayer).properties;
            else
                layerProperties.SelectedObject = null;
        }

        // Property value changed
        private void layerProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _controller.setChangesMade(true);
        }
    }
}
