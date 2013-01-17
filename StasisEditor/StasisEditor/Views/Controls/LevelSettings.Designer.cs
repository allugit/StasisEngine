namespace StasisEditor.Views.Controls
{
    partial class LevelSettings
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
            this.levelPropertiesGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // levelPropertiesGrid
            // 
            this.levelPropertiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelPropertiesGrid.HelpVisible = false;
            this.levelPropertiesGrid.Location = new System.Drawing.Point(3, 17);
            this.levelPropertiesGrid.Margin = new System.Windows.Forms.Padding(0);
            this.levelPropertiesGrid.Name = "levelPropertiesGrid";
            this.levelPropertiesGrid.Size = new System.Drawing.Size(219, 85);
            this.levelPropertiesGrid.TabIndex = 1;
            this.levelPropertiesGrid.ToolbarVisible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.levelPropertiesGrid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(226, 106);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Settings";
            // 
            // LevelSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "LevelSettings";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(234, 114);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid levelPropertiesGrid;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
