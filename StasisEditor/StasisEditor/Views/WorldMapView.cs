using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public partial class WorldMapView : UserControl
    {
        private WorldMapController _controller;
        private bool _draw;
        private bool _keysEnabled;

        public bool active
        {
            get { return _draw && _keysEnabled; }
            set { _draw = value; _keysEnabled = value; }
        }
        public bool keysEnabled { get { return _keysEnabled; } }
        public WorldMapController controller { get { return _controller; } set { _controller = value; } }
        public BindingList<EditorWorldMap> worldMaps { set { worldMapListBox.DataSource = value; } }

        public WorldMapView()
        {
            InitializeComponent();
            worldMapDisplay1.view = this;
        }

        private void worldMapListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            EditorWorldMap selectedWorldMap = worldMapListBox.SelectedItem as EditorWorldMap;

            worldMapPropertyGrid.SelectedObject = selectedWorldMap;
        }
    }
}
