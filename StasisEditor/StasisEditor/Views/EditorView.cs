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
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public partial class EditorView : Form, IEditorView
    {
        private EditorController _controller;
        private ILevelView _levelView;
        private LevelSettings _levelSettings;
        private BrushToolbar _brushToolbar;

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

        // getMaterialView
        public IMaterialView getMaterialView()
        {
            return materialView;
        }

        // getLevelContainerSize
        public System.Drawing.Point getLevelContainerSize()
        {
            return new System.Drawing.Point(mainSplit.Panel2.Width, mainSplit.Panel2.Height);
        }

        // addLevelView
        public void addLevelView(ILevelView levelView)
        {
            _levelView = levelView;
            //mainSplit.Panel2.Controls.Add(levelView as Control);
            levelTab.Controls.Add(levelView as Control);
        }

        // removeLevelView
        public void removeLevelView()
        {
            mainSplit.Panel2.Controls.Remove(_levelView as Control);
            _levelView.Dispose();
            _levelView = null;
        }

        // addLevelSettings
        public void addLevelSettings(Level level)
        {
            _levelSettings = new LevelSettings(level, mainSplit.Panel1.Width, 200);
            mainSplit.Panel1.Controls.Add(_levelSettings);
        }

        // removeLevelSettings
        public void removeLevelSettings()
        {
            mainSplit.Panel1.Controls.Remove(_levelSettings);
            _levelSettings.Dispose();
            _levelSettings = null;
        }

        // addBrushToolbar
        public void addBrushToolbar()
        {
            _brushToolbar = new BrushToolbar();
            mainSplit.Panel1.Controls.Add(_brushToolbar);
        }

        // removeBrushToolbar
        public void removeBrushToolbar()
        {
            mainSplit.Panel1.Controls.Remove(_brushToolbar);
            _brushToolbar.Dispose();
            _brushToolbar = null;
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

        // enableCloseLevel
        public void enableCloseLevel(bool enabled)
        {
            menuLevelClose.Enabled = enabled;
        }

        // enableSaveLevel
        public void enableSaveLevel(bool enabled)
        {
            menuLevelSave.Enabled = enabled;
        }

        // enableLoadLevel
        public void enableLoadLevel(bool enabled)
        {
            menuLevelLoad.Enabled = enabled;
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

        // Close level clicked
        private void menuLevelClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the level?", "Close Level", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _controller.closeLevel();
            }
        }

        // Materials clicked
        private void menuAssetsMaterials_Click(object sender, EventArgs e)
        {
            //_controller.openMaterialView();
        }

        // Textures clicked
        private void menuAssetsTextures_Click(object sender, EventArgs e)
        {
            _controller.openTextureView();
        }
    }
}
