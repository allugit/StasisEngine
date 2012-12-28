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
using Microsoft.Xna.Framework.Content;
using StasisEditor.Controllers;
using StasisCore;
using StasisCore.Models;
using StasisCore.Resources;

namespace StasisEditor.Views.Controls
{
    public partial class MaterialPreview : Form
    {
        private MaterialController _controller;

        public MaterialPreview(MaterialController controller, Material material)
        {
            _controller = controller;

            // Initialize components
            InitializeComponent();
        }

        // updateMaterial
        public void updateMaterial(Material material)
        {
            materialPreviewGraphics.setMaterial(material);
            Text = "Previewing " + material.uid;
        }

        private void MaterialPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            _controller.previewClosed();
        }
    }
}
