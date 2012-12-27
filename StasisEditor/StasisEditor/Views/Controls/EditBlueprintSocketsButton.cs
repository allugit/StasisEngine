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
    public partial class EditBlueprintSocketsButton : UserControl
    {
        private BlueprintController _blueprintController;
        private Blueprint _blueprint;
        private BlueprintView _view;

        public EditBlueprintSocketsButton(BlueprintController blueprintController, Blueprint blueprint)
        {
            _blueprintController = blueprintController;
            _blueprint = blueprint;
            _view = _blueprintController.view;

            InitializeComponent();
            Dock = DockStyle.Top;
        }

        // Edit button clicked
        private void editSocketsButton_Click(object sender, EventArgs e)
        {
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
            //_blueprintController.unhookXNAFromLevel();
            _blueprintController.enableLevelXNAInput(false);
            //_blueprintController.enableLevelXNADrawing(false);

            // Create view
            EditBlueprintSocketsView editSocketsView = new EditBlueprintSocketsView(_blueprint);
            _view.editBlueprintSocketsView = editSocketsView;

            // Open view
            if (editSocketsView.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("TODO: work on a copy of the blueprint scraps, otherwise the cancel button is not necessary.");
            }

            // Clean up view
            _view.editBlueprintSocketsView = null;

            // Hook XNA to level view
            //_blueprintController.hookXNAToLevel();
            _blueprintController.enableLevelXNAInput(true);
            //_blueprintController.enableLevelXNADrawing(true);
        }
    }
}
