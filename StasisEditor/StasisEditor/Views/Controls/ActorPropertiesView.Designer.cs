namespace StasisEditor.Views.Controls
{
    partial class ActorPropertiesView
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
            this.propertiesTitle = new System.Windows.Forms.Label();
            this.propertiesGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertiesTitle
            // 
            this.propertiesTitle.AutoSize = true;
            this.propertiesTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertiesTitle.Location = new System.Drawing.Point(0, 26);
            this.propertiesTitle.Margin = new System.Windows.Forms.Padding(0);
            this.propertiesTitle.Name = "propertiesTitle";
            this.propertiesTitle.Size = new System.Drawing.Size(98, 13);
            this.propertiesTitle.TabIndex = 0;
            this.propertiesTitle.Text = "Actor Properties";
            // 
            // propertiesGrid
            // 
            this.propertiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesGrid.Location = new System.Drawing.Point(0, 42);
            this.propertiesGrid.Name = "propertiesGrid";
            this.propertiesGrid.Size = new System.Drawing.Size(251, 305);
            this.propertiesGrid.TabIndex = 1;
            // 
            // ActorPropertiesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertiesGrid);
            this.Controls.Add(this.propertiesTitle);
            this.Name = "ActorPropertiesView";
            this.Size = new System.Drawing.Size(251, 347);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label propertiesTitle;
        private System.Windows.Forms.PropertyGrid propertiesGrid;
    }
}
