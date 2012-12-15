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
using StasisCore.Models;
using StasisEditor.Controllers;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapSocketsView : Form
    {
        private ItemView _itemView;
        private ItemController _itemController;
        private List<BlueprintScrapItemResource> _scraps;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Vector2 _mouse;

        public EditBlueprintScrapSocketsView(ItemView itemView, List<BlueprintScrapItemResource> scraps)
        {
            _itemView = itemView;
            _itemController = itemView.getController();
            _scraps = scraps;
            _spriteBatch = XNAResources.spriteBatch;
            _pixel = XNAResources.pixel;

            InitializeComponent();

            // Hook to XNA
            XNAResources.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            Microsoft.Xna.Framework.Input.Mouse.WindowHandle = pictureBox.FindForm().Handle;

            Resize += new EventHandler(EditBlueprintScrapSocketsView_Resize);

            // Initial resize
            _itemController.resizeGraphicsDevice(pictureBox.Width, pictureBox.Height);
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
            int deltaX = x - (int)_mouse.X;
            int deltaY = y - (int)_mouse.Y;

            // Store screen space mouse coordinates
            _mouse.X = x;
            _mouse.Y = y;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            // Draw mouse position
            _spriteBatch.Draw(_pixel, new Vector2(_mouse.X, _mouse.Y), new Rectangle(0, 0, 4, 4), Color.Yellow, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = pictureBox.Handle;
        }

        // Form resized
        void EditBlueprintScrapSocketsView_Resize(object sender, EventArgs e)
        {
            _itemController.resizeGraphicsDevice(pictureBox.Width, pictureBox.Height);
        }

        // Cancel clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        // Unhook from XNA
        private void EditBlueprintScrapSocketsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            XNAResources.graphics.PreparingDeviceSettings -= new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            Resize -= new EventHandler(EditBlueprintScrapSocketsView_Resize);
        }
    }
}
