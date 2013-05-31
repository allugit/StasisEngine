namespace StasisEditor.Views.Controls
{
    partial class EditPathsForm
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
            this.debugFolderPath = new System.Windows.Forms.TextBox();
            this.debugFolderBrowseButton = new System.Windows.Forms.Button();
            this.releaseFolderBrowseButton = new System.Windows.Forms.Button();
            this.releaseFolderPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gameGroupBox = new System.Windows.Forms.GroupBox();
            this.resourcesGroupBox = new System.Windows.Forms.GroupBox();
            this.resourcesSourceBrowseButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.resourcesSourcePath = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.gameGroupBox.SuspendLayout();
            this.resourcesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Debug Folder";
            // 
            // debugFolderPath
            // 
            this.debugFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debugFolderPath.Location = new System.Drawing.Point(6, 32);
            this.debugFolderPath.Name = "debugFolderPath";
            this.debugFolderPath.Size = new System.Drawing.Size(340, 20);
            this.debugFolderPath.TabIndex = 1;
            // 
            // debugFolderBrowseButton
            // 
            this.debugFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.debugFolderBrowseButton.Location = new System.Drawing.Point(352, 30);
            this.debugFolderBrowseButton.Name = "debugFolderBrowseButton";
            this.debugFolderBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.debugFolderBrowseButton.TabIndex = 2;
            this.debugFolderBrowseButton.Text = "Browse...";
            this.debugFolderBrowseButton.UseVisualStyleBackColor = true;
            this.debugFolderBrowseButton.Click += new System.EventHandler(this.debugFolderBrowseButton_Click);
            // 
            // releaseFolderBrowseButton
            // 
            this.releaseFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.releaseFolderBrowseButton.Location = new System.Drawing.Point(352, 80);
            this.releaseFolderBrowseButton.Name = "releaseFolderBrowseButton";
            this.releaseFolderBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.releaseFolderBrowseButton.TabIndex = 5;
            this.releaseFolderBrowseButton.Text = "Browse...";
            this.releaseFolderBrowseButton.UseVisualStyleBackColor = true;
            this.releaseFolderBrowseButton.Click += new System.EventHandler(this.releaseFolderBrowseButton_Click);
            // 
            // releaseFolderPath
            // 
            this.releaseFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.releaseFolderPath.Location = new System.Drawing.Point(6, 82);
            this.releaseFolderPath.Name = "releaseFolderPath";
            this.releaseFolderPath.Size = new System.Drawing.Size(340, 20);
            this.releaseFolderPath.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Release Folder";
            // 
            // gameGroupBox
            // 
            this.gameGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gameGroupBox.Controls.Add(this.label1);
            this.gameGroupBox.Controls.Add(this.releaseFolderBrowseButton);
            this.gameGroupBox.Controls.Add(this.debugFolderPath);
            this.gameGroupBox.Controls.Add(this.releaseFolderPath);
            this.gameGroupBox.Controls.Add(this.debugFolderBrowseButton);
            this.gameGroupBox.Controls.Add(this.label2);
            this.gameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.gameGroupBox.Name = "gameGroupBox";
            this.gameGroupBox.Size = new System.Drawing.Size(433, 110);
            this.gameGroupBox.TabIndex = 6;
            this.gameGroupBox.TabStop = false;
            this.gameGroupBox.Text = "Game Paths";
            // 
            // resourcesGroupBox
            // 
            this.resourcesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourcesGroupBox.Controls.Add(this.resourcesSourceBrowseButton);
            this.resourcesGroupBox.Controls.Add(this.label3);
            this.resourcesGroupBox.Controls.Add(this.resourcesSourcePath);
            this.resourcesGroupBox.Location = new System.Drawing.Point(12, 147);
            this.resourcesGroupBox.Name = "resourcesGroupBox";
            this.resourcesGroupBox.Size = new System.Drawing.Size(433, 59);
            this.resourcesGroupBox.TabIndex = 7;
            this.resourcesGroupBox.TabStop = false;
            this.resourcesGroupBox.Text = "Resources";
            // 
            // resourcesSourceBrowseButton
            // 
            this.resourcesSourceBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resourcesSourceBrowseButton.Location = new System.Drawing.Point(352, 30);
            this.resourcesSourceBrowseButton.Name = "resourcesSourceBrowseButton";
            this.resourcesSourceBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.resourcesSourceBrowseButton.TabIndex = 8;
            this.resourcesSourceBrowseButton.Text = "Browse...";
            this.resourcesSourceBrowseButton.UseVisualStyleBackColor = true;
            this.resourcesSourceBrowseButton.Click += new System.EventHandler(this.resourcesSourceBrowseButton_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Source Folder (folder containing \"data\")";
            // 
            // resourcesSourcePath
            // 
            this.resourcesSourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resourcesSourcePath.Location = new System.Drawing.Point(6, 32);
            this.resourcesSourcePath.Name = "resourcesSourcePath";
            this.resourcesSourcePath.Size = new System.Drawing.Size(340, 20);
            this.resourcesSourcePath.TabIndex = 7;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(289, 247);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(370, 247);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // EditPathsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 282);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.resourcesGroupBox);
            this.Controls.Add(this.gameGroupBox);
            this.Name = "EditPathsForm";
            this.Text = "Edit Paths";
            this.gameGroupBox.ResumeLayout(false);
            this.gameGroupBox.PerformLayout();
            this.resourcesGroupBox.ResumeLayout(false);
            this.resourcesGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox debugFolderPath;
        private System.Windows.Forms.Button debugFolderBrowseButton;
        private System.Windows.Forms.Button releaseFolderBrowseButton;
        private System.Windows.Forms.TextBox releaseFolderPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gameGroupBox;
        private System.Windows.Forms.GroupBox resourcesGroupBox;
        private System.Windows.Forms.Button resourcesSourceBrowseButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox resourcesSourcePath;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}