using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public partial class MaterialView : Form, IMaterialView
    {
        private EditorController _controller;
        private List<Material> _terrainMaterialsCopy;
        private List<Material> _treeMaterialsCopy;
        private List<Material> _fluidMaterialsCopy;
        private List<Material> _itemMaterialsCopy;
        private List<Material> _selectedMaterials;
        private bool _changesMade;

        public MaterialView()
        {
            _selectedMaterials = new List<Material>();

            InitializeComponent();

            // Set material types
            foreach (string materialType in Enum.GetNames(typeof(MaterialType)))
                materialTypesListBox.Items.Add(materialType);
        }

        // setController
        public void setController(EditorController controller)
        {
            _controller = controller;

            // Set material copies
            _terrainMaterialsCopy = Material.copyFrom(_controller.terrainMaterials);
            _treeMaterialsCopy = Material.copyFrom(_controller.treeMaterials);
            _fluidMaterialsCopy = Material.copyFrom(_controller.fluidMaterials);
            _itemMaterialsCopy = Material.copyFrom(_controller.itemMaterials);
        }

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
            switch (type)
            {
                case MaterialType.Terrain:
                    materialsListBox.DataSource = _terrainMaterialsCopy;
                    break;

                case MaterialType.Trees:
                    materialsListBox.DataSource = _treeMaterialsCopy;
                    break;

                case MaterialType.Fluid:
                    materialsListBox.DataSource = _fluidMaterialsCopy;
                    break;

                case MaterialType.Items:
                    materialsListBox.DataSource = _itemMaterialsCopy;
                    break;
            }
        }

        // Selected materials changed
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedMaterials.Clear();
            foreach (Material selectedMaterial in materialsListBox.SelectedItems)
                _selectedMaterials.Add(selectedMaterial);

            materialProperties.SelectedObjects = _selectedMaterials.ToArray();
        }

        // Material property changed
        private void materialProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _changesMade = true;
            saveButton.Enabled = true;
            (materialsListBox as RefreshingListBox).RefreshItems();
        }
    }
}
