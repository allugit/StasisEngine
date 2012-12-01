using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public partial class MaterialView : Form, IMaterialView
    {
        private EditorController _controller;

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
            if (MessageBox.Show("Are you sure you want to close the Materials View? Any unsaved changes will be lost.", "Close Materials", MessageBoxButtons.OKCancel) == DialogResult.OK)
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
                    materialsListBox.DataSource = _controller.terrainMaterials;
                    break;

                case MaterialType.Trees:
                    materialsListBox.DataSource = _controller.treeMaterials;
                    break;

                case MaterialType.Fluid:
                    materialsListBox.DataSource = _controller.fluidMaterials;
                    break;

                case MaterialType.Items:
                    materialsListBox.DataSource = _controller.itemMaterials;
                    break;
            }
        }
    }
}
