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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.backgroundDisplay = new StasisEditor.Views.Controls.BackgroundDisplay();
            this.SuspendLayout();
            // 
            // backgroundList
            // 
            this.backgroundList.FormattingEnabled = true;
            this.backgroundList.Location = new System.Drawing.Point(3, 3);
            this.backgroundList.Name = "backgroundList";
            this.backgroundList.Size = new System.Drawing.Size(187, 160);
            this.backgroundList.TabIndex = 1;
            // 
            // layerList
            // 
            this.layerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.layerList.FormattingEnabled = true;
            this.layerList.Location = new System.Drawing.Point(4, 209);
            this.layerList.Name = "layerList";
            this.layerList.Size = new System.Drawing.Size(186, 134);
            this.layerList.TabIndex = 2;
            // 
            // removeBackgroundButton
            // 
            this.removeBackgroundButton.Location = new System.Drawing.Point(85, 169);
            this.removeBackgroundButton.Name = "removeBackgroundButton";
            this.removeBackgroundButton.Size = new System.Drawing.Size(75, 23);
            this.removeBackgroundButton.TabIndex = 3;
            this.removeBackgroundButton.Text = "Remove";
            this.removeBackgroundButton.UseVisualStyleBackColor = true;
            // 
            // addBackgroundButton
            // 
            this.addBackgroundButton.Location = new System.Drawing.Point(4, 169);
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
            this.addBackgroundLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addBackgroundLayer.Location = new System.Drawing.Point(4, 349);
            this.addBackgroundLayer.Name = "addBackgroundLayer";
            this.addBackgroundLayer.Size = new System.Drawing.Size(75, 23);
            this.addBackgroundLayer.TabIndex = 6;
            this.addBackgroundLayer.Text = "Add";
            this.addBackgroundLayer.UseVisualStyleBackColor = true;
            // 
            // removeLayerButton
            // 
            this.removeLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeLayerButton.Location = new System.Drawing.Point(85, 349);
            this.removeLayerButton.Name = "removeLayerButton";
            this.removeLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeLayerButton.TabIndex = 7;
            this.removeLayerButton.Text = "Remove";
            this.removeLayerButton.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(4, 388);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(186, 146);
            this.propertyGrid1.TabIndex = 8;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // backgroundDisplay
            // 
            this.backgroundDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backgroundDisplay.Location = new System.Drawing.Point(196, 0);
            this.backgroundDisplay.Name = "backgroundDisplay";
            this.backgroundDisplay.Size = new System.Drawing.Size(589, 499);
            this.backgroundDisplay.TabIndex = 0;
            this.backgroundDisplay.Text = "backgroundDisplay";
            // 
            // BackgroundView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.removeLayerButton);
            this.Controls.Add(this.addBackgroundLayer);
            this.Controls.Add(this.saveBackgroundsButton);
            this.Controls.Add(this.addBackgroundButton);
            this.Controls.Add(this.removeBackgroundButton);
            this.Controls.Add(this.layerList);
            this.Controls.Add(this.backgroundList);
            this.Controls.Add(this.backgroundDisplay);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BackgroundView";
            this.Size = new System.Drawing.Size(785, 537);
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
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}
