namespace StasisEditor.Views
{
    partial class TextureView
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
            this.label1 = new System.Windows.Forms.Label();
            this.textureDataGrid = new System.Windows.Forms.DataGridView();
            this.removeTextureButton = new System.Windows.Forms.Button();
            this.addTextureButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.previewContainer = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.textureDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Textures";
            // 
            // textureDataGrid
            // 
            this.textureDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.textureDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.textureDataGrid.Location = new System.Drawing.Point(15, 26);
            this.textureDataGrid.Name = "textureDataGrid";
            this.textureDataGrid.Size = new System.Drawing.Size(302, 377);
            this.textureDataGrid.TabIndex = 2;
            this.textureDataGrid.SelectionChanged += new System.EventHandler(this.textureDataGrid_SelectionChanged);
            // 
            // removeTextureButton
            // 
            this.removeTextureButton.Location = new System.Drawing.Point(96, 409);
            this.removeTextureButton.Name = "removeTextureButton";
            this.removeTextureButton.Size = new System.Drawing.Size(75, 23);
            this.removeTextureButton.TabIndex = 3;
            this.removeTextureButton.Text = "Remove";
            this.removeTextureButton.UseVisualStyleBackColor = true;
            // 
            // addTextureButton
            // 
            this.addTextureButton.Location = new System.Drawing.Point(15, 409);
            this.addTextureButton.Name = "addTextureButton";
            this.addTextureButton.Size = new System.Drawing.Size(75, 23);
            this.addTextureButton.TabIndex = 4;
            this.addTextureButton.Text = "Add";
            this.addTextureButton.UseVisualStyleBackColor = true;
            this.addTextureButton.Click += new System.EventHandler(this.addTextureButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(320, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Preview";
            // 
            // previewContainer
            // 
            this.previewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewContainer.AutoScroll = true;
            this.previewContainer.BackColor = System.Drawing.Color.Black;
            this.previewContainer.Location = new System.Drawing.Point(323, 26);
            this.previewContainer.Name = "previewContainer";
            this.previewContainer.Size = new System.Drawing.Size(405, 377);
            this.previewContainer.TabIndex = 7;
            // 
            // TextureView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 444);
            this.Controls.Add(this.previewContainer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.addTextureButton);
            this.Controls.Add(this.removeTextureButton);
            this.Controls.Add(this.textureDataGrid);
            this.Controls.Add(this.label1);
            this.Name = "TextureView";
            this.Text = "Textures";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TextureView_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.textureDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView textureDataGrid;
        private System.Windows.Forms.Button removeTextureButton;
        private System.Windows.Forms.Button addTextureButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel previewContainer;
    }
}