using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor
{
    public partial class EditorForm : Form
    {
        private Editor editor;
        public SplitterPanel levelSurfaceContainer { get { return mainSplit.Panel2; } }

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

        // enableNewLevel
        public void enableNewLevel(bool status)
        {
            menuLevelNew.Enabled = status;
        }

        // New level clicked
        private void menuLevelNew_Click(object sender, EventArgs e)
        {
            // Create a level
            editor.createLevel();
        }
    }
}
