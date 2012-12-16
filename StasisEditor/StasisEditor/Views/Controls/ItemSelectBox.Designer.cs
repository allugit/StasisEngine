namespace StasisEditor.Views.Controls
{
    partial class ItemSelectBox
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
            this.allItemsListBox = new System.Windows.Forms.ListBox();
            this.listSelectButton = new System.Windows.Forms.Button();
            this.tagTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tagSelectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "All Items";
            // 
            // allItemsListBox
            // 
            this.allItemsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.allItemsListBox.FormattingEnabled = true;
            this.allItemsListBox.Location = new System.Drawing.Point(12, 25);
            this.allItemsListBox.Name = "allItemsListBox";
            this.allItemsListBox.Size = new System.Drawing.Size(250, 212);
            this.allItemsListBox.TabIndex = 1;
            // 
            // listSelectButton
            // 
            this.listSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listSelectButton.Location = new System.Drawing.Point(187, 249);
            this.listSelectButton.Name = "listSelectButton";
            this.listSelectButton.Size = new System.Drawing.Size(75, 23);
            this.listSelectButton.TabIndex = 2;
            this.listSelectButton.Text = "Select";
            this.listSelectButton.UseVisualStyleBackColor = true;
            this.listSelectButton.Click += new System.EventHandler(this.listSelectButton_Click);
            // 
            // tagTextbox
            // 
            this.tagTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagTextbox.Location = new System.Drawing.Point(12, 352);
            this.tagTextbox.Name = "tagTextbox";
            this.tagTextbox.Size = new System.Drawing.Size(250, 20);
            this.tagTextbox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 335);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Item Tag";
            // 
            // tagSelectButton
            // 
            this.tagSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tagSelectButton.Location = new System.Drawing.Point(187, 378);
            this.tagSelectButton.Name = "tagSelectButton";
            this.tagSelectButton.Size = new System.Drawing.Size(75, 23);
            this.tagSelectButton.TabIndex = 5;
            this.tagSelectButton.Text = "Select";
            this.tagSelectButton.UseVisualStyleBackColor = true;
            this.tagSelectButton.Click += new System.EventHandler(this.tagSelectButton_Click);
            // 
            // ItemSelectBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 412);
            this.Controls.Add(this.tagSelectButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tagTextbox);
            this.Controls.Add(this.listSelectButton);
            this.Controls.Add(this.allItemsListBox);
            this.Controls.Add(this.label1);
            this.Name = "ItemSelectBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox allItemsListBox;
        private System.Windows.Forms.Button listSelectButton;
        private System.Windows.Forms.TextBox tagTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button tagSelectButton;
    }
}