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
using StasisEditor.Controllers;

namespace StasisEditor.Views.Controls
{
    public partial class MaterialPreview : Form
    {
        private IMaterialController _controller;
        private Image _image;

        public MaterialPreview(IMaterialController controller, Texture2D texture, string title = "Material Preview")
        {
            _controller = controller;

            // Initialize components
            InitializeComponent();
            pictureBox.Width = texture.Width;
            pictureBox.Height = texture.Height;
            Text = title;

            setPictureBox(texture);
        }

        // Update preview
        public void updatePreview(Texture2D result)
        {
            setPictureBox(result);
        }

        // Set picture box to texture
        private void setPictureBox(Texture2D texture)
        {
            // Dispose previous image
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }

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

        // Preview window closing
        private void MaterialPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            _controller.previewClosed();
        }
    }
}
