using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public partial class LevelView : UserControl, ILevelView
    {
        private ILevelController _controller;

        public LevelView()
        {
            InitializeComponent();
        }

        // setController
        public void setController(ILevelController controller)
        {
            _controller = controller;

            // Hook to XNA
            XNAResources.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);

            // Add a listener to get the location of the mouse over the surface
            surface.MouseMove += new System.Windows.Forms.MouseEventHandler(surface_MouseMove);
            surface.MouseEnter += new EventHandler(surface_MouseEnter);
            surface.MouseLeave += new EventHandler(surface_MouseLeave);

            // Resize graphics device when the surface is resized
            Resize += new EventHandler(surface_Resize);

            // Temporary -- Force resize
            _controller.resizeGraphicsDevice(surface.Width, surface.Height);
        }

        // Surface resize event handler
        void surface_Resize(object sender, EventArgs e)
        {
            _controller.resizeGraphicsDevice(surface.Width, surface.Height);
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = surface.Handle;
        }

        // getWidth
        public int getWidth()
        {
            return surface.Width;
        }

        // getHeight
        public int getHeight()
        {
            return surface.Height;
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
            int screenWidth = surface.Width;
            int screenHeight = surface.Height;
            float scale = _controller.getScale();
            Vector2 worldOffset = _controller.getWorldOffset();
            Vector2 worldMouse = _controller.getWorldMouse();
            Microsoft.Xna.Framework.Rectangle destRect = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)(screenWidth + scale), (int)(screenHeight + scale));
            Vector2 gridOffset = new Vector2((worldOffset.X * scale) % scale, (worldOffset.Y * scale) % scale);
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(new Vector3(0.2f, 0.2f, 0.2f));

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += scale)
                XNAResources.spriteBatch.Draw(XNAResources.pixel, new Microsoft.Xna.Framework.Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += scale)
                XNAResources.spriteBatch.Draw(XNAResources.pixel, new Microsoft.Xna.Framework.Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);

            // Draw mouse position
            if (_controller.getIsMouseOverView())
            {
                XNAResources.spriteBatch.Draw(
                    XNAResources.pixel,
                    (worldMouse + worldOffset) * scale,
                    new Microsoft.Xna.Framework.Rectangle(0, 0, 8, 8),
                    Microsoft.Xna.Framework.Color.Yellow, 0, new Vector2(4, 4),
                    1f,
                    Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                    0);
                //level.editor.main.spriteBatch.DrawString(Main.arial, String.Format("{0}, {1}", worldMouse.X, worldMouse.Y), (worldMouse + worldOffset) * scale, Color.Gray);
            }
        }
    }
}
