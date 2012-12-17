using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapSocketsButton : UserControl
    {
        private ItemController _itemController;
        private ItemView _itemView;
        private List<EditorBlueprintScrap> _scraps;

        public EditBlueprintScrapSocketsButton(ItemView itemView, List<EditorBlueprintScrap> scraps)
        {
            _itemController = itemView.getController();
            _itemView = itemView;
            _scraps = scraps;

            InitializeComponent();
            Dock = DockStyle.Top;
        }

        // Edit button clicked
        private void editSocketsButton_Click(object sender, EventArgs e)
        {
            // Validate scrap texture tags
            foreach (EditorBlueprintScrap scrap in _scraps)
            {
                Texture2D texture = StasisCore.Controllers.TextureController.getTexture(scrap.blueprintScrapResource.scrapTextureTag);
                if (texture == null)
                {
                    MessageBox.Show(string.Format("Could not load the texture for scrap [{0}]", scrap.blueprintScrapResource.scrapTextureTag));
                    return;
                }
            }

            // Unhook XNA from level view
            _itemController.unhookXNAFromLevel();
            _itemController.enableLevelXNAInput(false);
            _itemController.enableLevelXNADrawing(false);

            // Create view
            EditBlueprintScrapSocketsView editSocketsView = new EditBlueprintScrapSocketsView(_itemView, _scraps);
            _itemView.setEditBlueprintScrapSocketsView(editSocketsView);

            // Open view
            if (editSocketsView.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("TODO: work on a copy of the blueprint scraps, otherwise the cancel button is not necessary.");
            }

            // Clean up view
            _itemView.setEditBlueprintScrapSocketsView(null);

            // Hook XNA to level view
            _itemController.hookXNAToLevel();
            _itemController.enableLevelXNAInput(true);
            _itemController.enableLevelXNADrawing(true);
        }
    }
}
