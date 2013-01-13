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
            this.propertiesGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertiesGrid
            // 
            this.propertiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesGrid.HelpVisible = false;
            this.propertiesGrid.Location = new System.Drawing.Point(3, 17);
            this.propertiesGrid.Name = "propertiesGrid";
            this.propertiesGrid.Size = new System.Drawing.Size(236, 99);
            this.propertiesGrid.TabIndex = 1;
            this.propertiesGrid.ToolbarVisible = false;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.propertiesGrid);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(4, 20);
            this.groupBox.Name = "groupBox";
            this.groupBox.Padding = new System.Windows.Forms.Padding(3, 4, 4, 4);
            this.groupBox.Size = new System.Drawing.Size(243, 120);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Actor Properties";
            // 
            // ActorPropertiesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "ActorPropertiesView";
            this.Padding = new System.Windows.Forms.Padding(4, 20, 4, 4);
            this.Size = new System.Drawing.Size(251, 144);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertiesGrid;
        private System.Windows.Forms.GroupBox groupBox;
    }
}
