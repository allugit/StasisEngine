using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Levels;

namespace StasisEditor
{
    public class Editor
    {
        public Main main;
        private EditorForm form;

        private KeyboardState newKey;
        private KeyboardState oldKey;
        private MouseState newMouse;
        private MouseState oldMouse;
        private int newScrollValue;
        private int oldScrollValue;

        private float _scale = 35f;
        public float scale { get { return _scale; } }
        public Vector2 screenCenter;
        public Vector2 worldOffset { get { return screenCenter + (new Vector2(main.GraphicsDevice.Viewport.Width, main.GraphicsDevice.Viewport.Height) / 2) / scale; } }

        public Level level;
        public System.Windows.Forms.SplitterPanel levelContainer { get { return form.levelSurfaceContainer; } }

        public Editor(Main main)
        {
            this.main = main;
            form = new EditorForm(this);
            form.Show();

            // Hide the main XNA game window
            System.Windows.Forms.Control.FromHandle(main.Window.Handle).VisibleChanged += new EventHandler(visibleChanged);
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
        }

        // unloadContent
        public void unloadContent()
        {
        }

        // createLevel
        public void createLevel()
        {
            level = new Level(this);
            form.enableNewLevel(false);
        }

        // update
        public void update()
        {
            // Update input
            newKey = Keyboard.GetState();
            newMouse = Mouse.GetState();
            newScrollValue = newMouse.ScrollWheelValue;

            if (level != null)
                level.update();

            // Store input
            oldKey = newKey;
            oldMouse = newMouse;
            oldScrollValue = newScrollValue;
        }

        // draw
        public void draw()
        {
            if (level != null)
                level.draw();
        }
    }
}
