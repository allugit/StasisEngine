using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapShape : Form
    {
        private BlueprintView _view;
        private BlueprintScrap _scrap;
        private List<Vector2> _points;

        public List<Vector2> points { get { return _points; } }

        public EditBlueprintScrapShape(BlueprintView view, BlueprintScrap scrap)
        {
            _view = view;
            //_textureCenter = new Vector2(_texture.Width, _texture.Height) / 2;
            _scrap = scrap;
            _points = new List<Vector2>(_scrap.points);

            InitializeComponent();

            // Set scrap texture
            editBlueprintScrapShapeGraphics.setTexture(scrap.scrapTextureUID);
        }

        // getPoints
        public List<Vector2> getPoints()
        {
            return _points;
        }

        // Enable save button
        public void enableSaveButton(bool status)
        {
            saveButton.Enabled = status;
        }

        // Cancel button clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        // Click on form
        private void EditBlueprintScrapShape_Click(object sender, EventArgs e)
        {
            editBlueprintScrapShapeGraphics.click();
        }

        // Done clicked
        private void doneButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        // Clear button clicked
        private void clearButton_Click(object sender, EventArgs e)
        {
            _points.Clear();
        }
    }
}
