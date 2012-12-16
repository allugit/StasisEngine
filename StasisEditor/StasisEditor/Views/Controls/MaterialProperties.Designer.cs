namespace StasisEditor.Views.Controls
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
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
            this.materialPropertyGrid.Size = new System.Drawing.Size(204, 179);
            this.materialPropertyGrid.TabIndex = 8;
            this.materialPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.materialPropertyGrid_PropertyValueChanged);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.materialPropertiesLabel);
            this.panel1.Controls.Add(this.materialPropertyGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 216);
            this.panel1.TabIndex = 10;
            // 
            // MaterialProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel1);
            this.Name = "MaterialProperties";
            this.Size = new System.Drawing.Size(204, 216);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label materialPropertiesLabel;
        private System.Windows.Forms.PropertyGrid materialPropertyGrid;
        private System.Windows.Forms.Panel panel1;

    }
}
