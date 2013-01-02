namespace StasisEditor.Views
{
    partial class EditorView
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
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuFileExit = new System.Windows.Forms.MenuItem();
            this.menuLevel = new System.Windows.Forms.MenuItem();
            this.menuLevelNew = new System.Windows.Forms.MenuItem();
            this.menuLevelLoad = new System.Windows.Forms.MenuItem();
            this.menuLevelSave = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuLevelClose = new System.Windows.Forms.MenuItem();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.editorTabControl = new System.Windows.Forms.TabControl();
            this.levelTab = new System.Windows.Forms.TabPage();
            this.levelView1 = new StasisEditor.Views.LevelView();
            this.materialsTab = new System.Windows.Forms.TabPage();
            this.materialView1 = new StasisEditor.Views.MaterialView();
            this.blueprintsTab = new System.Windows.Forms.TabPage();
            this.blueprintView1 = new StasisEditor.Views.BlueprintView();
            this.circuitsTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.saveCircuitsButton = new System.Windows.Forms.Button();
            this.circuitAddButton = new System.Windows.Forms.Button();
            this.circuitRemoveButton = new System.Windows.Forms.Button();
            this.circuitsList = new System.Windows.Forms.ListBox();
            this.circuitsView1 = new StasisEditor.Views.CircuitsView();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).BeginInit();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            this.editorTabControl.SuspendLayout();
            this.levelTab.SuspendLayout();
            this.materialsTab.SuspendLayout();
            this.blueprintsTab.SuspendLayout();
            this.circuitsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuLevel});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileExit});
            this.menuFile.Text = "File";
            // 
            // menuFileExit
            // 
            this.menuFileExit.Index = 0;
            this.menuFileExit.Text = "Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuLevel
            // 
            this.menuLevel.Index = 1;
            this.menuLevel.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuLevelNew,
            this.menuLevelLoad,
            this.menuLevelSave,
            this.menuItem2,
            this.menuLevelClose});
            this.menuLevel.Text = "Level";
            // 
            // menuLevelNew
            // 
            this.menuLevelNew.Index = 0;
            this.menuLevelNew.Text = "New";
            this.menuLevelNew.Click += new System.EventHandler(this.menuLevelNew_Click);
            // 
            // menuLevelLoad
            // 
            this.menuLevelLoad.Index = 1;
            this.menuLevelLoad.Text = "Load";
            // 
            // menuLevelSave
            // 
            this.menuLevelSave.Enabled = false;
            this.menuLevelSave.Index = 2;
            this.menuLevelSave.Text = "Save";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "-";
            // 
            // menuLevelClose
            // 
            this.menuLevelClose.Enabled = false;
            this.menuLevelClose.Index = 4;
            this.menuLevelClose.Text = "Close";
            this.menuLevelClose.Click += new System.EventHandler(this.menuLevelClose_Click);
            // 
            // mainSplit
            // 
            this.mainSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplit.Location = new System.Drawing.Point(0, 0);
            this.mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.BackColor = System.Drawing.SystemColors.Control;
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.mainSplit.Panel2.Controls.Add(this.editorTabControl);
            this.mainSplit.Size = new System.Drawing.Size(1004, 485);
            this.mainSplit.SplitterDistance = 261;
            this.mainSplit.TabIndex = 1;
            this.mainSplit.TabStop = false;
            // 
            // editorTabControl
            // 
            this.editorTabControl.Controls.Add(this.levelTab);
            this.editorTabControl.Controls.Add(this.materialsTab);
            this.editorTabControl.Controls.Add(this.blueprintsTab);
            this.editorTabControl.Controls.Add(this.circuitsTab);
            this.editorTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorTabControl.Location = new System.Drawing.Point(0, 0);
            this.editorTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.editorTabControl.Name = "editorTabControl";
            this.editorTabControl.SelectedIndex = 0;
            this.editorTabControl.Size = new System.Drawing.Size(737, 483);
            this.editorTabControl.TabIndex = 0;
            this.editorTabControl.SelectedIndexChanged += new System.EventHandler(this.editorTabControl_SelectedIndexChanged);
            // 
            // levelTab
            // 
            this.levelTab.BackColor = System.Drawing.SystemColors.ControlDark;
            this.levelTab.Controls.Add(this.levelView1);
            this.levelTab.Location = new System.Drawing.Point(4, 22);
            this.levelTab.Name = "levelTab";
            this.levelTab.Size = new System.Drawing.Size(729, 457);
            this.levelTab.TabIndex = 0;
            this.levelTab.Text = "Level";
            // 
            // levelView1
            // 
            this.levelView1.active = true;
            this.levelView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelView1.Location = new System.Drawing.Point(0, 0);
            this.levelView1.Margin = new System.Windows.Forms.Padding(0);
            this.levelView1.Name = "levelView1";
            this.levelView1.Size = new System.Drawing.Size(729, 457);
            this.levelView1.TabIndex = 0;
            // 
            // materialsTab
            // 
            this.materialsTab.Controls.Add(this.materialView1);
            this.materialsTab.Location = new System.Drawing.Point(4, 22);
            this.materialsTab.Margin = new System.Windows.Forms.Padding(0);
            this.materialsTab.Name = "materialsTab";
            this.materialsTab.Size = new System.Drawing.Size(729, 457);
            this.materialsTab.TabIndex = 1;
            this.materialsTab.Text = "Materials";
            this.materialsTab.UseVisualStyleBackColor = true;
            // 
            // materialView1
            // 
            this.materialView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialView1.Location = new System.Drawing.Point(0, 0);
            this.materialView1.Margin = new System.Windows.Forms.Padding(0);
            this.materialView1.Name = "materialView1";
            this.materialView1.Size = new System.Drawing.Size(729, 478);
            this.materialView1.TabIndex = 0;
            // 
            // blueprintsTab
            // 
            this.blueprintsTab.Controls.Add(this.blueprintView1);
            this.blueprintsTab.Location = new System.Drawing.Point(4, 22);
            this.blueprintsTab.Margin = new System.Windows.Forms.Padding(0);
            this.blueprintsTab.Name = "blueprintsTab";
            this.blueprintsTab.Size = new System.Drawing.Size(729, 457);
            this.blueprintsTab.TabIndex = 2;
            this.blueprintsTab.Text = "Blueprints";
            this.blueprintsTab.UseVisualStyleBackColor = true;
            // 
            // blueprintView1
            // 
            this.blueprintView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueprintView1.editBlueprintScrapShapeView = null;
            this.blueprintView1.editBlueprintSocketsView = null;
            this.blueprintView1.Location = new System.Drawing.Point(0, 0);
            this.blueprintView1.Margin = new System.Windows.Forms.Padding(0);
            this.blueprintView1.Name = "blueprintView1";
            this.blueprintView1.Size = new System.Drawing.Size(729, 478);
            this.blueprintView1.TabIndex = 0;
            // 
            // circuitsTab
            // 
            this.circuitsTab.Controls.Add(this.splitContainer1);
            this.circuitsTab.Location = new System.Drawing.Point(4, 22);
            this.circuitsTab.Name = "circuitsTab";
            this.circuitsTab.Padding = new System.Windows.Forms.Padding(3);
            this.circuitsTab.Size = new System.Drawing.Size(729, 457);
            this.circuitsTab.TabIndex = 3;
            this.circuitsTab.Text = "Circuits";
            this.circuitsTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
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
            this.splitContainer1.Panel2.Controls.Add(this.circuitsView1);
            this.splitContainer1.Size = new System.Drawing.Size(723, 472);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.TabIndex = 1;
            // 
            // saveCircuitsButton
            // 
            this.saveCircuitsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCircuitsButton.Location = new System.Drawing.Point(182, 445);
            this.saveCircuitsButton.Name = "saveCircuitsButton";
            this.saveCircuitsButton.Size = new System.Drawing.Size(75, 23);
            this.saveCircuitsButton.TabIndex = 3;
            this.saveCircuitsButton.Text = "Save";
            this.saveCircuitsButton.UseVisualStyleBackColor = true;
            // 
            // circuitAddButton
            // 
            this.circuitAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.circuitAddButton.Location = new System.Drawing.Point(3, 445);
            this.circuitAddButton.Name = "circuitAddButton";
            this.circuitAddButton.Size = new System.Drawing.Size(75, 23);
            this.circuitAddButton.TabIndex = 2;
            this.circuitAddButton.Text = "Add";
            this.circuitAddButton.UseVisualStyleBackColor = true;
            // 
            // circuitRemoveButton
            // 
            this.circuitRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.circuitRemoveButton.Location = new System.Drawing.Point(84, 445);
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
            this.circuitsList.Size = new System.Drawing.Size(254, 420);
            this.circuitsList.TabIndex = 0;
            // 
            // circuitsView1
            // 
            this.circuitsView1.active = false;
            this.circuitsView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circuitsView1.Location = new System.Drawing.Point(0, 0);
            this.circuitsView1.Name = "circuitsView1";
            this.circuitsView1.Size = new System.Drawing.Size(459, 472);
            this.circuitsView1.TabIndex = 0;
            this.circuitsView1.Text = "circuitsView";
            // 
            // EditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 485);
            this.Controls.Add(this.mainSplit);
            this.Menu = this.mainMenu1;
            this.Name = "EditorView";
            this.Text = "Stasis Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditorForm_Closed);
            this.mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).EndInit();
            this.mainSplit.ResumeLayout(false);
            this.editorTabControl.ResumeLayout(false);
            this.levelTab.ResumeLayout(false);
            this.materialsTab.ResumeLayout(false);
            this.blueprintsTab.ResumeLayout(false);
            this.circuitsTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplit;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuFile;
        private System.Windows.Forms.MenuItem menuFileExit;
        private System.Windows.Forms.MenuItem menuLevel;
        private System.Windows.Forms.MenuItem menuLevelNew;
        private System.Windows.Forms.MenuItem menuLevelLoad;
        private System.Windows.Forms.MenuItem menuLevelSave;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuLevelClose;
        private System.Windows.Forms.TabControl editorTabControl;
        private System.Windows.Forms.TabPage levelTab;
        private System.Windows.Forms.TabPage materialsTab;
        private MaterialView materialView1;
        private LevelView levelView1;
        private System.Windows.Forms.TabPage blueprintsTab;
        private BlueprintView blueprintView1;
        private System.Windows.Forms.TabPage circuitsTab;
        private CircuitsView circuitsView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox circuitsList;
        private System.Windows.Forms.Button circuitAddButton;
        private System.Windows.Forms.Button circuitRemoveButton;
        private System.Windows.Forms.Button saveCircuitsButton;

    }
}