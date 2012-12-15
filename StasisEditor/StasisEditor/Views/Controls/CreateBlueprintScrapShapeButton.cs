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
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public partial class CreateBlueprintScrapShapeButton : UserControl
    {
        BlueprintScrapItemResource _scrapResource;

        public CreateBlueprintScrapShapeButton(BlueprintScrapItemResource scrapResource)
        {
            _scrapResource = scrapResource;

            InitializeComponent();
            Dock = DockStyle.Top;
        }

        // Create blueprint scrap button clicked
        private void createBlueprintScrapButton_Click(object sender, EventArgs e)
        {
            // Test to see if the scrap texture exists
            Texture2D texture = TextureController.getTexture(_scrapResource.blueprintScrapProperties.scrapTextureTag);

            if (texture == null)
                MessageBox.Show("Could not load scrap texture. Make sure it exists.");
            else
            {
                EditBlueprintScrapShape editForm = new EditBlueprintScrapShape(texture, _scrapResource);
                if (editForm.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }
    }
}
