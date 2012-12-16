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
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapSocketsView : Form
    {
        private ItemView _itemView;
        private ItemController _itemController;
        private List<EditorBlueprintScrap> _editorScraps;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Vector2 _mouse;
        private EditorBlueprintScrap _selectedScrap;

        public EditBlueprintScrapSocketsView(ItemView itemView, List<BlueprintScrapItemResource> scraps)
        {
            _itemView = itemView;
            _itemController = itemView.getController();
            _spriteBatch = XNAResources.spriteBatch;
            _pixel = XNAResources.pixel;

            // Initialize editor scraps
            _editorScraps = new List<EditorBlueprintScrap>();
            for (int i = 0; i < scraps.Count; i++)
                _editorScraps.Add(new EditorBlueprintScrap(scraps[i]));

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

            // Move selected scrap
            if (_selectedScrap != null)
                _selectedScrap.position += new Vector2(deltaX, deltaY);

            // Store screen space mouse coordinates
            _mouse.X = x;
            _mouse.Y = y;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            // Draw scraps
            for (int i = 0; i < _editorScraps.Count; i++)
            {
                _spriteBatch.Draw(_editorScraps[i].texture, _editorScraps[i].position, Color.White);
            }

            // Draw mouse position
            _spriteBatch.Draw(_pixel, new Vector2(_mouse.X, _mouse.Y), new Rectangle(0, 0, 4, 4), Color.Yellow, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);
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

        // Picture box clicked
        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (_selectedScrap == null)
            {
                // Hit test scraps
                for (int i = 0; i < _editorScraps.Count; i++)
                {
                    if (_editorScraps[i].hitTest(_mouse))
                    {
                        _selectedScrap = _editorScraps[i];
                        return;
                    }
                }
            }
            else
            {
                // Place selected scrap
                _selectedScrap = null;
            }
        }
    }
}
