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

        private void EditorForm_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
