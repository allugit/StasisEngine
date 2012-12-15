using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapShape : Form
    {
        private BlueprintScrapItemResource _scrapResource;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private ItemView _itemView;
        private Point _mouse;

        public EditBlueprintScrapShape(ItemView itemView, Texture2D texture, BlueprintScrapItemResource scrapResource)
        {
            _itemView = itemView;
            _texture = texture;
            _scrapResource = scrapResource;
            _spriteBatch = XNAResources.spriteBatch;

            InitializeComponent();

            // Hook to XNA
            XNAResources.graphics.PreparingDeviceSettings += new EventHandler<Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            Microsoft.Xna.Framework.Input.Mouse.WindowHandle = pictureBox.FindForm().Handle;
            _itemView.getController().resizeGraphicsDevice(texture.Width, texture.Height);

            // Resize picturebox and form
            int widthDelta = texture.Width - pictureBox.Width;
            int heightDelta = texture.Height - pictureBox.Height;
            pictureBox.Width = texture.Width;
            pictureBox.Height = texture.Height;
            Width = Width + widthDelta;
            Height = Height + heightDelta;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            _spriteBatch.Draw(_texture, _texture.Bounds, Microsoft.Xna.Framework.Color.White);
        }

        // updateMousePosition
        public void updateMousePosition()
        {
            // View offset
            System.Drawing.Point viewOffset = FindForm().PointToClient(PointToScreen(pictureBox.Location));

            // Set mouse boundaries
            int x = Math.Min(Math.Max(0, Input.newMouse.X - viewOffset.X), pictureBox.Width);
            int y = Math.Min(Math.Max(0, Input.newMouse.Y - viewOffset.Y), pictureBox.Height);

            // Calculate change in mouse position (for screen and world coordinates)
            int deltaX = x - _mouse.X;
            int deltaY = y - _mouse.Y;

            // Store screen space mouse coordinates
            _mouse.X = x;
            _mouse.Y = y;
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = pictureBox.Handle;
        }

        // Cancel button clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        // Picture box clicked
        private void pictureBox_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_mouse.ToString());
        }

        // Unhook from XNA
        private void EditBlueprintScrapShape_FormClosing(object sender, FormClosingEventArgs e)
        {
            XNAResources.graphics.PreparingDeviceSettings -= new EventHandler<Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
        }
    }
}
