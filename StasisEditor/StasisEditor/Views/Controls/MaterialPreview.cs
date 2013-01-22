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
    using Vector2 = Microsoft.Xna.Framework.Vector2;

    public partial class MaterialPreview : Form
    {
        private MaterialController _controller;
        private Material _material;
        private List<Vector2> _polygonPoints;

        public MaterialPreview(MaterialController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        // updateMaterial
        public void updateMaterial(Material material, List<Vector2> polygonPoints)
        {
            _material = material;
            _polygonPoints = polygonPoints;
            redrawMaterial();
            Text = "Previewing " + material.uid;
        }

        // Redraw material
        private void redrawMaterial()
        {
            if (useTestPolygon.Checked)
                _polygonPoints = _controller.testPolygonPoints;

            materialPreviewGraphics.setMaterial(_material, _polygonPoints, (float)growthFactorBox.Value);
        }

        private void MaterialPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            _controller.previewClosed();
        }

        // Growth factor changed
        private void growthFactorBox_ValueChanged(object sender, EventArgs e)
        {
            redrawMaterial();
        }

        // Use polygon points value changed
        private void usePolygonPoints_CheckedChanged(object sender, EventArgs e)
        {
            _polygonPoints = null;
            redrawMaterial();
        }

        // Scale value changed
        private void scaleBox_ValueChanged(object sender, EventArgs e)
        {
            materialPreviewGraphics.scale = (float)scaleBox.Value;
            redrawMaterial();
        }
    }
}
