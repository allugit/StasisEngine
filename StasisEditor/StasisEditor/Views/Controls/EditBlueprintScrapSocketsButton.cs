using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Controllers;
using StasisCore.Resources;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapSocketsButton : UserControl
    {
        private EditorController _editorController;
        private Blueprint _blueprint;
        private BlueprintView _view;

        public EditBlueprintScrapSocketsButton(EditorController editorController, Blueprint blueprint)
        {
            _editorController = editorController;
            _blueprint = blueprint;

            InitializeComponent();
            Dock = DockStyle.Top;
        }

        // Edit button clicked
        private void editSocketsButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();

            // Validate scrap texture tags
            foreach (BlueprintScrap scrap in _blueprint.scraps)
            {
                if (!ResourceController.exists(scrap.scrapTextureUID))
                {
                    MessageBox.Show(string.Format("Could not load the texture for scrap [{0}]", scrap.scrapTextureUID));
                    return;
                }
            }

            // Unhook XNA from level view
            _editorController.unhookXNAFromLevel();
            _editorController.enableLevelXNAInput(false);
            _editorController.enableLevelXNADrawing(false);

            // Create view
            EditBlueprintScrapSocketsView editSocketsView = new EditBlueprintScrapSocketsView(_blueprint);
            _view.setEditBlueprintScrapSocketsView(editSocketsView);

            // Open view
            if (editSocketsView.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("TODO: work on a copy of the blueprint scraps, otherwise the cancel button is not necessary.");
            }

            // Clean up view
            _view.setEditBlueprintScrapSocketsView(null);

            // Hook XNA to level view
            _editorController.hookXNAToLevel();
            _editorController.enableLevelXNAInput(true);
            _editorController.enableLevelXNADrawing(true);
        }
    }
}
