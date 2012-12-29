namespace StasisEditor.Views.Controls
{
    partial class EditBlueprintSocketsView
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.clearSocketsButton = new System.Windows.Forms.Button();
            this.resetPositionsButton = new System.Windows.Forms.Button();
            this.editBlueprintSocketsGraphics = new StasisEditor.Views.Controls.EditBlueprintSocketsGraphics();
            this.okayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(612, 427);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // clearSocketsButton
            // 
            this.clearSocketsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearSocketsButton.Location = new System.Drawing.Point(12, 428);
            this.clearSocketsButton.Name = "clearSocketsButton";
            this.clearSocketsButton.Size = new System.Drawing.Size(96, 23);
            this.clearSocketsButton.TabIndex = 4;
            this.clearSocketsButton.Text = "Clear Sockets";
            this.clearSocketsButton.UseVisualStyleBackColor = true;
            this.clearSocketsButton.Click += new System.EventHandler(this.clearSocketsButton_Click);
            // 
            // resetPositionsButton
            // 
            this.resetPositionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetPositionsButton.Location = new System.Drawing.Point(115, 427);
            this.resetPositionsButton.Name = "resetPositionsButton";
            this.resetPositionsButton.Size = new System.Drawing.Size(103, 23);
            this.resetPositionsButton.TabIndex = 5;
            this.resetPositionsButton.Text = "Reset Positions";
            this.resetPositionsButton.UseVisualStyleBackColor = true;
            this.resetPositionsButton.Click += new System.EventHandler(this.resetPositionsButton_Click);
            // 
            // editBlueprintSocketsGraphics
            // 
            this.editBlueprintSocketsGraphics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editBlueprintSocketsGraphics.blueprint = null;
            this.editBlueprintSocketsGraphics.Location = new System.Drawing.Point(12, 12);
            this.editBlueprintSocketsGraphics.Name = "editBlueprintSocketsGraphics";
            this.editBlueprintSocketsGraphics.Size = new System.Drawing.Size(675, 409);
            this.editBlueprintSocketsGraphics.TabIndex = 3;
            this.editBlueprintSocketsGraphics.Text = "editBlueprintSocketsGraphics";
            // 
            // okayButton
            // 
            this.okayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okayButton.Location = new System.Drawing.Point(531, 427);
            this.okayButton.Name = "okayButton";
            this.okayButton.Size = new System.Drawing.Size(75, 23);
            this.okayButton.TabIndex = 6;
            this.okayButton.Text = "OK";
            this.okayButton.UseVisualStyleBackColor = true;
            this.okayButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // EditBlueprintSocketsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 463);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.resetPositionsButton);
            this.Controls.Add(this.clearSocketsButton);
            this.Controls.Add(this.editBlueprintSocketsGraphics);
            this.Controls.Add(this.cancelButton);
            this.Name = "EditBlueprintSocketsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Sockets";
            this.Load += new System.EventHandler(this.EditBlueprintSocketsView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private EditBlueprintSocketsGraphics editBlueprintSocketsGraphics;
        private System.Windows.Forms.Button clearSocketsButton;
        private System.Windows.Forms.Button resetPositionsButton;
        private System.Windows.Forms.Button okayButton;
    }
}