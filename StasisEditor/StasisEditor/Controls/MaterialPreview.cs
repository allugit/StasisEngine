using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.Controls
{
    public partial class MaterialPreview : Form
    {
        private Image _image;

        public MaterialPreview(string title, Texture2D texture)
        {
            // Initialize components
            InitializeComponent();
            pictureBox.Width = texture.Width;
            pictureBox.Height = texture.Height;
            Text = title;

            // Create image from texture
            using (MemoryStream ms = new MemoryStream())
            {
                // Save to memory
                texture.SaveAsPng(ms, texture.Width, texture.Height);

                // Read from memory
                byte[] data = new byte[(int)ms.Length];
                ms.Read(data, 0, (int)ms.Length);

                // Create image
                _image = Image.FromStream(ms);
            }

            // Set picture box to image
            pictureBox.Image = _image;
        }
    }
}
