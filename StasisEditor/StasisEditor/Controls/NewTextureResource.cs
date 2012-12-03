using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using StasisEditor.Models;

namespace StasisEditor.Controls
{
    public partial class NewTextureResource : Form
    {
        private List<TemporaryTextureResource> _newTextureResources;

        public NewTextureResource()
        {
            _newTextureResources = new List<TemporaryTextureResource>();
            
            InitializeComponent();
        }
        // Browse button clicked
        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files (*.bmp;*.png;*.jpg;*.jpeg;*.tga;*.dds;*.dib)|*.bmp;*.png;*.jpg;*.jpeg;*.tga;*.dds;*.dib";
            fd.Multiselect = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in fd.FileNames)
                {
                    // Make sure only unique files get added
                    bool skip = false;
                    foreach (TemporaryTextureResource tempResource in _newTextureResources)
                    {
                        if (tempResource.fileName == fileName)
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (skip)
                        continue;

                    _newTextureResources.Add(new TemporaryTextureResource(fileName));
                }

                SuspendLayout();
                newTextureResourcesGrid.DataSource = null;
                newTextureResourcesGrid.DataSource = _newTextureResources;
                ResumeLayout();
            }
        }

        // Cancel clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        // Add clicked
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
