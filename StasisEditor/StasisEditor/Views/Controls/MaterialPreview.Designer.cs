namespace StasisEditor.Views.Controls
{
    partial class MaterialPreview
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.useTestPolygon = new System.Windows.Forms.CheckBox();
            this.growthFactorBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.materialPreviewGraphics = new StasisEditor.Views.Controls.MaterialPreviewGraphics();
            this.label2 = new System.Windows.Forms.Label();
            this.scaleBox = new System.Windows.Forms.NumericUpDown();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.growthFactorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.scaleBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.useTestPolygon);
            this.panel1.Controls.Add(this.growthFactorBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 558);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 30);
            this.panel1.TabIndex = 1;
            // 
            // useTestPolygon
            // 
            this.useTestPolygon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.useTestPolygon.AutoSize = true;
            this.useTestPolygon.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.useTestPolygon.Location = new System.Drawing.Point(461, 7);
            this.useTestPolygon.Name = "useTestPolygon";
            this.useTestPolygon.Size = new System.Drawing.Size(110, 17);
            this.useTestPolygon.TabIndex = 15;
            this.useTestPolygon.Text = "Use Test Polygon";
            this.useTestPolygon.UseVisualStyleBackColor = true;
            this.useTestPolygon.CheckedChanged += new System.EventHandler(this.usePolygonPoints_CheckedChanged);
            // 
            // growthFactorBox
            // 
            this.growthFactorBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.growthFactorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.growthFactorBox.DecimalPlaces = 4;
            this.growthFactorBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.growthFactorBox.Location = new System.Drawing.Point(344, 6);
            this.growthFactorBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.growthFactorBox.Name = "growthFactorBox";
            this.growthFactorBox.Size = new System.Drawing.Size(81, 20);
            this.growthFactorBox.TabIndex = 13;
            this.growthFactorBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.growthFactorBox.ValueChanged += new System.EventHandler(this.growthFactorBox_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Growth Factor";
            // 
            // materialPreviewGraphics
            // 
            this.materialPreviewGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialPreviewGraphics.Location = new System.Drawing.Point(0, 0);
            this.materialPreviewGraphics.Name = "materialPreviewGraphics";
            this.materialPreviewGraphics.Size = new System.Drawing.Size(580, 558);
            this.materialPreviewGraphics.TabIndex = 2;
            this.materialPreviewGraphics.Text = "materialPreviewGraphics";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Scale";
            // 
            // scaleBox
            // 
            this.scaleBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scaleBox.DecimalPlaces = 1;
            this.scaleBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.scaleBox.Location = new System.Drawing.Point(171, 6);
            this.scaleBox.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.scaleBox.Name = "scaleBox";
            this.scaleBox.Size = new System.Drawing.Size(61, 20);
            this.scaleBox.TabIndex = 17;
            this.scaleBox.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.scaleBox.ValueChanged += new System.EventHandler(this.scaleBox_ValueChanged);
            // 
            // MaterialPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(580, 588);
            this.Controls.Add(this.materialPreviewGraphics);
            this.Controls.Add(this.panel1);
            this.Name = "MaterialPreview";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialPreview_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.growthFactorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox useTestPolygon;
        private System.Windows.Forms.NumericUpDown growthFactorBox;
        private System.Windows.Forms.Label label1;
        private MaterialPreviewGraphics materialPreviewGraphics;
        private System.Windows.Forms.NumericUpDown scaleBox;
        private System.Windows.Forms.Label label2;

    }
}
