using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor
{
    public class Editor
    {
        public Main main;
        private EditorForm form;

        private float _scale = 35f;
        public float scale { get { return _scale; } }
        public Vector2 screenCenter;
        public Vector2 worldOffset { get { return screenCenter + (new Vector2(main.GraphicsDevice.Viewport.Width, main.GraphicsDevice.Viewport.Height) / 2) / scale; } }

        public Editor(Main main)
        {
            this.main = main;
            form = new EditorForm(this);
            form.Show();

            // Hide the main XNA game window
            main.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle(main.Window.Handle).VisibleChanged += new EventHandler(visibleChanged);

            // Resize graphics device when the surface is resized
            form.surface.Resize += new EventHandler(surface_Resize);

            // Do an initial resizing of the graphics device, so it matches the surface size
            resizeGraphicsDevice();
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
            if (form.surface.Width > 0 && form.surface.Height > 0)
            {
                main.graphics.PreferredBackBufferWidth = form.surface.Width;
                main.graphics.PreferredBackBufferHeight = form.surface.Height;
                main.graphics.ApplyChanges();
            }
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = form.surface.Handle;
        }

        // Keep the main XNA game window hidden
        private void visibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle(main.Window.Handle).Visible)
                System.Windows.Forms.Control.FromHandle(main.Window.Handle).Visible = false;
        }

        // loadContent
        public void loadContent()
        {
            form.loadContent();
        }

        // unloadContent
        public void unloadContent()
        {
            form.unloadContent();
        }

        // update
        public void update()
        {
        }

        // draw
        public void draw()
        {
            form.draw();
        }
    }
}
