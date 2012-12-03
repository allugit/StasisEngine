using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Controls;

namespace StasisEditor.Views
{
    public partial class TextureView : Form, ITextureView
    {
        private ITextureController _controller;
        private BindingList<TextureResource> _textureResources;
        private BindingSource _textureBindingSource;

        public TextureView()
        {
            _textureResources = new BindingList<TextureResource>();
            _textureBindingSource = new BindingSource();

            InitializeComponent();

            textureDataGrid.DataSource = _textureBindingSource;
        }

        // setController
        public void setController(ITextureController controller)
        {
            _controller = controller;
        }

        // refreshGrid
        public void refreshGrid()
        {
            _textureResources.Clear();
            _textureResources = new BindingList<TextureResource>(TextureResource.copyFrom(_controller.getTextureResources()));
            _textureBindingSource.DataSource = _textureResources;
        }

        // Form closed
        private void TextureView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.viewClosed();
        }

        // Add texture resources
        private void addTextureButton_Click(object sender, EventArgs e)
        {
            NewTextureResource newResources = new NewTextureResource();
            if (newResources.ShowDialog() == DialogResult.OK)
                _controller.createNewTextureResources(newResources.newTextureResources);
        }
    }
}
