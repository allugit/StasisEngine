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
    }
}
