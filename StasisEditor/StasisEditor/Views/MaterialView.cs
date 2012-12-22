using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisCore.Resources;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public partial class MaterialView : UserControl
    {
        private MaterialController _controller;
        private MaterialProperties _materialProperties;

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

        // getSelectedMaterial
        public Material getSelectedMaterial()
        {
            return materialsListBox.SelectedItem as Material;
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
            // Update preview button
            previewButton.Enabled = materialsListBox.SelectedItem != null;
            
            // Open material properties
            closeProperties();
            openProperties(getSelectedMaterial());
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
            _controller.preview(getSelectedMaterial());
        }

        // Auto update changed
        private void autoUpdatePreview_CheckedChanged(object sender, EventArgs e)
        {
            _controller.setAutoUpdatePreview(autoUpdatePreview.Checked);
        }

        // Material save button
        private void saveButton_Click(object sender, EventArgs e)
        {
            //_controller.saveResource(getSelectedMaterial().resource);
        }

        // Add material clicked
        private void addButton_Click(object sender, EventArgs e)
        {
            CreateMaterialView createMaterialView = new CreateMaterialView();
            if (createMaterialView.ShowDialog() == DialogResult.OK)
            {
                _controller.createMaterial(createMaterialView.uid);
                materialsListBox.RefreshItems();
            }
        }
    }
}
