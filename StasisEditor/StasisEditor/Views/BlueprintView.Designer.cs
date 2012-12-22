using StasisEditor.Views.Controls;
namespace StasisEditor.Views
{
    partial class BlueprintView
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
            this.addBlueprintButton = new System.Windows.Forms.Button();
            this.removeBlueprintButton = new System.Windows.Forms.Button();
            this.addScrapButton = new System.Windows.Forms.Button();
            this.removeScrapButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.defineShapeButton = new System.Windows.Forms.Button();
            this.arrangeScrapsButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.blueprintList = new StasisEditor.Views.Controls.RefreshingListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.scrapList = new StasisEditor.Views.Controls.RefreshingListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // addBlueprintButton
            // 
            this.addBlueprintButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addBlueprintButton.Location = new System.Drawing.Point(6, 339);
            this.addBlueprintButton.Name = "addBlueprintButton";
            this.addBlueprintButton.Size = new System.Drawing.Size(46, 23);
            this.addBlueprintButton.TabIndex = 4;
            this.addBlueprintButton.Text = "Add";
            this.addBlueprintButton.UseVisualStyleBackColor = true;
            this.addBlueprintButton.Click += new System.EventHandler(this.addBlueprintButton_Click);
            // 
            // removeBlueprintButton
            // 
            this.removeBlueprintButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeBlueprintButton.Enabled = false;
            this.removeBlueprintButton.Location = new System.Drawing.Point(58, 339);
            this.removeBlueprintButton.Name = "removeBlueprintButton";
            this.removeBlueprintButton.Size = new System.Drawing.Size(63, 23);
            this.removeBlueprintButton.TabIndex = 5;
            this.removeBlueprintButton.Text = "Remove";
            this.removeBlueprintButton.UseVisualStyleBackColor = true;
            // 
            // addScrapButton
            // 
            this.addScrapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addScrapButton.Enabled = false;
            this.addScrapButton.Location = new System.Drawing.Point(6, 339);
            this.addScrapButton.Name = "addScrapButton";
            this.addScrapButton.Size = new System.Drawing.Size(46, 23);
            this.addScrapButton.TabIndex = 6;
            this.addScrapButton.Text = "Add";
            this.addScrapButton.UseVisualStyleBackColor = true;
            this.addScrapButton.Click += new System.EventHandler(this.addScrapButton_Click);
            // 
            // removeScrapButton
            // 
            this.removeScrapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeScrapButton.Enabled = false;
            this.removeScrapButton.Location = new System.Drawing.Point(58, 339);
            this.removeScrapButton.Name = "removeScrapButton";
            this.removeScrapButton.Size = new System.Drawing.Size(63, 23);
            this.removeScrapButton.TabIndex = 7;
            this.removeScrapButton.Text = "Remove";
            this.removeScrapButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(571, 379);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(59, 23);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // defineShapeButton
            // 
            this.defineShapeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.defineShapeButton.Enabled = false;
            this.defineShapeButton.Location = new System.Drawing.Point(271, 339);
            this.defineShapeButton.Name = "defineShapeButton";
            this.defineShapeButton.Size = new System.Drawing.Size(115, 23);
            this.defineShapeButton.TabIndex = 9;
            this.defineShapeButton.Text = "Define Scrap Shape";
            this.defineShapeButton.UseVisualStyleBackColor = true;
            // 
            // arrangeScrapsButton
            // 
            this.arrangeScrapsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.arrangeScrapsButton.Enabled = false;
            this.arrangeScrapsButton.Location = new System.Drawing.Point(127, 339);
            this.arrangeScrapsButton.Name = "arrangeScrapsButton";
            this.arrangeScrapsButton.Size = new System.Drawing.Size(99, 23);
            this.arrangeScrapsButton.TabIndex = 10;
            this.arrangeScrapsButton.Text = "Arrange Scraps";
            this.arrangeScrapsButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.removeBlueprintButton);
            this.groupBox1.Controls.Add(this.arrangeScrapsButton);
            this.groupBox1.Controls.Add(this.blueprintList);
            this.groupBox1.Controls.Add(this.addBlueprintButton);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 369);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Blueprints";
            // 
            // blueprintList
            // 
            this.blueprintList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.blueprintList.FormattingEnabled = true;
            this.blueprintList.Location = new System.Drawing.Point(6, 15);
            this.blueprintList.Name = "blueprintList";
            this.blueprintList.Size = new System.Drawing.Size(220, 316);
            this.blueprintList.TabIndex = 0;
            this.blueprintList.SelectedValueChanged += new System.EventHandler(this.blueprintList_SelectedValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.scrapList);
            this.groupBox2.Controls.Add(this.addScrapButton);
            this.groupBox2.Controls.Add(this.defineShapeButton);
            this.groupBox2.Controls.Add(this.removeScrapButton);
            this.groupBox2.Location = new System.Drawing.Point(244, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 369);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scraps";
            // 
            // scrapList
            // 
            this.scrapList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scrapList.FormattingEnabled = true;
            this.scrapList.Location = new System.Drawing.Point(6, 15);
            this.scrapList.Name = "scrapList";
            this.scrapList.Size = new System.Drawing.Size(380, 316);
            this.scrapList.TabIndex = 2;
            this.scrapList.SelectedValueChanged += new System.EventHandler(this.scrapList_SelectedValueChanged);
            // 
            // BlueprintView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.saveButton);
            this.Name = "BlueprintView";
            this.Size = new System.Drawing.Size(641, 410);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RefreshingListBox blueprintList;
        private RefreshingListBox scrapList;
        private System.Windows.Forms.Button addBlueprintButton;
        private System.Windows.Forms.Button removeBlueprintButton;
        private System.Windows.Forms.Button addScrapButton;
        private System.Windows.Forms.Button removeScrapButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button defineShapeButton;
        private System.Windows.Forms.Button arrangeScrapsButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
