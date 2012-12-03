using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using StasisEditor.Models;

namespace StasisEditor.Controls
{
    public partial class NewTextureResource : Form
    {
        private BindingList<TemporaryTextureResource> _newTextureResources;
        private BindingSource _bindingSource;
        private DataGridViewButtonColumn _buttonColumn;

        public NewTextureResource()
        {
            _newTextureResources = new BindingList<TemporaryTextureResource>();
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = _newTextureResources;

            InitializeComponent();
            newTextureResourcesGrid.DataSource = _bindingSource;

            // Add button column
            _buttonColumn = new DataGridViewButtonColumn();
            _buttonColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            _buttonColumn.HeaderText = "";
            _buttonColumn.Text = "Remove";
            _buttonColumn.Width = 80;
            _buttonColumn.FlatStyle = FlatStyle.Standard;
            _buttonColumn.UseColumnTextForButtonValue = true;
            newTextureResourcesGrid.Columns.Add(_buttonColumn);
            newTextureResourcesGrid.CellContentClick += new DataGridViewCellEventHandler(newTextureResourcesGrid_CellClick);
        }

        // Remove button handler
        void newTextureResourcesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1 && e.RowIndex < _newTextureResources.Count)
                _newTextureResources.RemoveAt(e.RowIndex);
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
                        if (tempResource.filePath == fileName)
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (skip)
                        continue;

                    _newTextureResources.Add(new TemporaryTextureResource(fileName));
                }
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
                newTextureResourcesGrid.Rows[e.RowIndex].ErrorText = String.Format("Invalid characters: {0}", new string(foundInvalidChars.ToArray()));
                e.Cancel = true;
            }

            // Validate uniqueness
            if (value.Length > 0)   // skip empty cells
            {
                // Only validate the uniqueness of tags
                if (newTextureResourcesGrid.Columns[e.ColumnIndex].Name == "tag")
                {
                    foreach (DataGridViewRow row in newTextureResourcesGrid.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            // Skip self and button cells
                            if ((cell.ColumnIndex == e.ColumnIndex && cell.RowIndex == e.RowIndex) ||
                                cell.FormattedValue.ToString() == "Remove")
                                continue;

                            if (cell.FormattedValue.ToString() == value)
                            {
                                newTextureResourcesGrid.Rows[e.RowIndex].ErrorText = String.Format("Must have unique values: {0}", value);
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
            newTextureResourcesGrid.Rows[e.RowIndex].ErrorText = String.Empty;

            // Validate the entire form
            bool valid = true;
            foreach (DataGridViewRow row in newTextureResourcesGrid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                    break;
            }
            createButton.Enabled = valid;
        }
    }
}
