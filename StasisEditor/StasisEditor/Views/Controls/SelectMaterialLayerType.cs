using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisCore.Resources;
namespace StasisEditor.Views.Controls
{
    public class SelectMaterialLayerType : Form
    {
        private ListBox layerTypeListBox;
        private Label label1;
        private Button addChildButton;
        private Button cancelButton;
    
        public SelectMaterialLayerType()
        {
            InitializeComponent();

            layerTypeListBox.DataSource = new string[] { "group", "texture", "noise", "uniform_scatter" };
        }

        private void InitializeComponent()
        {
            this.layerTypeListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.addChildButton = new System.Windows.Forms.Button();
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
            this.layerTypeListBox.Size = new System.Drawing.Size(217, 69);
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
            this.cancelButton.Location = new System.Drawing.Point(157, 114);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // addChildButton
            // 
            this.addChildButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addChildButton.Location = new System.Drawing.Point(76, 114);
            this.addChildButton.Name = "addChildButton";
            this.addChildButton.Size = new System.Drawing.Size(75, 23);
            this.addChildButton.TabIndex = 5;
            this.addChildButton.Text = "OK";
            this.addChildButton.UseVisualStyleBackColor = true;
            this.addChildButton.Click += new System.EventHandler(this.addChildButton_Click);
            // 
            // NewTerrainLayerForm
            // 
            this.ClientSize = new System.Drawing.Size(244, 149);
            this.Controls.Add(this.addChildButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.layerTypeListBox);
            this.Name = "NewTerrainLayerForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // getSelectedType
        public string getSelectedType()
        {
            return (string)layerTypeListBox.SelectedItem;
        }

        // Cancel button clicked
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        // Add child clicked
        private void addChildButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
