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
        private Texture2D _pixel;
        private Vector2 _mouse;
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

        // updateMousePosition
        public void updateMousePosition()
        {
            /*
            // View offset
            System.Drawing.Point viewOffset = FindForm().PointToClient(PointToScreen(pictureBox.Location));

            // Set mouse boundaries
            int x = Math.Min(Math.Max(0, Input.newMouse.X - viewOffset.X), pictureBox.Width);
            int y = Math.Min(Math.Max(0, Input.newMouse.Y - viewOffset.Y), pictureBox.Height);

            // Calculate change in mouse position (for screen and world coordinates)
            int deltaX = x - (int)_mouse.X;
            int deltaY = y - (int)_mouse.Y;

            // Move selected scrap
            if (_selectedScrap != null)
                _selectedScrap.currentCraftPosition += new Vector2(deltaX, deltaY);

            // Store screen space mouse coordinates
            _mouse.X = x;
            _mouse.Y = y;
            */
        }
        /*
        // handleXNADraw
        public void handleXNADraw()
        {
            // Draw scraps
            for (int i = _blueprint.scraps.Count - 1; i >= 0; i--)
                _spriteBatch.Draw(_blueprint.scraps[i].scrapTexture, _blueprint.scraps[i].currentCraftPosition, _blueprint.scraps[i].scrapTexture.Bounds, Color.White, 0f, _blueprint.scraps[i].textureCenter, 1f, SpriteEffects.None, 0);

            // Draw scrap sockets
            foreach (BlueprintSocket socket in _blueprint.sockets)
            {
                drawLine(
                    socket.scrapA.currentCraftPosition,
                    socket.scrapA.currentCraftPosition + socket.relativePoint,
                    Color.Green);
            }

            // Draw mouse position
            _spriteBatch.Draw(_pixel, new Vector2(_mouse.X, _mouse.Y), new Rectangle(0, 0, 4, 4), Color.Yellow, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);

            if (_socketTargetA != null)
            {
                // Draw socket line
                drawLine(_socketTargetA.currentCraftPosition, _mouse, Color.Blue);
            }
        }

        // drawLine
        private void drawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            Vector2 relative = pointB - pointA;
            float length = relative.Length();
            float angle = (float)Math.Atan2(relative.Y, relative.X);
            Rectangle rect = new Rectangle(0, 0, (int)length, 2);
            _spriteBatch.Draw(_pixel, pointA, rect, color, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
        }*/
        /*
        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = pictureBox.Handle;
        }
        */
        /*
        // Form resized
        void EditBlueprintScrapSocketsView_Resize(object sender, EventArgs e)
        {
            //_itemController.resizeGraphicsDevice(pictureBox.Width, pictureBox.Height);
        }
        */
        // Cancel clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        /*
        // Unhook from XNA
        private void EditBlueprintScrapSocketsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            XNAResources.graphics.PreparingDeviceSettings -= new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            Resize -= new EventHandler(EditBlueprintScrapSocketsView_Resize);
        }
        */
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
    }
}
