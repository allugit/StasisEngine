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

            editBlueprintSocketsGraphics.blueprint = _blueprint;
        }

        /*
        // Picture box clicked
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Hit test scraps
            BlueprintScrap target = null;
            for (int i = 0; i < _blueprint.scraps.Count; i++)
            {
                if (_blueprint.scraps[i].hitTest(_mouse))
                {
                    target = _blueprint.scraps[i];
                    break;
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_selectedScrap == null)
                {
                    // Select scrap
                    _selectedScrap = target;
                }
                else
                {
                    // Place selected scrap
                    _selectedScrap = null;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (_socketTargetA == null)
                {
                    // Set first socket target
                    _socketTargetA = target;
                }
                else
                {
                    // Create socket on first target
                    Vector2 relativePoint = target.currentCraftPosition - _socketTargetA.currentCraftPosition;
                    BlueprintSocket firstSocket = new BlueprintSocket(_socketTargetA, target, relativePoint);
                    _blueprint.sockets.Add(firstSocket);

                    // Create socket on second target
                    BlueprintSocket secondSocket = new BlueprintSocket(target, _socketTargetA, -relativePoint);
                    _blueprint.sockets.Add(secondSocket);

                    // Set opposing sockets
                    firstSocket.opposingSocket = secondSocket;
                    secondSocket.opposingSocket = firstSocket;

                    // Clear socket target
                    _socketTargetA = null;
                }
            }

            // Enable save button
            saveButton.Enabled = true;
        }*/

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
