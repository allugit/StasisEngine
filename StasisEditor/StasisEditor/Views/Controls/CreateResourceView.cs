using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore;

namespace StasisEditor.Views.Controls
{
    public partial class CreateResourceView : Form
    {
        public string uid { get { return uidBox.Text; } }

        public CreateResourceView()
        {
            InitializeComponent();
        }

        // Keydown
        private void uidBox_KeyDown(object sender, KeyEventArgs e)
        {
            createButton.Enabled = uidBox.Text.Length > 0;

            if (createButton.Enabled && e.KeyCode == Keys.Enter)
            {
                createButton_Click(sender, e);
            }
        }

        // Create clicked
        private void createButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(uidBox.Text.Length > 0);

            try
            {
                if (EditorResourceManager.exists(uidBox.Text))
                {
                    MessageBox.Show("That uid already exists.", "Duplicate UID");
                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
            }
            catch (InvalidResourceException resourceException)
            {
                MessageBox.Show(String.Format("Invalid resource:\n {0}", resourceException.StackTrace), "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ResourceNotFoundException resourceException)
            {
                MessageBox.Show(String.Format("Resource not found:\n {0}", resourceException.StackTrace), "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cancel clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
