namespace StasisEditor.Controls
{
    partial class MaterialProperties
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.materialPropertiesLabel = new System.Windows.Forms.Label();
            this.materialPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // materialPropertiesLabel
            // 
            this.materialPropertiesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materialPropertiesLabel.AutoSize = true;
            this.materialPropertiesLabel.Location = new System.Drawing.Point(0, 0);
            this.materialPropertiesLabel.Margin = new System.Windows.Forms.Padding(0);
            this.materialPropertiesLabel.Name = "materialPropertiesLabel";
            this.materialPropertiesLabel.Size = new System.Drawing.Size(54, 13);
            this.materialPropertiesLabel.TabIndex = 9;
            this.materialPropertiesLabel.Text = "Properties";
            // 
            // materialPropertyGrid
            // 
            this.materialPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materialPropertyGrid.Location = new System.Drawing.Point(0, 13);
            this.materialPropertyGrid.Margin = new System.Windows.Forms.Padding(0);
            this.materialPropertyGrid.Name = "materialPropertyGrid";
            this.materialPropertyGrid.Size = new System.Drawing.Size(245, 166);
            this.materialPropertyGrid.TabIndex = 8;
            // 
            // MaterialProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialPropertiesLabel);
            this.Controls.Add(this.materialPropertyGrid);
            this.Name = "MaterialProperties";
            this.Size = new System.Drawing.Size(245, 179);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label materialPropertiesLabel;
        private System.Windows.Forms.PropertyGrid materialPropertyGrid;

    }
}
