using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace StasisEditor.Levels
{
    public class LevelSurface : PictureBox
    {
        public Level level;

        public bool isMouseOverSurface;
        public System.Drawing.Point mouse;
        private float _scale = 35f;
        public float scale { get { return _scale; } }
        public Vector2 screenCenter;
        public Vector2 worldOffset { get { return screenCenter + (new Vector2(level.editor.main.GraphicsDevice.Viewport.Width, level.editor.main.GraphicsDevice.Viewport.Height) / 2) / scale; } }
        public Vector2 worldMouse { get { return new Vector2(mouse.X, mouse.Y) / scale - worldOffset; } }

        public LevelSurface(Level level)
            : base()
        {
            this.level = level;
            Width = level.editor.levelContainer.Width;
            Height = level.editor.levelContainer.Height;
            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;

            // Add a listener to get the location of the mouse over the surface
            MouseMove += new System.Windows.Forms.MouseEventHandler(surface_MouseMove);
            MouseEnter += new EventHandler(surface_MouseEnter);
            MouseLeave += new EventHandler(surface_MouseLeave);
        }

        // Surface mouse move event
        void surface_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int x = Math.Min(Math.Max(0, e.X), Width);
            int y = Math.Min(Math.Max(0, e.Y), Height);

            bool ctrl = Input.newKey.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl);

            if (ctrl)
            {
                int deltaX = mouse.X - x;
                int deltaY = mouse.Y - y;

                screenCenter -= new Vector2(deltaX, deltaY) / scale;
            }

            mouse.X = x;
            mouse.Y = y;
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

        // draw
        public void draw()
        {
            // Draw grid
            int screenWidth = level.editor.main.GraphicsDevice.Viewport.Width;
            int screenHeight = level.editor.main.GraphicsDevice.Viewport.Height;
            Rectangle destRect = new Rectangle(0, 0, (int)(screenWidth + scale), (int)(screenHeight + scale));
            Vector2 gridOffset = new Vector2((worldOffset.X * scale) % scale, (worldOffset.Y * scale) % scale);
            Color color = new Color(new Vector3(0.2f, 0.2f, 0.2f));

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += scale)
                level.editor.main.spriteBatch.Draw(Main.pixel, new Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += scale)
                level.editor.main.spriteBatch.Draw(Main.pixel, new Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);

            // Draw mouse position
            if (isMouseOverSurface)
            {
                level.editor.main.spriteBatch.Draw(Main.pixel, (worldMouse + worldOffset) * scale, new Rectangle(0, 0, 8, 8), Color.Yellow, 0, new Vector2(4, 4), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                //level.editor.main.spriteBatch.DrawString(Main.arial, String.Format("{0}, {1}", worldMouse.X, worldMouse.Y), (worldMouse + worldOffset) * scale, Color.Gray);
            }
        }
    }
}
