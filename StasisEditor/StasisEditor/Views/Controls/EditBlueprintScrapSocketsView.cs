using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public partial class EditBlueprintScrapSocketsView : Form
    {
        private List<BlueprintScrapItemResource> _scraps;

        public EditBlueprintScrapSocketsView(List<BlueprintScrapItemResource> scraps)
        {
            _scraps = scraps;

            InitializeComponent();
        }

        // Cancel clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
