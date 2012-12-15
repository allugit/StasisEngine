using System;
using System.IO;
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
    public partial class EditBlueprintScrapShape : Form
    {
        private BlueprintScrapItemResource _scrapResource;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Texture2D _texture;
        private ItemView _itemView;
        private Vector2 _mouse;
        private List<Vector2> _points;

        public EditBlueprintScrapShape(ItemView itemView, Texture2D texture, BlueprintScrapItemResource scrapResource)
        {
            _itemView = itemView;
            _texture = texture;
            _scrapResource = scrapResource;
            _spriteBatch = XNAResources.spriteBatch;
            _pixel = XNAResources.pixel;
            _points = new List<Vector2>(_scrapResource.points);

            InitializeComponent();

            // Hook to XNA
            XNAResources.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
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

        // getPoints
        public List<Vector2> getPoints()
        {
            return _points;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            // Draw texture
            _spriteBatch.Draw(_texture, _texture.Bounds, Color.White);

            // Draw mouse position
            _spriteBatch.Draw(_pixel, new Vector2(_mouse.X, _mouse.Y), new Rectangle(0, 0, 4, 4), Color.Yellow, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);

            // Draw lines
            for (int i = 0; i < _points.Count; i++)
            {
                // Point
                _spriteBatch.Draw(_pixel, _points[i], new Rectangle(0, 0, 4, 4), Color.Orange, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);

                // Lines
                if (i > 0)
                {
                    drawLine(_points[i - 1],  _points[i], Color.Purple);
                }
            }

            if (_points.Count > 0)
            {
                // Draw line between last point and first point
                drawLine(_points[0], _points[_points.Count - 1], Color.DarkBlue);
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

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = pictureBox.Handle;
        }

        // Cancel button clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        // Action click
        public void click()
        {
            // Add point
            _points.Add(_mouse);

            // Enable done button
            saveButton.Enabled = _points.Count > 2;
        }

        // Unhook from XNA
        private void EditBlueprintScrapShape_FormClosing(object sender, FormClosingEventArgs e)
        {
            XNAResources.graphics.PreparingDeviceSettings -= new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
        }

        // Click on form
        private void EditBlueprintScrapShape_Click(object sender, EventArgs e)
        {
            click();
        }

        // Click on picture box
        private void pictureBox_Click(object sender, EventArgs e)
        {
            click();
        }

        // Done clicked
        private void doneButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        // Clear button clicked
        private void clearButton_Click(object sender, EventArgs e)
        {
            _points.Clear();
        }
    }
}
