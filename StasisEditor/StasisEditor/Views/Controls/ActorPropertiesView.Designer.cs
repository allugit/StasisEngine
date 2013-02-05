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
            this.actorPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // actorPropertyGrid
            // 
            this.actorPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actorPropertyGrid.HelpVisible = false;
            this.actorPropertyGrid.Location = new System.Drawing.Point(6, 20);
            this.actorPropertyGrid.Name = "actorPropertyGrid";
            this.actorPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.actorPropertyGrid.Size = new System.Drawing.Size(236, 257);
            this.actorPropertyGrid.TabIndex = 0;
            this.actorPropertyGrid.ToolbarVisible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.actorPropertyGrid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 7, 7, 7);
            this.groupBox1.Size = new System.Drawing.Size(249, 284);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actor Properties";
            // 
            // ActorPropertiesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ActorPropertiesView";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(257, 292);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid actorPropertyGrid;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
