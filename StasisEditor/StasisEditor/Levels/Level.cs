using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.Levels
{
    public class Level
    {
        public Editor editor;
        private List<Brush> brushes;
        public LevelSurface surface;
        public bool isMouseOverSurface;
        private System.Drawing.Point surfaceMouse;
        public Vector2 worldMouse { get { return new Vector2(surfaceMouse.X, surfaceMouse.Y) / editor.scale + editor.screenCenter; } }

        public Level(Editor editor)
        {
            this.editor = editor;
            brushes = new List<Brush>();
            surface = new LevelSurface(this);

            // Hook surface to XNA
            editor.main.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);

            // Resize graphics device when the surface is resized
            surface.Resize += new EventHandler(surface_Resize);

            // Do an initial resizing of the graphics device, so it matches the surface size
            resizeGraphicsDevice();

            // Add a listener to get the location of the mouse over the surface
            surface.MouseMove += new System.Windows.Forms.MouseEventHandler(surface_MouseMove);
            surface.MouseEnter += new EventHandler(surface_MouseEnter);
            surface.MouseLeave += new EventHandler(surface_MouseLeave);

            // Add surface to editor form
            editor.levelContainer.Controls.Add(surface);
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = surface.Handle;
        }

        // Mouse left the surface
        void surface_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverSurface = false;
        }

        // Mouse entered the surface
        void surface_MouseEnter(object sender, EventArgs e)
        {
            isMouseOverSurface = true;
        }

        // Surface mouse move event
        void surface_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            surfaceMouse.X = Math.Min(Math.Max(0, e.X), surface.Width);
            surfaceMouse.Y = Math.Min(Math.Max(0, e.Y), surface.Height);
        }

        // Surface resize event handler
        void surface_Resize(object sender, EventArgs e)
        {
            resizeGraphicsDevice();
        }

        // resizeGraphicsDevice
        private void resizeGraphicsDevice()
        {
            // Resize graphics device
            if (surface.Width > 0 && surface.Height > 0)
            {
                editor.main.graphics.PreferredBackBufferWidth = surface.Width;
                editor.main.graphics.PreferredBackBufferHeight = surface.Height;
                editor.main.graphics.ApplyChanges();
            }
        }

        // load
        public static Level load(XmlDocument document)
        {
            return null;
        }

        // addBrush
        public void addBrush(Brush brush)
        {
            brushes.Add(brush);
        }

        // removeBrush
        public void removeBrush(Brush brush)
        {
            brushes.Remove(brush);
        }

        // update
        public void update()
        {
        }

        // draw
        public void draw()
        {
            // Draw grid
            int screenWidth = editor.main.GraphicsDevice.Viewport.Width;
            int screenHeight = editor.main.GraphicsDevice.Viewport.Height;
            Rectangle destRect = new Rectangle(0, 0, (int)(screenWidth + editor.scale), (int)(screenHeight + editor.scale));
            Vector2 gridOffset = new Vector2((editor.worldOffset.X * editor.scale) % editor.scale, (editor.worldOffset.Y * editor.scale) % editor.scale);
            Color color = new Color(new Vector3(0.2f, 0.2f, 0.2f));

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += editor.scale)
                editor.main.spriteBatch.Draw(Main.pixel, new Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += editor.scale)
                editor.main.spriteBatch.Draw(Main.pixel, new Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);

            // Draw mouse position
            if (isMouseOverSurface)
                editor.main.spriteBatch.Draw(Main.pixel, worldMouse * editor.scale, new Rectangle(0, 0, 8, 8), Color.Yellow, 0, new Vector2(4, 4), 1f, SpriteEffects.None, 0);
        }
    }
}
