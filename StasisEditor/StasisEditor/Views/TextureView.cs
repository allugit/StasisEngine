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
using StasisEditor.Models;
using StasisEditor.Views;
using StasisCore.Models;

namespace StasisEditor.Views
{
    public partial class TextureView : Form, ITextureView
    {
        private ITextureController _controller;
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
            textureDataGrid.CellContentClick += new DataGridViewCellEventHandler(removeTextureButton_Click);
        }

        // Browse button clicked
        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files (*.bmp;*.png;*.jpg;*.jpeg;*.tga;*.dds;*.dib)|*.bmp;*.png;*.jpg;*.jpeg;*.tga;*.dds;*.dib";
            fd.Multiselect = true;
            if (fd.ShowDialog() == DialogResult.OK)
                _controller.addTextureResources(fd.FileNames);
        }

        // setController
        public void setController(ITextureController controller)
        {
            _controller = controller;
            _textureBindingSource.DataSource = _controller.getTextureResources();
        }

        /*
        // refreshGrid
        public void refreshGrid()
        {
            _textureResources.Clear();
            _textureResources = new BindingList<TextureResource>(TextureResource.copyFrom(_controller.getTextureResources()));
            _textureBindingSource.DataSource = _textureResources;
        }*/

        // preview
        public void preview(List<TextureResource> resources)
        {
            // Clear previous preview images
            clearPreview();

            string textureDirectory = EditorController.TEXTURE_RESOURCE_DIRECTORY;
            foreach (TextureResource resource in resources)
            {
                // Load file, falling back to the file path it was loaded from if invalid
                string filePath = resource.isValid ? resource.getFullPath(textureDirectory) : resource.currentPath;

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

        // Selection changed
        private void textureDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            // Preview selected texture resources
            preview(getSelectedResources());
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

        // Validate the cell
        private void newTextureResourcesGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Validate characters
            string value = e.FormattedValue.ToString();
            List<char> invalidChars = new List<char>(Path.GetInvalidPathChars());
            invalidChars.AddRange(Path.GetInvalidFileNameChars());
            string invalidCharsString = String.Format("{0}", new string(invalidChars.ToArray()));
            List<char> foundInvalidChars = new List<char>();
            foreach (char c in value)
            {
                if (invalidChars.Contains(c))
                    foundInvalidChars.Add(c);
            }
            if (foundInvalidChars.Count > 0)
            {
                textureDataGrid.Rows[e.RowIndex].ErrorText = String.Format("Invalid characters: {0}", new string(foundInvalidChars.ToArray()));
                e.Cancel = true;
            }

            // Validate uniqueness
            if (value.Length > 0)   // skip empty cells
            {
                // Only validate the uniqueness of tags
                if (textureDataGrid.Columns[e.ColumnIndex].Name == "tag")
                {
                    foreach (DataGridViewRow row in textureDataGrid.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            // Skip self and button cells
                            if ((cell.ColumnIndex == e.ColumnIndex && cell.RowIndex == e.RowIndex) ||
                                cell.FormattedValue.ToString() == "Remove")
                                continue;

                            if (cell.FormattedValue.ToString() == value)
                            {
                                textureDataGrid.Rows[e.RowIndex].ErrorText = String.Format("Must have unique values: {0}", value);
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
        }

        // Done validating the cell
        private void newTextureResourcesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            textureDataGrid.Rows[e.RowIndex].ErrorText = String.Empty;

            // Check if a move is necessary
            TextureResource resource = textureDataGrid.Rows[e.RowIndex].DataBoundItem as TextureResource;
            if (resource.isValid && !File.Exists(resource.getFullPath(EditorController.TEXTURE_RESOURCE_DIRECTORY)))
                _controller.relocateTextureResource(resource);
        }

        // Remove button clicked
        private void removeTextureButton_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == _buttonColumn.Index)
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

        // Form closed
        private void NewTextureResource_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.viewClosed();
        }
    }
}
