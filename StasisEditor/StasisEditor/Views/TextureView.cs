using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        // preview
        public void preview(List<TextureResource> resources)
        {
            // Clear previous preview images
            clearPreview();

            string textureDirectory = EditorController.TEXTURE_RESOURCE_DIRECTORY;
            foreach (TextureResource resource in resources)
            {
                // Load file
                string filePath = string.Format("{0}\\{1}", textureDirectory, resource.relativePath);
                Image image = null;
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    image = Image.FromStream(stream);
                }

                // Create picture box
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Transparent;
                pictureBox.Image = image;
                pictureBox.Size = image.Size;
                previewContainer.Controls.Add(pictureBox);
            }
        }

        // clearPreview
        public void clearPreview()
        {
            foreach (Control control in previewContainer.Controls)
            {
                if (control is PictureBox)
                {
                    //(control as PictureBox).Image.Dispose();
                    control.Dispose();
                }
            }

            previewContainer.Controls.Clear();
        }

        // getSelectedResources
        private List<TextureResource> getSelectedResources()
        {
            List<int> selectedRows = new List<int>();

            // Consider selected cells as selected rows
            foreach (DataGridViewCell cell in textureDataGrid.SelectedCells)
            {
                if (!selectedRows.Contains(cell.RowIndex))
                    selectedRows.Add(cell.RowIndex);
            }

            // Add selected rows
            foreach (DataGridViewRow row in textureDataGrid.SelectedRows)
            {
                if (!selectedRows.Contains(row.Index))
                    selectedRows.Add(row.Index);
            }

            // Convert rows to texture resources
            List<TextureResource> selectedResources = new List<TextureResource>(selectedRows.Count);
            foreach (int index in selectedRows)
                selectedResources.Add(textureDataGrid.Rows[index].DataBoundItem as TextureResource);

            return selectedResources;
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

        // Selection changed
        private void textureDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            // Preview selected texture resources
            preview(getSelectedResources());
        }

        // Remove button clicked
        private void removeTextureButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected textures from the hard drive?", "Delete textures", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                // Clear preview
                clearPreview();

                // Destroy selected resources
                _controller.removeTextureResource(getSelectedResources());
            }
        }
    }
}
