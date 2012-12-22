namespace StasisEditor.Views.Controls
{
    partial class EditBlueprintSocketsButton
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
            this.editSocketsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Blueprint Sockets (final scrap positions)";
            // 
            // editSocketsButton
            // 
            this.editSocketsButton.Location = new System.Drawing.Point(0, 16);
            this.editSocketsButton.Margin = new System.Windows.Forms.Padding(0);
            this.editSocketsButton.Name = "editSocketsButton";
            this.editSocketsButton.Size = new System.Drawing.Size(89, 23);
            this.editSocketsButton.TabIndex = 1;
            this.editSocketsButton.Text = "Edit Sockets";
            this.editSocketsButton.UseVisualStyleBackColor = true;
            this.editSocketsButton.Click += new System.EventHandler(this.editSocketsButton_Click);
            // 
            // EditBlueprintScrapSocketsButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editSocketsButton);
            this.Controls.Add(this.label1);
            this.Name = "EditBlueprintScrapSocketsButton";
            this.Size = new System.Drawing.Size(351, 62);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button editSocketsButton;
    }
}
