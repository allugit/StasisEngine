using StasisEditor.Views.Controls;
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
            this.previewButton = new System.Windows.Forms.Button();
            this.propertiesContainer = new System.Windows.Forms.Panel();
            this.autoUpdatePreview = new System.Windows.Forms.CheckBox();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.materialsListBox = new StasisEditor.Views.Controls.RefreshingListBox();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(592, 503);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // previewButton
            // 
            this.previewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previewButton.Enabled = false;
            this.previewButton.Location = new System.Drawing.Point(511, 503);
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
            this.propertiesContainer.Size = new System.Drawing.Size(418, 484);
            this.propertiesContainer.TabIndex = 9;
            // 
            // autoUpdatePreview
            // 
            this.autoUpdatePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoUpdatePreview.AutoSize = true;
            this.autoUpdatePreview.Location = new System.Drawing.Point(378, 507);
            this.autoUpdatePreview.Name = "autoUpdatePreview";
            this.autoUpdatePreview.Size = new System.Drawing.Size(127, 17);
            this.autoUpdatePreview.TabIndex = 10;
            this.autoUpdatePreview.Text = "Auto Update Preview";
            this.autoUpdatePreview.UseVisualStyleBackColor = true;
            this.autoUpdatePreview.CheckedChanged += new System.EventHandler(this.autoUpdatePreview_CheckedChanged);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(12, 503);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(58, 23);
            this.addButton.TabIndex = 11;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(76, 503);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(58, 23);
            this.removeButton.TabIndex = 12;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // cloneButton
            // 
            this.cloneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cloneButton.Enabled = false;
            this.cloneButton.Location = new System.Drawing.Point(140, 503);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(58, 23);
            this.cloneButton.TabIndex = 13;
            this.cloneButton.Text = "Clone";
            this.cloneButton.UseVisualStyleBackColor = true;
            this.cloneButton.Click += new System.EventHandler(this.cloneButton_Click);
            // 
            // materialsListBox
            // 
            this.materialsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.materialsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.materialsListBox.FormattingEnabled = true;
            this.materialsListBox.Location = new System.Drawing.Point(12, 12);
            this.materialsListBox.Name = "materialsListBox";
            this.materialsListBox.Size = new System.Drawing.Size(221, 483);
            this.materialsListBox.TabIndex = 4;
            this.materialsListBox.SelectedIndexChanged += new System.EventHandler(this.materialsListBox_SelectedIndexChanged);
            // 
            // MaterialView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.autoUpdatePreview);
            this.Controls.Add(this.propertiesContainer);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.materialsListBox);
            this.Controls.Add(this.saveButton);
            this.Name = "MaterialView";
            this.Size = new System.Drawing.Size(680, 537);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private RefreshingListBox materialsListBox;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Panel propertiesContainer;
        private System.Windows.Forms.CheckBox autoUpdatePreview;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button cloneButton;
    }
}