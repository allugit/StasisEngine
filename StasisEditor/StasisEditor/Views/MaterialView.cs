using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public partial class MaterialView : Form, IMaterialView
    {
        private MaterialController _controller;
        private List<Material>[] _materialCopies;
        private List<Material> _selectedMaterials;
        private bool _changesMade;
        private MaterialProperties _properties;

        public MaterialView()
        {
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            _materialCopies = new List<Material>[numMaterialTypes];

            _selectedMaterials = new List<Material>();

            InitializeComponent();

            // Set material types
            foreach (string materialType in Enum.GetNames(typeof(MaterialType)))
                materialTypesListBox.Items.Add(materialType);
        }

        // setController
        public void setController(MaterialController controller)
        {
            _controller = controller;

            // Set material copies
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            for (int i = 0; i < numMaterialTypes; i++)
                _materialCopies[i] = Material.copyFrom(_controller.getMaterials((MaterialType)i));
        }

        // openProperties
        private void openProperties(MaterialType type)
        {
            // Common
            _properties = new MaterialProperties();
            propertiesContainer.Controls.Add(_properties);

            // Material specific
            switch (type)
            {
                case MaterialType.Terrain:
                    break;
            }

            // Set material property grid's selected objects
            _properties.PropertyGrid.SelectedObjects = _selectedMaterials.ToArray();
        }

        // closeProperties
        private void closeProperties()
        {
            if (_properties == null)
                return;

            propertiesContainer.Controls.Remove(_properties);
            _properties.Dispose();
            _properties = null;
        }

        // Close button clicked
        private void closeButton_Click(object sender, EventArgs e)
        {
            if (_changesMade)
            {
                if (MessageBox.Show("Are you sure you want to close the Materials View? Any unsaved changes will be lost.", "Close Materials", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    Close();
            }
            else
                Close();
        }

        // Material type selection changed
        private void materialTypesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBox listBox = (sender as ListBox);
            MaterialType type = (MaterialType)Enum.Parse(typeof(MaterialType), listBox.SelectedItem as string);
            materialsListBox.DataSource = _materialCopies[(int)type];
        }

        // Selected materials changed
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear selected materials
            _selectedMaterials.Clear();

            // Construct new selected materials list from materialListBox's selected items
            foreach (Material selectedMaterial in materialsListBox.SelectedItems)
                _selectedMaterials.Add(selectedMaterial);

            // Update preview button
            previewButton.Enabled = _selectedMaterials.Count == 1;
            
            // Open material properties
            closeProperties();
            openProperties(((sender as ListBox).SelectedItems[0] as Material).type);
        }

        // Material property changed
        private void materialProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Changes were made
            _changesMade = true;
            saveButton.Enabled = true;

            // Refresh the materials list
            (materialsListBox as RefreshingListBox).RefreshItems();
        }

        // Preview material
        private void previewButton_Click(object sender, EventArgs e)
        {
            _controller.preview(_selectedMaterials[0]);
        }
    }
}
