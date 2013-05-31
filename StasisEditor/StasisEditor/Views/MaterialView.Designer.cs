using StasisEditor.Views.Controls;
namespace StasisEditor.Views
{
    partial class MaterialView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.cloneButton = new System.Windows.Forms.Button();
            this.materialPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.materialsListBox = new StasisEditor.Views.Controls.RefreshingListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.downButton = new System.Windows.Forms.Button();
            this.removeLayerButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.addLayerButton = new System.Windows.Forms.Button();
            this.layerPasteButton = new System.Windows.Forms.Button();
            this.layersTreeView = new StasisEditor.Views.Controls.LayersTreeView();
            this.layerCopyButton = new System.Windows.Forms.Button();
            this.layerProperties = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(599, 493);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(3, 493);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(58, 23);
            this.addButton.TabIndex = 11;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(67, 493);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(58, 23);
            this.removeButton.TabIndex = 12;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // cloneButton
            // 
            this.cloneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cloneButton.Enabled = false;
            this.cloneButton.Location = new System.Drawing.Point(131, 493);
            this.cloneButton.Name = "cloneButton";
            this.cloneButton.Size = new System.Drawing.Size(58, 23);
            this.cloneButton.TabIndex = 13;
            this.cloneButton.Text = "Clone";
            this.cloneButton.UseVisualStyleBackColor = true;
            this.cloneButton.Click += new System.EventHandler(this.cloneButton_Click);
            // 
            // materialPropertyGrid
            // 
            this.materialPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialPropertyGrid.HelpVisible = false;
            this.materialPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.materialPropertyGrid.Margin = new System.Windows.Forms.Padding(0);
            this.materialPropertyGrid.Name = "materialPropertyGrid";
            this.materialPropertyGrid.Size = new System.Drawing.Size(212, 238);
            this.materialPropertyGrid.TabIndex = 14;
            this.materialPropertyGrid.ToolbarVisible = false;
            this.materialPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.materialPropertyGrid_PropertyValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.materialsListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.materialPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(212, 484);
            this.splitContainer1.SplitterDistance = 242;
            this.splitContainer1.TabIndex = 16;
            // 
            // materialsListBox
            // 
            this.materialsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.materialsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialsListBox.FormattingEnabled = true;
            this.materialsListBox.Location = new System.Drawing.Point(0, 0);
            this.materialsListBox.Name = "materialsListBox";
            this.materialsListBox.Size = new System.Drawing.Size(212, 242);
            this.materialsListBox.TabIndex = 4;
            this.materialsListBox.SelectedIndexChanged += new System.EventHandler(this.materialsListBox_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(674, 484);
            this.splitContainer2.SplitterDistance = 212;
            this.splitContainer2.TabIndex = 17;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.downButton);
            this.splitContainer3.Panel1.Controls.Add(this.removeLayerButton);
            this.splitContainer3.Panel1.Controls.Add(this.upButton);
            this.splitContainer3.Panel1.Controls.Add(this.addLayerButton);
            this.splitContainer3.Panel1.Controls.Add(this.layerPasteButton);
            this.splitContainer3.Panel1.Controls.Add(this.layersTreeView);
            this.splitContainer3.Panel1.Controls.Add(this.layerCopyButton);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.layerProperties);
            this.splitContainer3.Size = new System.Drawing.Size(458, 484);
            this.splitContainer3.SplitterDistance = 242;
            this.splitContainer3.TabIndex = 13;
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Enabled = false;
            this.downButton.Location = new System.Drawing.Point(423, 32);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(32, 23);
            this.downButton.TabIndex = 3;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // removeLayerButton
            // 
            this.removeLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeLayerButton.Enabled = false;
            this.removeLayerButton.Location = new System.Drawing.Point(380, 211);
            this.removeLayerButton.Name = "removeLayerButton";
            this.removeLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeLayerButton.TabIndex = 5;
            this.removeLayerButton.Text = "Remove";
            this.removeLayerButton.UseVisualStyleBackColor = true;
            this.removeLayerButton.Click += new System.EventHandler(this.removeLayerButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Enabled = false;
            this.upButton.Location = new System.Drawing.Point(423, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(32, 23);
            this.upButton.TabIndex = 2;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // addLayerButton
            // 
            this.addLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addLayerButton.Enabled = false;
            this.addLayerButton.Location = new System.Drawing.Point(304, 211);
            this.addLayerButton.Name = "addLayerButton";
            this.addLayerButton.Size = new System.Drawing.Size(70, 23);
            this.addLayerButton.TabIndex = 9;
            this.addLayerButton.Text = "Add";
            this.addLayerButton.UseVisualStyleBackColor = true;
            this.addLayerButton.Click += new System.EventHandler(this.addLayerButton_Click);
            // 
            // layerPasteButton
            // 
            this.layerPasteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.layerPasteButton.Enabled = false;
            this.layerPasteButton.Location = new System.Drawing.Point(84, 211);
            this.layerPasteButton.Name = "layerPasteButton";
            this.layerPasteButton.Size = new System.Drawing.Size(75, 23);
            this.layerPasteButton.TabIndex = 11;
            this.layerPasteButton.Text = "Paste";
            this.layerPasteButton.UseVisualStyleBackColor = true;
            this.layerPasteButton.Click += new System.EventHandler(this.layerPasteButton_Click);
            // 
            // layersTreeView
            // 
            this.layersTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layersTreeView.CheckBoxes = true;
            this.layersTreeView.HideSelection = false;
            this.layersTreeView.Location = new System.Drawing.Point(3, 3);
            this.layersTreeView.Name = "layersTreeView";
            this.layersTreeView.ShowPlusMinus = false;
            this.layersTreeView.Size = new System.Drawing.Size(416, 202);
            this.layersTreeView.TabIndex = 8;
            this.layersTreeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.layersTreeView_BeforeCheck);
            this.layersTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.layersTreeView_AfterCheck);
            this.layersTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.layersTreeView_AfterSelect);
            // 
            // layerCopyButton
            // 
            this.layerCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.layerCopyButton.Enabled = false;
            this.layerCopyButton.Location = new System.Drawing.Point(3, 211);
            this.layerCopyButton.Name = "layerCopyButton";
            this.layerCopyButton.Size = new System.Drawing.Size(75, 23);
            this.layerCopyButton.TabIndex = 10;
            this.layerCopyButton.Text = "Copy";
            this.layerCopyButton.UseVisualStyleBackColor = true;
            this.layerCopyButton.Click += new System.EventHandler(this.layerCopyButton_Click);
            // 
            // layerProperties
            // 
            this.layerProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layerProperties.HelpVisible = false;
            this.layerProperties.Location = new System.Drawing.Point(0, 0);
            this.layerProperties.Margin = new System.Windows.Forms.Padding(0);
            this.layerProperties.Name = "layerProperties";
            this.layerProperties.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.layerProperties.Size = new System.Drawing.Size(458, 238);
            this.layerProperties.TabIndex = 6;
            this.layerProperties.ToolbarVisible = false;
            this.layerProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.layerProperties_PropertyValueChanged);
            // 
            // MaterialView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.cloneButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.saveButton);
            this.Name = "MaterialView";
            this.Size = new System.Drawing.Size(680, 527);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private RefreshingListBox materialsListBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button cloneButton;
        private System.Windows.Forms.PropertyGrid materialPropertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button removeLayerButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button addLayerButton;
        private System.Windows.Forms.Button layerPasteButton;
        private LayersTreeView layersTreeView;
        private System.Windows.Forms.Button layerCopyButton;
        private System.Windows.Forms.PropertyGrid layerProperties;
    }
}