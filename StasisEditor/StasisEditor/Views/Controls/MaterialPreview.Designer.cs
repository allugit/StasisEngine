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
            this.materialPreviewGraphics = new StasisEditor.Views.Controls.MaterialPreviewGraphics();
            this.SuspendLayout();
            // 
            // materialPreviewGraphics
            // 
            this.materialPreviewGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialPreviewGraphics.Location = new System.Drawing.Point(0, 0);
            this.materialPreviewGraphics.Margin = new System.Windows.Forms.Padding(0);
            this.materialPreviewGraphics.Name = "materialPreviewGraphics";
            this.materialPreviewGraphics.Size = new System.Drawing.Size(512, 512);
            this.materialPreviewGraphics.TabIndex = 0;
            this.materialPreviewGraphics.Text = "materialPreviewGraphics1";
            // 
            // MaterialPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.materialPreviewGraphics);
            this.Name = "MaterialPreview";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialPreview_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialPreviewGraphics materialPreviewGraphics;

    }
}
