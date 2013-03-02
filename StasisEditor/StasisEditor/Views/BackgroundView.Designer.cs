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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.removeBackgroundButton = new System.Windows.Forms.Button();
            this.addBackgroundButton = new System.Windows.Forms.Button();
            this.saveBackgroundsButton = new System.Windows.Forms.Button();
            this.addBackgroundLayer = new System.Windows.Forms.Button();
            this.removeLayerButton = new System.Windows.Forms.Button();
            this.backgroundDisplay1 = new StasisEditor.Views.Controls.BackgroundDisplay();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(187, 160);
            this.listBox1.TabIndex = 1;
            // 
            // listBox2
            // 
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(4, 209);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(186, 290);
            this.listBox2.TabIndex = 2;
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
            this.addBackgroundLayer.Location = new System.Drawing.Point(4, 511);
            this.addBackgroundLayer.Name = "addBackgroundLayer";
            this.addBackgroundLayer.Size = new System.Drawing.Size(75, 23);
            this.addBackgroundLayer.TabIndex = 6;
            this.addBackgroundLayer.Text = "Add";
            this.addBackgroundLayer.UseVisualStyleBackColor = true;
            // 
            // removeLayerButton
            // 
            this.removeLayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeLayerButton.Location = new System.Drawing.Point(85, 511);
            this.removeLayerButton.Name = "removeLayerButton";
            this.removeLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeLayerButton.TabIndex = 7;
            this.removeLayerButton.Text = "Remove";
            this.removeLayerButton.UseVisualStyleBackColor = true;
            // 
            // backgroundDisplay1
            // 
            this.backgroundDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backgroundDisplay1.Location = new System.Drawing.Point(196, 0);
            this.backgroundDisplay1.Name = "backgroundDisplay1";
            this.backgroundDisplay1.Size = new System.Drawing.Size(589, 499);
            this.backgroundDisplay1.TabIndex = 0;
            this.backgroundDisplay1.Text = "backgroundDisplay1";
            // 
            // BackgroundView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.removeLayerButton);
            this.Controls.Add(this.addBackgroundLayer);
            this.Controls.Add(this.saveBackgroundsButton);
            this.Controls.Add(this.addBackgroundButton);
            this.Controls.Add(this.removeBackgroundButton);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.backgroundDisplay1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BackgroundView";
            this.Size = new System.Drawing.Size(785, 537);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.BackgroundDisplay backgroundDisplay1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button removeBackgroundButton;
        private System.Windows.Forms.Button addBackgroundButton;
        private System.Windows.Forms.Button saveBackgroundsButton;
        private System.Windows.Forms.Button addBackgroundLayer;
        private System.Windows.Forms.Button removeLayerButton;
    }
}
