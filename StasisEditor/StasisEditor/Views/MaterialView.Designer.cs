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
            this.previewButton = new System.Windows.Forms.Button();
            this.propertiesContainer = new System.Windows.Forms.Panel();
            this.materialsListBox = new StasisEditor.Controls.RefreshingListBox();
            this.autoUpdatePreview = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(505, 585);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(586, 585);
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
            this.materialTypesLabel.Location = new System.Drawing.Point(9, 12);
            this.materialTypesLabel.Name = "materialTypesLabel";
            this.materialTypesLabel.Size = new System.Drawing.Size(36, 13);
            this.materialTypesLabel.TabIndex = 2;
            this.materialTypesLabel.Text = "Types";
            // 
            // materialTypesListBox
            // 
            this.materialTypesListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.materialTypesListBox.FormattingEnabled = true;
            this.materialTypesListBox.Location = new System.Drawing.Point(12, 29);
            this.materialTypesListBox.Name = "materialTypesListBox";
            this.materialTypesListBox.Size = new System.Drawing.Size(221, 54);
            this.materialTypesListBox.TabIndex = 3;
            this.materialTypesListBox.SelectedValueChanged += new System.EventHandler(this.materialTypesListBox_SelectedValueChanged);
            // 
            // materialsLabel
            // 
            this.materialsLabel.AutoSize = true;
            this.materialsLabel.Location = new System.Drawing.Point(9, 109);
            this.materialsLabel.Name = "materialsLabel";
            this.materialsLabel.Size = new System.Drawing.Size(49, 13);
            this.materialsLabel.TabIndex = 5;
            this.materialsLabel.Text = "Materials";
            // 
            // previewButton
            // 
            this.previewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previewButton.Enabled = false;
            this.previewButton.Location = new System.Drawing.Point(424, 585);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(75, 23);
            this.previewButton.TabIndex = 8;
            this.previewButton.Text = "Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // propertiesContainer
            // 
            this.propertiesContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesContainer.AutoScroll = true;
            this.propertiesContainer.Location = new System.Drawing.Point(250, 12);
            this.propertiesContainer.Name = "propertiesContainer";
            this.propertiesContainer.Size = new System.Drawing.Size(411, 567);
            this.propertiesContainer.TabIndex = 9;
            // 
            // materialsListBox
            // 
            this.materialsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.materialsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.materialsListBox.FormattingEnabled = true;
            this.materialsListBox.Location = new System.Drawing.Point(12, 125);
            this.materialsListBox.Name = "materialsListBox";
            this.materialsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.materialsListBox.Size = new System.Drawing.Size(221, 470);
            this.materialsListBox.TabIndex = 4;
            this.materialsListBox.SelectedIndexChanged += new System.EventHandler(this.materialsListBox_SelectedIndexChanged);
            // 
            // autoUpdatePreview
            // 
            this.autoUpdatePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoUpdatePreview.AutoSize = true;
            this.autoUpdatePreview.Location = new System.Drawing.Point(291, 589);
            this.autoUpdatePreview.Name = "autoUpdatePreview";
            this.autoUpdatePreview.Size = new System.Drawing.Size(127, 17);
            this.autoUpdatePreview.TabIndex = 10;
            this.autoUpdatePreview.Text = "Auto Update Preview";
            this.autoUpdatePreview.UseVisualStyleBackColor = true;
            this.autoUpdatePreview.CheckedChanged += new System.EventHandler(this.autoUpdatePreview_CheckedChanged);
            // 
            // MaterialView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 620);
            this.ControlBox = false;
            this.Controls.Add(this.autoUpdatePreview);
            this.Controls.Add(this.propertiesContainer);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.materialsListBox);
            this.Controls.Add(this.materialsLabel);
            this.Controls.Add(this.materialTypesListBox);
            this.Controls.Add(this.materialTypesLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Name = "MaterialView";
            this.Text = "Materials";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MaterialView_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label materialTypesLabel;
        private System.Windows.Forms.ListBox materialTypesListBox;
        private System.Windows.Forms.Label materialsLabel;
        private RefreshingListBox materialsListBox;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Panel propertiesContainer;
        private System.Windows.Forms.CheckBox autoUpdatePreview;
    }
}