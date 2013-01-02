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
            this.circuitCreateButton = new System.Windows.Forms.Button();
            this.circuitDeleteButton = new System.Windows.Forms.Button();
            this.circuitsList = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gateAddButton = new System.Windows.Forms.Button();
            this.saveCircuitsButton = new System.Windows.Forms.Button();
            this.circuitDisplay1 = new StasisEditor.Views.Controls.CircuitDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.circuitCreateButton);
            this.splitContainer1.Panel1.Controls.Add(this.circuitDeleteButton);
            this.splitContainer1.Panel1.Controls.Add(this.circuitsList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.circuitDisplay1);
            this.splitContainer1.Size = new System.Drawing.Size(674, 401);
            this.splitContainer1.SplitterDistance = 242;
            this.splitContainer1.TabIndex = 2;
            // 
            // circuitCreateButton
            // 
            this.circuitCreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.circuitCreateButton.Location = new System.Drawing.Point(3, 374);
            this.circuitCreateButton.Name = "circuitCreateButton";
            this.circuitCreateButton.Size = new System.Drawing.Size(75, 23);
            this.circuitCreateButton.TabIndex = 2;
            this.circuitCreateButton.Text = "Create";
            this.circuitCreateButton.UseVisualStyleBackColor = true;
            this.circuitCreateButton.Click += new System.EventHandler(this.circuitCreateButton_Click);
            // 
            // circuitDeleteButton
            // 
            this.circuitDeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.circuitDeleteButton.Enabled = false;
            this.circuitDeleteButton.Location = new System.Drawing.Point(84, 374);
            this.circuitDeleteButton.Name = "circuitDeleteButton";
            this.circuitDeleteButton.Size = new System.Drawing.Size(75, 23);
            this.circuitDeleteButton.TabIndex = 1;
            this.circuitDeleteButton.Text = "Delete";
            this.circuitDeleteButton.UseVisualStyleBackColor = true;
            this.circuitDeleteButton.Click += new System.EventHandler(this.circuitDeleteButton_Click);
            // 
            // circuitsList
            // 
            this.circuitsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.circuitsList.FormattingEnabled = true;
            this.circuitsList.IntegralHeight = false;
            this.circuitsList.Location = new System.Drawing.Point(3, 3);
            this.circuitsList.Name = "circuitsList";
            this.circuitsList.Size = new System.Drawing.Size(236, 365);
            this.circuitsList.TabIndex = 0;
            this.circuitsList.SelectedValueChanged += new System.EventHandler(this.circuitsList_SelectedValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gateAddButton);
            this.panel1.Controls.Add(this.saveCircuitsButton);
            this.panel1.Location = new System.Drawing.Point(0, 368);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 33);
            this.panel1.TabIndex = 1;
            // 
            // gateAddButton
            // 
            this.gateAddButton.Enabled = false;
            this.gateAddButton.Location = new System.Drawing.Point(3, 6);
            this.gateAddButton.Name = "gateAddButton";
            this.gateAddButton.Size = new System.Drawing.Size(75, 23);
            this.gateAddButton.TabIndex = 5;
            this.gateAddButton.Text = "Add Gate";
            this.gateAddButton.UseVisualStyleBackColor = true;
            this.gateAddButton.Click += new System.EventHandler(this.gateAddButton_Click);
            // 
            // saveCircuitsButton
            // 
            this.saveCircuitsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCircuitsButton.Location = new System.Drawing.Point(324, 6);
            this.saveCircuitsButton.Name = "saveCircuitsButton";
            this.saveCircuitsButton.Size = new System.Drawing.Size(101, 23);
            this.saveCircuitsButton.TabIndex = 3;
            this.saveCircuitsButton.Text = "Save Circuits";
            this.saveCircuitsButton.UseVisualStyleBackColor = true;
            // 
            // circuitDisplay1
            // 
            this.circuitDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.circuitDisplay1.Location = new System.Drawing.Point(0, 0);
            this.circuitDisplay1.Name = "circuitDisplay1";
            this.circuitDisplay1.Size = new System.Drawing.Size(428, 368);
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
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button saveCircuitsButton;
        private System.Windows.Forms.Button circuitCreateButton;
        private System.Windows.Forms.Button circuitDeleteButton;
        private System.Windows.Forms.ListBox circuitsList;
        private Controls.CircuitDisplay circuitDisplay1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button gateAddButton;
    }
}
