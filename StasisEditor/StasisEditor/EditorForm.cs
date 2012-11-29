using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StasisEditor
{
    public partial class EditorForm : Form
    {
        public Main main;

        // Constructor
        public EditorForm()
        {
            InitializeComponent();
        }

        // getSurface
        public PictureBox getSurface()
        {
            return surface;
        }

        // EditorForm closed event
        private void EditorForm_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void EditorForm_Resize(object sender, EventArgs e)
        {
            // Resize surface
            //surface.Width = Width - surface.Location.X;
            //surface.Height = Height;

            // Resize main options control
            //mainOptionsControl.Height = Height - 103;

            // Resize graphics device
            if (surface.Width > 0 && surface.Height > 0)
            {
                Main.graphics.PreferredBackBufferWidth = surface.Width;
                Main.graphics.PreferredBackBufferHeight = surface.Height;
                Main.graphics.ApplyChanges();
            }
        }
    }
}
