namespace StasisEditor.Views
{
    partial class CircuitsView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.saveCircuitsButton = new System.Windows.Forms.Button();
            this.circuitAddButton = new System.Windows.Forms.Button();
            this.circuitRemoveButton = new System.Windows.Forms.Button();
            this.circuitsList = new System.Windows.Forms.ListBox();
            this.circuitDisplay1 = new StasisEditor.Views.Controls.CircuitDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.saveCircuitsButton);
            this.splitContainer1.Panel1.Controls.Add(this.circuitAddButton);
            this.splitContainer1.Panel1.Controls.Add(this.circuitRemoveButton);
            this.splitContainer1.Panel1.Controls.Add(this.circuitsList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.circuitDisplay1);
            this.splitContainer1.Size = new System.Drawing.Size(674, 401);
            this.splitContainer1.SplitterDistance = 242;
            this.splitContainer1.TabIndex = 2;
            // 
            // saveCircuitsButton
            // 
            this.saveCircuitsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCircuitsButton.Location = new System.Drawing.Point(164, 374);
            this.saveCircuitsButton.Name = "saveCircuitsButton";
            this.saveCircuitsButton.Size = new System.Drawing.Size(75, 23);
            this.saveCircuitsButton.TabIndex = 3;
            this.saveCircuitsButton.Text = "Save";
            this.saveCircuitsButton.UseVisualStyleBackColor = true;
            // 
            // circuitAddButton
            // 
            this.circuitAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.circuitAddButton.Location = new System.Drawing.Point(3, 374);
            this.circuitAddButton.Name = "circuitAddButton";
            this.circuitAddButton.Size = new System.Drawing.Size(75, 23);
            this.circuitAddButton.TabIndex = 2;
            this.circuitAddButton.Text = "Add";
            this.circuitAddButton.UseVisualStyleBackColor = true;
            // 
            // circuitRemoveButton
            // 
            this.circuitRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.circuitRemoveButton.Location = new System.Drawing.Point(84, 374);
            this.circuitRemoveButton.Name = "circuitRemoveButton";
            this.circuitRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.circuitRemoveButton.TabIndex = 1;
            this.circuitRemoveButton.Text = "Remove";
            this.circuitRemoveButton.UseVisualStyleBackColor = true;
            // 
            // circuitsList
            // 
            this.circuitsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.circuitsList.FormattingEnabled = true;
            this.circuitsList.Location = new System.Drawing.Point(3, 3);
            this.circuitsList.Name = "circuitsList";
            this.circuitsList.Size = new System.Drawing.Size(236, 342);
            this.circuitsList.TabIndex = 0;
            this.circuitsList.SelectedValueChanged += new System.EventHandler(this.circuitsList_SelectedValueChanged);
            // 
            // circuitDisplay1
            // 
            this.circuitDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circuitDisplay1.Location = new System.Drawing.Point(0, 0);
            this.circuitDisplay1.Name = "circuitDisplay1";
            this.circuitDisplay1.Size = new System.Drawing.Size(428, 401);
            this.circuitDisplay1.TabIndex = 0;
            this.circuitDisplay1.Text = "circuitDisplay1";
            // 
            // CircuitsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CircuitsView";
            this.Size = new System.Drawing.Size(674, 401);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button saveCircuitsButton;
        private System.Windows.Forms.Button circuitAddButton;
        private System.Windows.Forms.Button circuitRemoveButton;
        private System.Windows.Forms.ListBox circuitsList;
        private Controls.CircuitDisplay circuitDisplay1;
    }
}
