using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.Levels
{
    public class Level
    {
        public Editor editor;
        private List<Brush> brushes;
        public LevelSurface surface;
        public bool isMouseOverSurface;


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

            // Add surface to editor form
            editor.levelContainer.Controls.Add(surface);
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = surface.Handle;
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
            surface.draw();
        }
    }
}
