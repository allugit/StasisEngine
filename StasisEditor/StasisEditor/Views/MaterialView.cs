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
        private List<Material>[] _materialCopies;
        private List<Material> _selectedMaterials;
        private bool _changesMade;

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
        public void setController(EditorController controller)
        {
            _controller = controller;

            // Set material copies
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            for (int i = 0; i < numMaterialTypes; i++)
                _materialCopies[i] = Material.copyFrom(_controller.getMaterials((MaterialType)i));
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
            materialsListBox.DataSource = _materialCopies[(int)type];
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
