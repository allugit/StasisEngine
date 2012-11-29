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

namespace StasisEditor
{
    public partial class EditorForm : Form
    {
        private Editor editor;
        private Texture2D pixel;

        // Constructor
        public EditorForm(Editor editor)
        {
            this.editor = editor;
            InitializeComponent();
        }

        // EditorForm closed event
        private void EditorForm_Closed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // loadContent
        public void loadContent()
        {
            pixel = new Texture2D(editor.main.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        // unloadContent
        public void unloadContent()
        {
            pixel.Dispose();
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

            editor.main.spriteBatch.Begin();

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += editor.scale)
                editor.main.spriteBatch.Draw(pixel, new Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += editor.scale)
                editor.main.spriteBatch.Draw(pixel, new Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);

            editor.main.spriteBatch.End();
        }
    }
}
