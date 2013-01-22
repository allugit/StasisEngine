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
        private Material _material;

        public MaterialPreview(MaterialController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        // updateMaterial
        public void updateMaterial(Material material, List<Microsoft.Xna.Framework.Vector2> polygonPoints)
        {
            _material = material;
            materialPreviewGraphics.setMaterial(material, polygonPoints, (float)growthFactorBox.Value);
            Text = "Previewing " + material.uid;
        }

        private void MaterialPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            _controller.previewClosed();
        }

        // Growth factor changed
        private void growthFactorBox_ValueChanged(object sender, EventArgs e)
        {
            materialPreviewGraphics.setMaterial(_material, usePolygonPoints.Checked ? _controller.testPolygonPoints : null, (float)growthFactorBox.Value);
        }

        // Use polygon points value changed
        private void usePolygonPoints_CheckedChanged(object sender, EventArgs e)
        {
            materialPreviewGraphics.setMaterial(_material, usePolygonPoints.Checked ? _controller.testPolygonPoints : null, (float)growthFactorBox.Value);
        }
    }
}
