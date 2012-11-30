using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public class LevelView : PictureBox, ILevelView
    {
        private EditorController _controller;
        private IContainerControl _propertyContainer;

        public LevelView()
            : base()
        {
            // Control properties
            Dock = DockStyle.Fill;
            BackColor = System.Drawing.Color.Black;

            // Add a listener to get the location of the mouse over the surface
            MouseMove += new System.Windows.Forms.MouseEventHandler(surface_MouseMove);
            MouseEnter += new EventHandler(surface_MouseEnter);
            MouseLeave += new EventHandler(surface_MouseLeave);

            // Resize graphics device when the surface is resized
            Resize += new EventHandler(surface_Resize);

            // Hook to XNA
            XNAResources.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            // Unhook from XNA
            XNAResources.graphics.PreparingDeviceSettings -= new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            base.Dispose(disposing);
        }

        // Surface resize event handler
        void surface_Resize(object sender, EventArgs e)
        {
            _controller.resizeGraphicsDevice(Width, Height);
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = Handle;
        }

        // setController
        public void setController(EditorController controller)
        {
            _controller = controller;
        }

        // getWidth
        public int getWidth()
        {
            return Width;
        }

        // getHeight
        public int getHeight()
        {
            return Height;
        }

        // Surface mouse move event
        void surface_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _controller.mouseMove(e);
        }

        // Mouse left the surface
        void surface_MouseLeave(object sender, EventArgs e)
        {
            _controller.mouseLeave();
        }

        // Mouse entered the surface
        void surface_MouseEnter(object sender, EventArgs e)
        {
            _controller.mouseEnter();
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            // Draw grid
            drawGrid();
        }

        // drawGrid
        private void drawGrid()
        {
            // Draw grid
            int screenWidth = Width;
            int screenHeight = Height;
            Rectangle destRect = new Rectangle(0, 0, (int)(screenWidth + _controller.scale), (int)(screenHeight + _controller.scale));
            Vector2 gridOffset = new Vector2((_controller.worldOffset.X * _controller.scale) % _controller.scale, (_controller.worldOffset.Y * _controller.scale) % _controller.scale);
            Color color = new Color(new Vector3(0.2f, 0.2f, 0.2f));

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += _controller.scale)
                XNAResources.spriteBatch.Draw(XNAResources.pixel, new Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += _controller.scale)
                XNAResources.spriteBatch.Draw(XNAResources.pixel, new Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);

            // Draw mouse position
            if (_controller.isMouseOverView)
            {
                XNAResources.spriteBatch.Draw(XNAResources.pixel, (_controller.worldMouse + _controller.worldOffset) * _controller.scale, new Rectangle(0, 0, 8, 8), Color.Yellow, 0, new Vector2(4, 4), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                //level.editor.main.spriteBatch.DrawString(Main.arial, String.Format("{0}, {1}", worldMouse.X, worldMouse.Y), (worldMouse + worldOffset) * scale, Color.Gray);
            }
        }
    }
}
