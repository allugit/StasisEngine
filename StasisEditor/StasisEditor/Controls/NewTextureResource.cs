using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StasisEditor.Controls
{
    public partial class NewTextureResource : Form
    {
        private bool _validTag;
        private bool _validCategory;
        private bool _validFile;
        private string _tag;
        private string _category;
        private string _fileName;

        public NewTextureResource()
        {
            InitializeComponent();
        }

        // Check validation
        private void checkValidation()
        {
            if (_validTag && _validCategory && _validFile)
                addButton.Enabled = true;
        }

        // Browse button clicked
        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "PNG Files | *.png";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                _validFile = true;
                _fileName = fd.FileName;
            }

            // Check validation
            checkValidation();
        }

        // Tag text changed
        private void tagTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: Check tag
            _validTag = true;

            // Check validation
            checkValidation();
        }

        // Category text changed
        private void categoryTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: check category
            _validCategory = true;

            // Check validation
            checkValidation();
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
