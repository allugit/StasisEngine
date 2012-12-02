using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Controls
{
    public partial class TerrainLayers : UserControl
    {
        private List<TerrainLayer> _layers;

        public TerrainLayers(List<TerrainLayer> layers)
        {
            _layers = layers;

            // Controls
            InitializeComponent();
            Dock = DockStyle.Top;
            upButton.Text = char.ConvertFromUtf32(0x000002c4);
            downButton.Text = char.ConvertFromUtf32(0x000002c5);
            layersListBox.DataSource = _layers;
        }

        // Move layer up
        private void upButton_Click(object sender, EventArgs e)
        {
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
        }

        // Move layer down
        private void downButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = layersListBox.SelectedIndex;
            if (selectedIndex == _layers.Count - 1)
                return;

            SuspendLayout();

            TerrainLayer selectedLayer = _layers[selectedIndex];
            _layers.Remove(selectedLayer);
            _layers.Insert(selectedIndex + 1, selectedLayer);
            layersListBox.DataSource = null;
            layersListBox.DataSource = _layers;
            layersListBox.SelectedItem = selectedLayer;

            ResumeLayout();
        }
    }
}
