namespace StasisEditor.Views
{
    partial class BackgroundView
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
            this.backgroundList = new System.Windows.Forms.ListBox();
            this.layerList = new System.Windows.Forms.ListBox();
            this.removeBackgroundButton = new System.Windows.Forms.Button();
            this.addBackgroundButton = new System.Windows.Forms.Button();
            this.saveBackgroundsButton = new System.Windows.Forms.Button();
            this.addBackgroundLayer = new System.Windows.Forms.Button();
            this.removeLayerButton = new System.Windows.Forms.Button();
            this.layerProperties = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.backgroundDisplay = new StasisEditor.Views.Controls.BackgroundDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundList
            // 
            this.backgroundList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backgroundList.FormattingEnabled = true;
            this.backgroundList.Location = new System.Drawing.Point(0, 0);
            this.backgroundList.Name = "backgroundList";
            this.backgroundList.Size = new System.Drawing.Size(187, 160);
            this.backgroundList.TabIndex = 1;
            this.backgroundList.SelectedValueChanged += new System.EventHandler(this.backgroundList_SelectedValueChanged);
            // 
            // layerList
            // 
            this.layerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.layerList.FormattingEnabled = true;
            this.layerList.Location = new System.Drawing.Point(1, 206);
            this.layerList.Name = "layerList";
            this.layerList.Size = new System.Drawing.Size(186, 147);
            this.layerList.TabIndex = 2;
            this.layerList.SelectedValueChanged += new System.EventHandler(this.layerList_SelectedValueChanged);
            // 
            // removeBackgroundButton
            // 
            this.removeBackgroundButton.Location = new System.Drawing.Point(82, 166);
            this.removeBackgroundButton.Name = "removeBackgroundButton";
            this.removeBackgroundButton.Size = new System.Drawing.Size(75, 23);
            this.removeBackgroundButton.TabIndex = 3;
            this.removeBackgroundButton.Text = "Remove";
            this.removeBackgroundButton.UseVisualStyleBackColor = true;
            this.removeBackgroundButton.Click += new System.EventHandler(this.removeBackgroundButton_Click);
            // 
            // addBackgroundButton
            // 
            this.addBackgroundButton.Location = new System.Drawing.Point(1, 166);
            this.addBackgroundButton.Name = "addBackgroundButton";
            this.addBackgroundButton.Size = new System.Drawing.Size(75, 23);
            this.addBackgroundButton.TabIndex = 4;
            this.addBackgroundButton.Text = "Add";
            this.addBackgroundButton.UseVisualStyleBackColor = true;
            this.addBackgroundButton.Click += new System.EventHandler(this.addBackgroundButton_Click);
            // 
            // saveBackgroundsButton
            // 
            this.saveBackgroundsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveBackgroundsButton.Location = new System.Drawing.Point(707, 511);
            this.saveBackgroundsButton.Name = "saveBackgroundsButton";
            this.saveBackgroundsButton.Size = new System.Drawing.Size(75, 23);
            this.saveBackgroundsButton.TabIndex = 5;
            this.saveBackgroundsButton.Text = "Save";
            this.saveBackgroundsButton.UseVisualStyleBackColor = true;
            // 
            // addBackgroundLayer
            // 
            this.addBackgroundLayer.Location = new System.Drawing.Point(1, 359);
            this.addBackgroundLayer.Name = "addBackgroundLayer";
            this.addBackgroundLayer.Size = new System.Drawing.Size(75, 23);
            this.addBackgroundLayer.TabIndex = 6;
            this.addBackgroundLayer.Text = "Add";
            this.addBackgroundLayer.UseVisualStyleBackColor = true;
            this.addBackgroundLayer.Click += new System.EventHandler(this.addBackgroundLayer_Click);
            // 
            // removeLayerButton
            // 
            this.removeLayerButton.Location = new System.Drawing.Point(82, 359);
            this.removeLayerButton.Name = "removeLayerButton";
            this.removeLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeLayerButton.TabIndex = 7;
            this.removeLayerButton.Text = "Remove";
            this.removeLayerButton.UseVisualStyleBackColor = true;
            this.removeLayerButton.Click += new System.EventHandler(this.removeLayerButton_Click);
            // 
            // layerProperties
            // 
            this.layerProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.layerProperties.HelpVisible = false;
            this.layerProperties.Location = new System.Drawing.Point(3, 398);
            this.layerProperties.Name = "layerProperties";
            this.layerProperties.Size = new System.Drawing.Size(184, 104);
            this.layerProperties.TabIndex = 8;
            this.layerProperties.ToolbarVisible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.backgroundList);
            this.splitContainer1.Panel1.Controls.Add(this.layerProperties);
            this.splitContainer1.Panel1.Controls.Add(this.layerList);
            this.splitContainer1.Panel1.Controls.Add(this.removeLayerButton);
            this.splitContainer1.Panel1.Controls.Add(this.removeBackgroundButton);
            this.splitContainer1.Panel1.Controls.Add(this.addBackgroundLayer);
            this.splitContainer1.Panel1.Controls.Add(this.addBackgroundButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.backgroundDisplay);
            this.splitContainer1.Size = new System.Drawing.Size(782, 505);
            this.splitContainer1.SplitterDistance = 192;
            this.splitContainer1.TabIndex = 9;
            // 
            // backgroundDisplay
            // 
            this.backgroundDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundDisplay.Location = new System.Drawing.Point(0, 0);
            this.backgroundDisplay.Name = "backgroundDisplay";
            this.backgroundDisplay.Size = new System.Drawing.Size(586, 505);
            this.backgroundDisplay.TabIndex = 0;
            this.backgroundDisplay.Text = "backgroundDisplay";
            // 
            // BackgroundView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.saveBackgroundsButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BackgroundView";
            this.Size = new System.Drawing.Size(785, 537);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.BackgroundDisplay backgroundDisplay;
        private System.Windows.Forms.ListBox backgroundList;
        private System.Windows.Forms.ListBox layerList;
        private System.Windows.Forms.Button removeBackgroundButton;
        private System.Windows.Forms.Button addBackgroundButton;
        private System.Windows.Forms.Button saveBackgroundsButton;
        private System.Windows.Forms.Button addBackgroundLayer;
        private System.Windows.Forms.Button removeLayerButton;
        private System.Windows.Forms.PropertyGrid layerProperties;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
