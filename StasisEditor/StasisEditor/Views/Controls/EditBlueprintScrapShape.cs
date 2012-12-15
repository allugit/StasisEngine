using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapShape : Form
    {
        private BlueprintScrapItemResource _scrapResource;
        private Image _image;
        private Point _mousePosition;

        public EditBlueprintScrapShape(Texture2D texture, BlueprintScrapItemResource scrapResource)
        {
            _scrapResource = scrapResource;

            InitializeComponent();
        }

        // Cancel button clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        // Picture box clicked
        private void pictureBox_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_mousePosition.ToString());
        }

        // Picture box mouse moved
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePosition = e.Location;
        }
    }
}
