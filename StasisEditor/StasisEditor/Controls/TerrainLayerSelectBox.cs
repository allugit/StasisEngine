using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisCore.Models;
namespace StasisEditor.Controls
{
    public class TerrainLayerSelectBox : Form
    {
        private ListBox layerTypeListBox;
        private Label label1;
        private Button cancelButton;
        private Button okayButton;
    
        public TerrainLayerSelectBox()
        {
            InitializeComponent();

            List<TerrainLayerType> types = new List<TerrainLayerType>();
            foreach (TerrainLayerType type in Enum.GetValues(typeof(TerrainLayerType)))
                types.Add(type);

            layerTypeListBox.DataSource = types;
        }

        private void InitializeComponent()
        {
            this.layerTypeListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // layerTypeListBox
            // 
            this.layerTypeListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.layerTypeListBox.FormattingEnabled = true;
            this.layerTypeListBox.Location = new System.Drawing.Point(14, 26);
            this.layerTypeListBox.Name = "layerTypeListBox";
            this.layerTypeListBox.Size = new System.Drawing.Size(183, 69);
            this.layerTypeListBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Layer Type";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(123, 107);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okayButton
            // 
            this.okayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okayButton.Location = new System.Drawing.Point(42, 107);
            this.okayButton.Name = "okayButton";
            this.okayButton.Size = new System.Drawing.Size(75, 23);
            this.okayButton.TabIndex = 3;
            this.okayButton.Text = "OK";
            this.okayButton.UseVisualStyleBackColor = true;
            this.okayButton.Click += new System.EventHandler(this.okayButton_Click);
            // 
            // TerrainLayerSelectBox
            // 
            this.ClientSize = new System.Drawing.Size(210, 142);
            this.ControlBox = false;
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.layerTypeListBox);
            this.Name = "TerrainLayerSelectBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // getSelectedType
        public TerrainLayerType getSelectedType()
        {
            return (TerrainLayerType)layerTypeListBox.SelectedItem;
        }

        // Cancel button clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        // OK button clicked
        private void okayButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
