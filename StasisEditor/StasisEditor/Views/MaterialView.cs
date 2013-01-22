using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using StasisCore.Resources;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public partial class MaterialView : UserControl
    {
        private MaterialController _controller;
        private MaterialProperties _materialProperties;

        public EditorMaterial selectedMaterial { get { return materialsListBox.SelectedItem as EditorMaterial; } }
        public float growthFactor
        {
            get
            {
                Debug.Assert(_materialProperties != null);
                return _materialProperties.growthFactor;
            }
        }

        public MaterialView()
        {
            InitializeComponent();
        }

        // getController
        public MaterialController getController()
        {
            return _controller;
        }

        // setController
        public void setController(MaterialController controller)
        {
            _controller = controller;
            materialsListBox.DataSource = controller.materials;
        }

        // setAutoUpdatePreview -- this will trigger an event
        public void setAutoUpdatePreview(bool status)
        {
            autoUpdatePreview.Checked = status;
        }

        // openProperties
        private void openProperties(Material material)
        {
            _materialProperties = new MaterialProperties(this, material);
            propertiesContainer.Controls.Add(_materialProperties);

            // Set material property grid's selected objects
            _materialProperties.PropertyGrid.SelectedObject = material;
        }

        // closeProperties
        private void closeProperties()
        {
            if (_materialProperties == null)
                return;

            propertiesContainer.Controls.Remove(_materialProperties);
            _materialProperties.Dispose();
            _materialProperties = null;
        }

        // Refresh material list
        public void refreshMaterialList()
        {
            materialsListBox.RefreshItems();
        }

        // Selected materials changed
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enabled/disable preview button
            previewButton.Enabled = selectedMaterial != null;

            // Enable/disable remove button
            removeButton.Enabled = selectedMaterial != null;

            // Enable/disable clone button
            cloneButton.Enabled = selectedMaterial != null;
            
            // Close current properties
            closeProperties();

            // Open material properties
            if (selectedMaterial != null)
            openProperties(selectedMaterial);
        }

        // Material property changed
        private void materialProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Refresh the materials list
            (materialsListBox as RefreshingListBox).RefreshItems();
        }

        // Preview material
        private void previewButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(_materialProperties != null);
            _controller.preview(selectedMaterial, _materialProperties.growthFactor);
        }

        // Auto update changed
        private void autoUpdatePreview_CheckedChanged(object sender, EventArgs e)
        {
            _controller.setAutoUpdatePreview(autoUpdatePreview.Checked);
        }

        // Material save button
        private void saveButton_Click(object sender, EventArgs e)
        {
            _controller.saveMaterials();
        }

        // Add material clicked
        private void addButton_Click(object sender, EventArgs e)
        {
            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                _controller.createMaterial(createResourceView.uid);
                materialsListBox.RefreshItems();
            }
        }

        // Remove material clicked
        private void removeButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(selectedMaterial != null);

            try
            {
                _controller.removeMaterial(selectedMaterial.uid, true);
                propertiesContainer.Controls.Clear();
                removeButton.Enabled = false;
                materialsListBox.RefreshItems();
            }
            catch (InvalidResourceException exception)
            {
                MessageBox.Show(exception.Message, "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ResourceNotFoundException exception)
            {
                MessageBox.Show(exception.Message, "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clone button clicked
        private void cloneButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(selectedMaterial != null);

            _controller.cloneMaterial(selectedMaterial);
            materialsListBox.RefreshItems();
        }
    }
}
