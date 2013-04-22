namespace StasisEditor.Views
{
    partial class WorldMapView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldMapView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createLevelIconButton = new System.Windows.Forms.ToolStripButton();
            this.createPathButton = new System.Windows.Forms.ToolStripButton();
            this.worldMapDisplay1 = new StasisEditor.Views.Controls.WorldMapDisplay();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createLevelIconButton,
            this.createPathButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(535, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // createLevelIconButton
            // 
            this.createLevelIconButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.createLevelIconButton.Image = ((System.Drawing.Image)(resources.GetObject("createLevelIconButton.Image")));
            this.createLevelIconButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createLevelIconButton.Name = "createLevelIconButton";
            this.createLevelIconButton.Size = new System.Drawing.Size(23, 22);
            this.createLevelIconButton.Text = "Create Level Icon";
            // 
            // createPathButton
            // 
            this.createPathButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.createPathButton.Image = ((System.Drawing.Image)(resources.GetObject("createPathButton.Image")));
            this.createPathButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createPathButton.Name = "createPathButton";
            this.createPathButton.Size = new System.Drawing.Size(23, 22);
            this.createPathButton.Text = "Create Path";
            // 
            // worldMapDisplay1
            // 
            this.worldMapDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.worldMapDisplay1.Location = new System.Drawing.Point(0, 25);
            this.worldMapDisplay1.Name = "worldMapDisplay1";
            this.worldMapDisplay1.Size = new System.Drawing.Size(535, 345);
            this.worldMapDisplay1.TabIndex = 1;
            this.worldMapDisplay1.Text = "worldMapDisplay1";
            this.worldMapDisplay1.view = null;
            // 
            // WorldMapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.worldMapDisplay1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "WorldMapView";
            this.Size = new System.Drawing.Size(535, 370);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton createLevelIconButton;
        private System.Windows.Forms.ToolStripButton createPathButton;
        private Controls.WorldMapDisplay worldMapDisplay1;
    }
}
