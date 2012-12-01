using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public partial class MaterialView : Form, IMaterialView
    {
        private EditorController _controller;
        private Material _selectedMaterial;
        private List<TerrainMaterial> _terrainMaterialsCopy;
        private List<TreeMaterial> _treeMaterialsCopy;
        private List<FluidMaterial> _fluidMaterialsCopy;
        private List<ItemMaterial> _itemMaterialsCopy;
        private bool _changesMade;

        public MaterialView()
        {
            InitializeComponent();

            // Set material types
            foreach (string materialType in Enum.GetNames(typeof(MaterialType)))
                materialTypesListBox.Items.Add(materialType);
        }

        // setController
        public void setController(EditorController controller)
        {
            _controller = controller;
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
                    _terrainMaterialsCopy = new List<TerrainMaterial>(_controller.terrainMaterials);
                    materialsListBox.DataSource = _terrainMaterialsCopy;
                    break;

                case MaterialType.Trees:
                    _treeMaterialsCopy = new List<TreeMaterial>(_controller.treeMaterials);
                    materialsListBox.DataSource = _treeMaterialsCopy;
                    break;

                case MaterialType.Fluid:
                    _fluidMaterialsCopy = new List<FluidMaterial>(_controller.fluidMaterials);
                    materialsListBox.DataSource = _fluidMaterialsCopy;
                    break;

                case MaterialType.Items:
                    _itemMaterialsCopy = new List<ItemMaterial>(_controller.itemMaterials);
                    materialsListBox.DataSource = _itemMaterialsCopy;
                    break;
            }
        }

        // Selected material changed
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changesMade = true;
            saveButton.Enabled = true;
            throw new NotImplementedException();
        }
    }
}
