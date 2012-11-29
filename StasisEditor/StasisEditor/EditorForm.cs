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
        private Editor editor;

        // Constructor
        public EditorForm(Editor editor)
        {
            this.editor = editor;
            InitializeComponent();
        }

        // EditorForm closed event
        private void EditorForm_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void EditorForm_Resize(object sender, EventArgs e)
        {
            // Resize graphics device
            if (surface.Width > 0 && surface.Height > 0)
            {
                editor.main.graphics.PreferredBackBufferWidth = surface.Width;
                editor.main.graphics.PreferredBackBufferHeight = surface.Height;
                editor.main.graphics.ApplyChanges();
            }
        }
    }
}
