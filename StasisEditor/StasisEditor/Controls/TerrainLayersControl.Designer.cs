namespace StasisEditor.Controls
{
    partial class TerrainLayersControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.removeLayerButton = new System.Windows.Forms.Button();
            this.layerProperties = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.addLayerButton = new System.Windows.Forms.Button();
            this.layersTreeView = new StasisEditor.Controls.LayersTreeView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Terrain Layers";
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Enabled = false;
            this.upButton.Location = new System.Drawing.Point(279, 16);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(32, 23);
            this.upButton.TabIndex = 2;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Enabled = false;
            this.downButton.Location = new System.Drawing.Point(279, 45);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(32, 23);
            this.downButton.TabIndex = 3;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // removeLayerButton
            // 
            this.removeLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeLayerButton.Enabled = false;
            this.removeLayerButton.Location = new System.Drawing.Point(198, 148);
            this.removeLayerButton.Name = "removeLayerButton";
            this.removeLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeLayerButton.TabIndex = 5;
            this.removeLayerButton.Text = "Remove";
            this.removeLayerButton.UseVisualStyleBackColor = true;
            this.removeLayerButton.Click += new System.EventHandler(this.removeLayerButton_Click);
            // 
            // layerProperties
            // 
            this.layerProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.layerProperties.Location = new System.Drawing.Point(0, 211);
            this.layerProperties.Margin = new System.Windows.Forms.Padding(0);
            this.layerProperties.Name = "layerProperties";
            this.layerProperties.Size = new System.Drawing.Size(314, 208);
            this.layerProperties.TabIndex = 6;
            this.layerProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.layerProperties_PropertyValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Layer Properties";
            // 
            // addLayerButton
            // 
            this.addLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addLayerButton.Enabled = false;
            this.addLayerButton.Location = new System.Drawing.Point(122, 148);
            this.addLayerButton.Name = "addLayerButton";
            this.addLayerButton.Size = new System.Drawing.Size(70, 23);
            this.addLayerButton.TabIndex = 9;
            this.addLayerButton.Text = "Add";
            this.addLayerButton.UseVisualStyleBackColor = true;
            this.addLayerButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // layersTreeView
            // 
            this.layersTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.layersTreeView.CheckBoxes = true;
            this.layersTreeView.HideSelection = false;
            this.layersTreeView.Location = new System.Drawing.Point(0, 16);
            this.layersTreeView.Name = "layersTreeView";
            this.layersTreeView.ShowPlusMinus = false;
            this.layersTreeView.Size = new System.Drawing.Size(273, 126);
            this.layersTreeView.TabIndex = 8;
            this.layersTreeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.layersTreeView_BeforeCheck);
            this.layersTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.layersTreeView_AfterCheck);
            this.layersTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.layersTreeView_AfterSelect);
            // 
            // TerrainLayersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addLayerButton);
            this.Controls.Add(this.layersTreeView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.layerProperties);
            this.Controls.Add(this.removeLayerButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(273, 0);
            this.Name = "TerrainLayersControl";
            this.Size = new System.Drawing.Size(314, 422);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button removeLayerButton;
        private System.Windows.Forms.PropertyGrid layerProperties;
        private System.Windows.Forms.Label label2;
        private LayersTreeView layersTreeView;
        private System.Windows.Forms.Button addLayerButton;
    }
}
