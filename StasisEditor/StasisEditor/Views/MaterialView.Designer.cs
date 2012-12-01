using StasisEditor.Controls;
namespace StasisEditor.Views
{
    partial class MaterialView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.materialTypesLabel = new System.Windows.Forms.Label();
            this.materialTypesListBox = new System.Windows.Forms.ListBox();
            this.materialsLabel = new System.Windows.Forms.Label();
            this.materialProperties = new System.Windows.Forms.PropertyGrid();
            this.materialPropertiesLabel = new System.Windows.Forms.Label();
            this.materialsListBox = new StasisEditor.Controls.RefreshingListBox();
            this.previewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(387, 414);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(468, 414);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // materialTypesLabel
            // 
            this.materialTypesLabel.AutoSize = true;
            this.materialTypesLabel.Location = new System.Drawing.Point(13, 13);
            this.materialTypesLabel.Name = "materialTypesLabel";
            this.materialTypesLabel.Size = new System.Drawing.Size(36, 13);
            this.materialTypesLabel.TabIndex = 2;
            this.materialTypesLabel.Text = "Types";
            // 
            // materialTypesListBox
            // 
            this.materialTypesListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.materialTypesListBox.FormattingEnabled = true;
            this.materialTypesListBox.Location = new System.Drawing.Point(16, 29);
            this.materialTypesListBox.Name = "materialTypesListBox";
            this.materialTypesListBox.Size = new System.Drawing.Size(208, 54);
            this.materialTypesListBox.TabIndex = 3;
            this.materialTypesListBox.SelectedValueChanged += new System.EventHandler(this.materialTypesListBox_SelectedValueChanged);
            // 
            // materialsLabel
            // 
            this.materialsLabel.AutoSize = true;
            this.materialsLabel.Location = new System.Drawing.Point(12, 108);
            this.materialsLabel.Name = "materialsLabel";
            this.materialsLabel.Size = new System.Drawing.Size(49, 13);
            this.materialsLabel.TabIndex = 5;
            this.materialsLabel.Text = "Materials";
            // 
            // materialProperties
            // 
            this.materialProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materialProperties.Location = new System.Drawing.Point(250, 29);
            this.materialProperties.Name = "materialProperties";
            this.materialProperties.Size = new System.Drawing.Size(293, 362);
            this.materialProperties.TabIndex = 6;
            this.materialProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.materialProperties_PropertyValueChanged);
            // 
            // materialPropertiesLabel
            // 
            this.materialPropertiesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materialPropertiesLabel.AutoSize = true;
            this.materialPropertiesLabel.Location = new System.Drawing.Point(247, 13);
            this.materialPropertiesLabel.Name = "materialPropertiesLabel";
            this.materialPropertiesLabel.Size = new System.Drawing.Size(54, 13);
            this.materialPropertiesLabel.TabIndex = 7;
            this.materialPropertiesLabel.Text = "Properties";
            // 
            // materialsListBox
            // 
            this.materialsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.materialsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.materialsListBox.FormattingEnabled = true;
            this.materialsListBox.Location = new System.Drawing.Point(16, 125);
            this.materialsListBox.Name = "materialsListBox";
            this.materialsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.materialsListBox.Size = new System.Drawing.Size(208, 301);
            this.materialsListBox.TabIndex = 4;
            this.materialsListBox.SelectedIndexChanged += new System.EventHandler(this.materialsListBox_SelectedIndexChanged);
            // 
            // previewButton
            // 
            this.previewButton.Enabled = false;
            this.previewButton.Location = new System.Drawing.Point(306, 414);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(75, 23);
            this.previewButton.TabIndex = 8;
            this.previewButton.Text = "Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            // 
            // MaterialView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 449);
            this.ControlBox = false;
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.materialsListBox);
            this.Controls.Add(this.materialPropertiesLabel);
            this.Controls.Add(this.materialProperties);
            this.Controls.Add(this.materialsLabel);
            this.Controls.Add(this.materialTypesListBox);
            this.Controls.Add(this.materialTypesLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Name = "MaterialView";
            this.Text = "MaterialView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label materialTypesLabel;
        private System.Windows.Forms.ListBox materialTypesListBox;
        private System.Windows.Forms.Label materialsLabel;
        private System.Windows.Forms.PropertyGrid materialProperties;
        private System.Windows.Forms.Label materialPropertiesLabel;
        private RefreshingListBox materialsListBox;
        private System.Windows.Forms.Button previewButton;
    }
}