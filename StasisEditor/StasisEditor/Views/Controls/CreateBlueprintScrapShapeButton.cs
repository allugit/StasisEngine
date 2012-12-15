using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Controllers;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public partial class CreateBlueprintScrapShapeButton : UserControl
    {
        BlueprintScrapItemResource _scrapResource;
        ItemView _itemView;
        ItemController _itemController;

        public CreateBlueprintScrapShapeButton(ItemView itemView, BlueprintScrapItemResource scrapResource)
        {
            _itemView = itemView;
            _itemController = _itemView.getController();
            _scrapResource = scrapResource;

            InitializeComponent();
            Dock = DockStyle.Top;
        }

        // Create blueprint scrap button clicked
        private void createBlueprintScrapButton_Click(object sender, EventArgs e)
        {
            // Test to see if the scrap texture exists
            Texture2D texture = StasisCore.Controllers.TextureController.getTexture(_scrapResource.blueprintScrapProperties.scrapTextureTag);

            if (texture == null)
                MessageBox.Show("Could not load scrap texture. Make sure it exists.");
            else
            {
                // Unhook XNA from level view
                _itemController.unhookXNAFromLevel();
                _itemController.enableLevelInput(false);

                // Create edit view
                EditBlueprintScrapShape editForm = new EditBlueprintScrapShape(_itemView, texture, _scrapResource);
                _itemView.setEditBlueprintScrapView(editForm);
                _itemController.enableEditBlueprintScrapInput(true);

                // Open edit view
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                }

                // Close edit view
                _itemView.setEditBlueprintScrapView(null);
                _itemController.enableEditBlueprintScrapInput(false);

                // Hook XNA to level view
                _itemController.hookXNAToLevel();
                _itemController.enableLevelInput(true);
            }
        }
    }
}
