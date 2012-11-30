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
using StasisEditor.Controller;

namespace StasisEditor.View
{
    public partial class EditorView : Form, IEditorView
    {
        private EditorController _controller;

        // Constructor
        public EditorView()
        {
            InitializeComponent();
            Show();
        }

        // setController
        public void setController(EditorController controller)
        {
            _controller = controller;
        }

        // getLevelContainerSize
        public System.Drawing.Point getLevelContainerSize()
        {
            return new System.Drawing.Point(mainSplit.Panel2.Width, mainSplit.Panel2.Height);
        }

        // addLevelView
        public void addLevelView(ILevelView levelView)
        {
            mainSplit.Panel2.Controls.Add(levelView as Control);
        }

        // removeLevelView
        public void removeLevelView(ILevelView levelView)
        {
            mainSplit.Panel2.Controls.Remove(levelView as Control);
        }

        // EditorForm closed event
        private void EditorForm_Closed(object sender, FormClosedEventArgs e)
        {
            _controller.exit();
        }

        // enableNewLevel
        public void enableNewLevel(bool enabled)
        {
            menuLevelNew.Enabled = enabled;
        }

        // New level clicked
        private void menuLevelNew_Click(object sender, EventArgs e)
        {
            // Create a new level
            _controller.createNewLevel();
        }

        // Menu exit clicked
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            _controller.exit();
        }
    }
}
