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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.saveWorldMapsButton = new System.Windows.Forms.Button();
            this.worldMapPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.removeWorldMapButton = new System.Windows.Forms.Button();
            this.addWorldMapButton = new System.Windows.Forms.Button();
            this.worldMapListBox = new StasisEditor.Views.Controls.RefreshingListBox();
            this.worldMapDisplay1 = new StasisEditor.Views.Controls.WorldMapDisplay();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.toolStrip1.Size = new System.Drawing.Size(178, 25);
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
            this.createLevelIconButton.Click += new System.EventHandler(this.createLevelIconButton_Click);
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.saveWorldMapsButton);
            this.splitContainer1.Panel1.Controls.Add(this.worldMapPropertyGrid);
            this.splitContainer1.Panel1.Controls.Add(this.removeWorldMapButton);
            this.splitContainer1.Panel1.Controls.Add(this.addWorldMapButton);
            this.splitContainer1.Panel1.Controls.Add(this.worldMapListBox);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.worldMapDisplay1);
            this.splitContainer1.Size = new System.Drawing.Size(535, 370);
            this.splitContainer1.SplitterDistance = 178;
            this.splitContainer1.TabIndex = 2;
            // 
            // saveWorldMapsButton
            // 
            this.saveWorldMapsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveWorldMapsButton.Location = new System.Drawing.Point(4, 337);
            this.saveWorldMapsButton.Name = "saveWorldMapsButton";
            this.saveWorldMapsButton.Size = new System.Drawing.Size(108, 23);
            this.saveWorldMapsButton.TabIndex = 5;
            this.saveWorldMapsButton.Text = "Save World Maps";
            this.saveWorldMapsButton.UseVisualStyleBackColor = true;
            this.saveWorldMapsButton.Click += new System.EventHandler(this.saveWorldMapsButton_Click);
            // 
            // worldMapPropertyGrid
            // 
            this.worldMapPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.worldMapPropertyGrid.HelpVisible = false;
            this.worldMapPropertyGrid.Location = new System.Drawing.Point(3, 200);
            this.worldMapPropertyGrid.Name = "worldMapPropertyGrid";
            this.worldMapPropertyGrid.Size = new System.Drawing.Size(172, 130);
            this.worldMapPropertyGrid.TabIndex = 4;
            this.worldMapPropertyGrid.ToolbarVisible = false;
            // 
            // removeWorldMapButton
            // 
            this.removeWorldMapButton.Location = new System.Drawing.Point(86, 156);
            this.removeWorldMapButton.Name = "removeWorldMapButton";
            this.removeWorldMapButton.Size = new System.Drawing.Size(75, 23);
            this.removeWorldMapButton.TabIndex = 3;
            this.removeWorldMapButton.Text = "Remove";
            this.removeWorldMapButton.UseVisualStyleBackColor = true;
            this.removeWorldMapButton.Click += new System.EventHandler(this.removeWorldMapButton_Click);
            // 
            // addWorldMapButton
            // 
            this.addWorldMapButton.Location = new System.Drawing.Point(4, 156);
            this.addWorldMapButton.Name = "addWorldMapButton";
            this.addWorldMapButton.Size = new System.Drawing.Size(75, 23);
            this.addWorldMapButton.TabIndex = 2;
            this.addWorldMapButton.Text = "Add";
            this.addWorldMapButton.UseVisualStyleBackColor = true;
            this.addWorldMapButton.Click += new System.EventHandler(this.addWorldMapButton_Click);
            // 
            // worldMapListBox
            // 
            this.worldMapListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.worldMapListBox.FormattingEnabled = true;
            this.worldMapListBox.Location = new System.Drawing.Point(3, 28);
            this.worldMapListBox.Name = "worldMapListBox";
            this.worldMapListBox.Size = new System.Drawing.Size(172, 121);
            this.worldMapListBox.TabIndex = 1;
            this.worldMapListBox.SelectedValueChanged += new System.EventHandler(this.worldMapListBox_SelectedValueChanged);
            // 
            // worldMapDisplay1
            // 
            this.worldMapDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.worldMapDisplay1.Location = new System.Drawing.Point(0, 0);
            this.worldMapDisplay1.Name = "worldMapDisplay1";
            this.worldMapDisplay1.Size = new System.Drawing.Size(353, 370);
            this.worldMapDisplay1.TabIndex = 1;
            this.worldMapDisplay1.Text = "worldMapDisplay1";
            this.worldMapDisplay1.view = null;
            // 
            // WorldMapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "WorldMapView";
            this.Size = new System.Drawing.Size(535, 370);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton createLevelIconButton;
        private System.Windows.Forms.ToolStripButton createPathButton;
        private Controls.WorldMapDisplay worldMapDisplay1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button removeWorldMapButton;
        private System.Windows.Forms.Button addWorldMapButton;
        private Controls.RefreshingListBox worldMapListBox;
        private System.Windows.Forms.Button saveWorldMapsButton;
        private System.Windows.Forms.PropertyGrid worldMapPropertyGrid;
    }
}
