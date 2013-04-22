using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;

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

        public WorldMapView()
        {
            InitializeComponent();
            worldMapDisplay1.view = this;
        }
    }
}
