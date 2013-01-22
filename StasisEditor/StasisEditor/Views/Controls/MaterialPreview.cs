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
            ResizeEnd += new EventHandler(MaterialPreview_ResizeEnd);
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

            List<Vector2> transformedPolygonPoints = null;
            if (_polygonPoints != null)
            {
                // Transform points to screen space
                transformedPolygonPoints = new List<Vector2>();
                float scale = EditorController.ORIGINAL_SCALE;
                Vector2 topLeft = _polygonPoints[0] * scale;
                Vector2 bottomRight = _polygonPoints[0] * scale;
                for (int i = 0; i < _polygonPoints.Count; i++)
                {
                    transformedPolygonPoints.Add(_polygonPoints[i] * scale);
                    topLeft = Vector2.Min(topLeft, transformedPolygonPoints[i]);
                    bottomRight = Vector2.Max(bottomRight, transformedPolygonPoints[i]);
                }
                float width = bottomRight.X - topLeft.X;
                float height = bottomRight.Y - topLeft.Y;
                for (int i = 0; i < transformedPolygonPoints.Count; i++)
                {
                    transformedPolygonPoints[i] += -topLeft;
                }
            }

            materialPreviewGraphics.setMaterial(_material, transformedPolygonPoints, (float)growthFactorBox.Value);
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

        // Done resizing
        void MaterialPreview_ResizeEnd(object sender, EventArgs e)
        {
            if (!useTestPolygon.Checked)
                redrawMaterial();
        }
    }
}
