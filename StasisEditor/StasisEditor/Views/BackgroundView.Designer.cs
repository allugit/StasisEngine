using StasisEditor.Views.Controls;

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
            this.removeBackgroundButton = new System.Windows.Forms.Button();
            this.addBackgroundButton = new System.Windows.Forms.Button();
            this.saveBackgroundsButton = new System.Windows.Forms.Button();
            this.addBackgroundLayer = new System.Windows.Forms.Button();
            this.removeLayerButton = new System.Windows.Forms.Button();
            this.layerProperties = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.previewButton = new System.Windows.Forms.Button();
            this.horizontalScrollList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.verticalScrollList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundList = new StasisEditor.Views.Controls.RefreshingListBox();
            this.layerList = new StasisEditor.Views.Controls.RefreshingListBox();
            this.backgroundDisplay = new StasisEditor.Views.Controls.BackgroundDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.saveBackgroundsButton.Click += new System.EventHandler(this.saveBackgroundsButton_Click);
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
            this.layerProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.layerProperties_PropertyValueChanged);
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
            // previewButton
            // 
            this.previewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.previewButton.Location = new System.Drawing.Point(626, 511);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(75, 23);
            this.previewButton.TabIndex = 10;
            this.previewButton.Text = "Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // horizontalScrollList
            // 
            this.horizontalScrollList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.horizontalScrollList.FormattingEnabled = true;
            this.horizontalScrollList.Items.AddRange(new object[] {
            "None",
            "Left",
            "Right"});
            this.horizontalScrollList.Location = new System.Drawing.Point(499, 511);
            this.horizontalScrollList.Name = "horizontalScrollList";
            this.horizontalScrollList.Size = new System.Drawing.Size(121, 21);
            this.horizontalScrollList.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(385, 516);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Horizontal Auto Scroll";
            // 
            // verticalScrollList
            // 
            this.verticalScrollList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.verticalScrollList.FormattingEnabled = true;
            this.verticalScrollList.Items.AddRange(new object[] {
            "None",
            "Up",
            "Down"});
            this.verticalScrollList.Location = new System.Drawing.Point(214, 511);
            this.verticalScrollList.Name = "verticalScrollList";
            this.verticalScrollList.Size = new System.Drawing.Size(121, 21);
            this.verticalScrollList.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 516);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Vertical Auto Scroll";
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
            // backgroundDisplay
            // 
            this.backgroundDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundDisplay.Location = new System.Drawing.Point(0, 0);
            this.backgroundDisplay.Name = "backgroundDisplay";
            this.backgroundDisplay.Size = new System.Drawing.Size(586, 505);
            this.backgroundDisplay.TabIndex = 0;
            this.backgroundDisplay.Text = "backgroundDisplay";
            this.backgroundDisplay.view = null;
            // 
            // BackgroundView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.verticalScrollList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.horizontalScrollList);
            this.Controls.Add(this.previewButton);
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
            this.PerformLayout();

        }

        #endregion

        private Controls.BackgroundDisplay backgroundDisplay;
        private RefreshingListBox backgroundList;
        private RefreshingListBox layerList;
        private System.Windows.Forms.Button removeBackgroundButton;
        private System.Windows.Forms.Button addBackgroundButton;
        private System.Windows.Forms.Button saveBackgroundsButton;
        private System.Windows.Forms.Button addBackgroundLayer;
        private System.Windows.Forms.Button removeLayerButton;
        private System.Windows.Forms.PropertyGrid layerProperties;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.ComboBox horizontalScrollList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox verticalScrollList;
        private System.Windows.Forms.Label label2;
    }
}
