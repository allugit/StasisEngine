namespace StasisEditor.Views
{
    partial class LevelView
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
            this.surface = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.surface)).BeginInit();
            this.SuspendLayout();
            // 
            // surface
            // 
            this.surface.BackColor = System.Drawing.Color.Black;
            this.surface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.surface.Location = new System.Drawing.Point(0, 0);
            this.surface.Margin = new System.Windows.Forms.Padding(0);
            this.surface.Name = "surface";
            this.surface.Size = new System.Drawing.Size(150, 150);
            this.surface.TabIndex = 0;
            this.surface.TabStop = false;
            this.surface.MouseDown += new System.Windows.Forms.MouseEventHandler(this.surface_MouseDown);
            // 
            // LevelView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.surface);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LevelView";
            ((System.ComponentModel.ISupportInitialize)(this.surface)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox surface;
    }
}
