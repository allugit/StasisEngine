using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace StasisEditor
{
    public class Editor
    {
        public Main main;
        private EditorForm form;

        public Editor(Main main)
        {
            this.main = main;
            form = new EditorForm(this);
            form.Show();

            // Hide the main XNA game window
            main.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle(main.Window.Handle).VisibleChanged += new EventHandler(visibleChanged);
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
    }
}
