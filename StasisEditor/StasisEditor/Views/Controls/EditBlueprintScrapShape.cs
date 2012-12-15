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
        private Point _mousePosition;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private ItemView _itemView;

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
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            _spriteBatch.Draw(_texture, _texture.Bounds, Microsoft.Xna.Framework.Color.White);
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
            MessageBox.Show(_mousePosition.ToString());
        }

        // Picture box mouse moved
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePosition = e.Location;
        }
    }
}
