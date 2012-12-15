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
using StasisCore.Controllers;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapSocketsButton : UserControl
    {
        private List<BlueprintScrapItemResource> _scraps;

        public EditBlueprintScrapSocketsButton(List<BlueprintScrapItemResource> scraps)
        {
            _scraps = scraps;

            InitializeComponent();
            Dock = DockStyle.Top;
        }

        // Edit button clicked
        private void editSocketsButton_Click(object sender, EventArgs e)
        {
            // Validate scrap texture tags
            foreach (BlueprintScrapItemResource scrap in _scraps)
            {
                Texture2D texture = TextureController.getTexture(scrap.blueprintScrapProperties.scrapTextureTag);
                if (texture == null)
                {
                    MessageBox.Show(string.Format("Could not load the texture for scrap [{0}]", scrap.blueprintScrapProperties.scrapTextureTag));
                    return;
                }
            }

            EditBlueprintScrapSocketsView editSocketsView = new EditBlueprintScrapSocketsView(_scraps);
            if (editSocketsView.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}
