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
            this.growthFactorBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.growthFactorBox)).BeginInit();
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
            this.materialPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.materialPropertyGrid.HelpVisible = false;
            this.materialPropertyGrid.Location = new System.Drawing.Point(0, 17);
            this.materialPropertyGrid.Margin = new System.Windows.Forms.Padding(0);
            this.materialPropertyGrid.Name = "materialPropertyGrid";
            this.materialPropertyGrid.Size = new System.Drawing.Size(204, 100);
            this.materialPropertyGrid.TabIndex = 8;
            this.materialPropertyGrid.ToolbarVisible = false;
            this.materialPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.materialPropertyGrid_PropertyValueChanged);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.growthFactorBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.materialPropertiesLabel);
            this.panel1.Controls.Add(this.materialPropertyGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 146);
            this.panel1.TabIndex = 10;
            // 
            // growthFactorBox
            // 
            this.growthFactorBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.growthFactorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.growthFactorBox.DecimalPlaces = 4;
            this.growthFactorBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.growthFactorBox.Location = new System.Drawing.Point(122, 120);
            this.growthFactorBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.growthFactorBox.Name = "growthFactorBox";
            this.growthFactorBox.Size = new System.Drawing.Size(81, 20);
            this.growthFactorBox.TabIndex = 11;
            this.growthFactorBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.growthFactorBox.ValueChanged += new System.EventHandler(this.growthFactorBox_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Growth Factor Preview";
            // 
            // MaterialProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "MaterialProperties";
            this.Size = new System.Drawing.Size(204, 147);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.growthFactorBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label materialPropertiesLabel;
        private System.Windows.Forms.PropertyGrid materialPropertyGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown growthFactorBox;
        private System.Windows.Forms.Label label1;

    }
}
