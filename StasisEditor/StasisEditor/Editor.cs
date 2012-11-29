using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor
{
    public class Editor
    {
        public Main main;
        private EditorForm form;

        private KeyboardState newKey;
        private KeyboardState oldKey;
        private MouseState newMouse;
        private System.Drawing.Point surfaceMouse;
        private MouseState oldMouse;
        private int newScrollValue;
        private int oldScrollValue;
        public bool isMouseOverSurface = false;

        private float _scale = 35f;
        public float scale { get { return _scale; } }
        public Vector2 screenCenter;
        public Vector2 worldOffset { get { return screenCenter + (new Vector2(main.GraphicsDevice.Viewport.Width, main.GraphicsDevice.Viewport.Height) / 2) / scale; } }
        public Vector2 worldMouse { get { return new Vector2(surfaceMouse.X, surfaceMouse.Y) / scale + screenCenter; } }

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

            // Add a listener to get the location of the mouse over the surface
            form.surface.MouseMove += new System.Windows.Forms.MouseEventHandler(surface_MouseMove);
            form.surface.MouseEnter += new EventHandler(surface_MouseEnter);
            form.surface.MouseLeave += new EventHandler(surface_MouseLeave);
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
            surfaceMouse.X = Math.Min(Math.Max(0, e.X), form.surface.Width);
            surfaceMouse.Y = Math.Min(Math.Max(0, e.Y), form.surface.Height);
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
            // Update input
            newKey = Keyboard.GetState();
            newMouse = Mouse.GetState();
            newScrollValue = newMouse.ScrollWheelValue;

            // Store input
            oldKey = newKey;
            oldMouse = newMouse;
            oldScrollValue = newScrollValue;
        }

        // draw
        public void draw()
        {
            form.draw();
        }
    }
}
