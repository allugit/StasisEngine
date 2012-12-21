using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using StasisEditor.Controllers;
using StasisEditor.Views;
using StasisCore.Resources;

namespace StasisEditor.Views
{
    public partial class TextureView : UserControl
    {
        private TextureController _controller;
        //private BindingList<TextureResource> _textureResources;
        private BindingSource _textureBindingSource;
        private DataGridViewButtonColumn _buttonColumn;

        public TextureView()
        {
            _textureBindingSource = new BindingSource();
            _textureBindingSource.DataSource = new BindingList<TextureResource>();

            InitializeComponent();

            textureDataGrid.DataSource = _textureBindingSource;

            // Add button column
            _buttonColumn = new DataGridViewButtonColumn();
            _buttonColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            _buttonColumn.HeaderText = "";
            _buttonColumn.Text = "Remove";
            _buttonColumn.Width = 80;
            _buttonColumn.FlatStyle = FlatStyle.Standard;
            _buttonColumn.UseColumnTextForButtonValue = true;
            textureDataGrid.Columns.Add(_buttonColumn);
        }

        // Browse button clicked
        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files (*.bmp;*.png;*.jpg;*.jpeg;*.tga;*.dds;*.dib)|*.bmp;*.png;*.jpg;*.jpeg;*.tga;*.dds;*.dib";
            fd.Multiselect = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                //_controller.addTextureResources(fd.FileNames);
            }
        }

        // setController
        public void setController(TextureController controller)
        {
            _controller = controller;
        }

        // clearPreview
        public void clearPreview()
        {
            foreach (Control control in previewContainer.Controls)
            {
                if (control is PictureBox)
                    control.Dispose();
            }

            previewContainer.Controls.Clear();
        }

        // Form closed
        private void NewTextureResource_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.viewClosed();
        }
    }
}
