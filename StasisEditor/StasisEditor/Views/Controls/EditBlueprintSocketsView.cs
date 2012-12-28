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
using StasisCore.Resources;
using StasisCore.Models;
using StasisCore.Controllers;
using StasisEditor.Controllers;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintSocketsView : Form
    {
        private Blueprint _blueprint;
        private BlueprintScrap _selectedScrap;
        private BlueprintScrap _socketTargetA;

        public BlueprintScrap socketTargetA { get { return _socketTargetA; } set { _socketTargetA = value; } }
        public BlueprintScrap selectedScrap { get { return _selectedScrap; } set { _selectedScrap = value; } }

        public EditBlueprintSocketsView(Blueprint blueprint)
        {
            _blueprint = blueprint;

            InitializeComponent();

            // Initialize blueprint scrap textures
            foreach (BlueprintScrap scrap in _blueprint.scraps)
            {
                // Texture
                if (scrap.scrapTexture == null)
                    scrap.scrapTexture = ResourceController.getTexture(scrap.scrapTextureUID);
                scrap.textureCenter = new Vector2(scrap.scrapTexture.Width, scrap.scrapTexture.Height) / 2;
            }

            // Move blueprint scraps into their ideal positions by testing sockets with a large tolerance
            Console.WriteLine("testing");
            foreach (BlueprintSocket socket in _blueprint.sockets)
                socket.test(float.MaxValue);

            editBlueprintSocketsGraphics.blueprint = _blueprint;
        }

        // Save button clicked
        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        // Cancel clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
