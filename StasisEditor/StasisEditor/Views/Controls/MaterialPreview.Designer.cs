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
            this.usePolygonPoints = new System.Windows.Forms.CheckBox();
            this.growthFactorBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.materialPreviewGraphics = new StasisEditor.Views.Controls.MaterialPreviewGraphics();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.growthFactorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.usePolygonPoints);
            this.panel1.Controls.Add(this.growthFactorBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 555);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 33);
            this.panel1.TabIndex = 1;
            // 
            // usePolygonPoints
            // 
            this.usePolygonPoints.AutoSize = true;
            this.usePolygonPoints.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.usePolygonPoints.Location = new System.Drawing.Point(192, 8);
            this.usePolygonPoints.Name = "usePolygonPoints";
            this.usePolygonPoints.Size = new System.Drawing.Size(118, 17);
            this.usePolygonPoints.TabIndex = 15;
            this.usePolygonPoints.Text = "Use Polygon Points";
            this.usePolygonPoints.UseVisualStyleBackColor = true;
            this.usePolygonPoints.CheckedChanged += new System.EventHandler(this.usePolygonPoints_CheckedChanged);
            // 
            // growthFactorBox
            // 
            this.growthFactorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.growthFactorBox.DecimalPlaces = 4;
            this.growthFactorBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.growthFactorBox.Location = new System.Drawing.Point(83, 7);
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
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
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
            this.materialPreviewGraphics.Size = new System.Drawing.Size(580, 555);
            this.materialPreviewGraphics.TabIndex = 2;
            this.materialPreviewGraphics.Text = "materialPreviewGraphics";
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox usePolygonPoints;
        private System.Windows.Forms.NumericUpDown growthFactorBox;
        private System.Windows.Forms.Label label1;
        private MaterialPreviewGraphics materialPreviewGraphics;

    }
}
