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
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class CreateBlueprintScrapShapeButton : UserControl
    {
        EditorBlueprintScrap _scrap;
        ItemView _itemView;
        ItemController _itemController;

        public CreateBlueprintScrapShapeButton(ItemView itemView, EditorBlueprintScrap scrap)
        {
            _itemView = itemView;
            _itemController = _itemView.getController();
            _scrap = scrap;

            InitializeComponent();
            Dock = DockStyle.Top;
        }

        // Create blueprint scrap button clicked
        private void createBlueprintScrapButton_Click(object sender, EventArgs e)
        {
            // Test to see if the scrap texture exists
            Texture2D texture = StasisCore.Controllers.TextureController.getTexture(_scrap.blueprintScrapResource.scrapTextureTag);

            if (texture == null)
                MessageBox.Show("Could not load scrap texture. Make sure it exists.");
            else
            {
                // Unhook XNA from level view
                _itemController.unhookXNAFromLevel();
                _itemController.enableLevelXNAInput(false);
                _itemController.enableLevelXNADrawing(false);

                // Create edit view
                EditBlueprintScrapShape editForm = new EditBlueprintScrapShape(_itemView, texture, _scrap);
                _itemView.setEditBlueprintScrapShapeView(editForm);

                // Open edit view
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Set scrap points
                    _scrap.points = editForm.getPoints();
                }

                // Close edit view
                _itemView.setEditBlueprintScrapShapeView(null);

                // Hook XNA to level view
                _itemController.hookXNAToLevel();
                _itemController.enableLevelXNAInput(true);
                _itemController.enableLevelXNADrawing(true);
            }
        }
    }
}
