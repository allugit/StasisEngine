using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Resources;

namespace StasisEditor.Views.Controls
{
    public partial class PlantSelectBox : Form
    {
        private PlantType _selectedPlantType;

        public PlantType selectedPlantType { get { return _selectedPlantType; } }

        public PlantSelectBox()
        {
            InitializeComponent();

            // Populate combo box
            foreach (PlantType plantType in Enum.GetValues(typeof(PlantType)))
                plantTypeBox.Items.Add(plantType);

            // Select first item
            plantTypeBox.SelectedItem = plantTypeBox.Items[0];
        }

        // Cancel clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        // OK clicked
        private void okayButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        // Selected value changed
        private void plantTypeBox_SelectedValueChanged(object sender, EventArgs e)
        {
            _selectedPlantType = (PlantType)plantTypeBox.SelectedItem;
            okayButton.Enabled = plantTypeBox.SelectedItem != null;
        }
    }
}
