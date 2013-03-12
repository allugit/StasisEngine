using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Models;
using StasisCore;

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
        }

        // Form load
        private void EditBlueprintSocketsView_Load(object sender, EventArgs e)
        {
            // Initialize blueprint scrap textures
            foreach (BlueprintScrap scrap in _blueprint.scraps)
            {
                // Texture
                if (scrap.scrapTexture == null)
                    scrap.scrapTexture = ResourceManager.getTexture(scrap.scrapTextureUID);
                scrap.textureCenter = new Vector2(scrap.scrapTexture.Width, scrap.scrapTexture.Height) / 2;
            }

            // Move blueprint scraps into their ideal positions by testing sockets with a large tolerance
            foreach (BlueprintSocket socket in _blueprint.sockets)
                socket.test(float.MaxValue);

            editBlueprintSocketsGraphics.blueprint = _blueprint;
        }

        // Clear sockets (and connections)
        private void clearSocketsButton_Click(object sender, EventArgs e)
        {
            _blueprint.sockets.Clear();
            foreach (BlueprintScrap scrap in _blueprint.scraps)
                scrap.connected.Clear();
        }

        // Reset position
        private void resetPositionsButton_Click(object sender, EventArgs e)
        {
            bool doClear = true;
            if (_blueprint.sockets.Count > 0)
            {
                if (MessageBox.Show("Resetting positions will clear all sockets and their connections. Continue?", "Clear connections too?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    doClear = false;
            }

            if (doClear)
            {
                _blueprint.sockets.Clear();
                foreach (BlueprintScrap scrap in _blueprint.scraps)
                {
                    scrap.currentCraftPosition = Vector2.Zero;
                    scrap.connected.Clear();
                }
            }
        }

        // OK clicked
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
