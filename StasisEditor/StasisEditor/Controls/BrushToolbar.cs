using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StasisEditor.Controls
{
    public class BrushToolbar : ToolStrip
    {
        private ToolStripButton boxButton;
        private ToolStripButton circleButton;
        private ToolStripButton objectSpawnerButton;
        private ToolStripButton slidingBoxButton;
        private ToolStripButton pressurePlateButton;
        private ToolStripButton timerButton;
        private ToolStripButton playerSpawnButton;
        private ToolStripButton goalButton;
        private ToolStripButton itemsButton;
        private ToolStripButton plantsButton;
        private ToolStripButton fluidButton;
        private ToolStripButton ropeButton;
        private ToolStripButton edgeButton;
    
        public BrushToolbar()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrushToolbar));
            this.boxButton = new System.Windows.Forms.ToolStripButton();
            this.circleButton = new System.Windows.Forms.ToolStripButton();
            this.edgeButton = new System.Windows.Forms.ToolStripButton();
            this.objectSpawnerButton = new System.Windows.Forms.ToolStripButton();
            this.slidingBoxButton = new System.Windows.Forms.ToolStripButton();
            this.pressurePlateButton = new System.Windows.Forms.ToolStripButton();
            this.timerButton = new System.Windows.Forms.ToolStripButton();
            this.playerSpawnButton = new System.Windows.Forms.ToolStripButton();
            this.goalButton = new System.Windows.Forms.ToolStripButton();
            this.itemsButton = new System.Windows.Forms.ToolStripButton();
            this.plantsButton = new System.Windows.Forms.ToolStripButton();
            this.fluidButton = new System.Windows.Forms.ToolStripButton();
            this.ropeButton = new System.Windows.Forms.ToolStripButton();
            this.SuspendLayout();
            // 
            // boxButton
            // 
            this.boxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.boxButton.Image = ((System.Drawing.Image)(resources.GetObject("boxButton.Image")));
            this.boxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.boxButton.Name = "boxButton";
            this.boxButton.Size = new System.Drawing.Size(23, 22);
            this.boxButton.Text = "Box";
            // 
            // circleButton
            // 
            this.circleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.circleButton.Image = ((System.Drawing.Image)(resources.GetObject("circleButton.Image")));
            this.circleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.circleButton.Name = "circleButton";
            this.circleButton.Size = new System.Drawing.Size(23, 22);
            this.circleButton.Text = "Circle";
            // 
            // edgeButton
            // 
            this.edgeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edgeButton.Image = ((System.Drawing.Image)(resources.GetObject("edgeButton.Image")));
            this.edgeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edgeButton.Name = "edgeButton";
            this.edgeButton.Size = new System.Drawing.Size(23, 22);
            this.edgeButton.Text = "Edge";
            // 
            // objectSpawnerButton
            // 
            this.objectSpawnerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.objectSpawnerButton.Image = ((System.Drawing.Image)(resources.GetObject("objectSpawnerButton.Image")));
            this.objectSpawnerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.objectSpawnerButton.Name = "objectSpawnerButton";
            this.objectSpawnerButton.Size = new System.Drawing.Size(23, 20);
            this.objectSpawnerButton.Text = "Object Spawner (Signal Receiver)";
            // 
            // slidingBoxButton
            // 
            this.slidingBoxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.slidingBoxButton.Image = ((System.Drawing.Image)(resources.GetObject("slidingBoxButton.Image")));
            this.slidingBoxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.slidingBoxButton.Name = "slidingBoxButton";
            this.slidingBoxButton.Size = new System.Drawing.Size(23, 20);
            this.slidingBoxButton.Text = "Sliding Box (Signal Receiver)";
            // 
            // pressurePlateButton
            // 
            this.pressurePlateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pressurePlateButton.Image = ((System.Drawing.Image)(resources.GetObject("pressurePlateButton.Image")));
            this.pressurePlateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pressurePlateButton.Name = "pressurePlateButton";
            this.pressurePlateButton.Size = new System.Drawing.Size(23, 23);
            this.pressurePlateButton.Text = "Pressure Plate (Signal Transmitter)";
            // 
            // timerButton
            // 
            this.timerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.timerButton.Image = ((System.Drawing.Image)(resources.GetObject("timerButton.Image")));
            this.timerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.timerButton.Name = "timerButton";
            this.timerButton.Size = new System.Drawing.Size(23, 23);
            this.timerButton.Text = "Timer (Signal Transmitter)";
            // 
            // playerSpawnButton
            // 
            this.playerSpawnButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.playerSpawnButton.Image = ((System.Drawing.Image)(resources.GetObject("playerSpawnButton.Image")));
            this.playerSpawnButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playerSpawnButton.Name = "playerSpawnButton";
            this.playerSpawnButton.Size = new System.Drawing.Size(23, 23);
            this.playerSpawnButton.Text = "Player Spawn";
            // 
            // goalButton
            // 
            this.goalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goalButton.Image = ((System.Drawing.Image)(resources.GetObject("goalButton.Image")));
            this.goalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goalButton.Name = "goalButton";
            this.goalButton.Size = new System.Drawing.Size(23, 23);
            this.goalButton.Text = "Goal";
            // 
            // itemsButton
            // 
            this.itemsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemsButton.Image = ((System.Drawing.Image)(resources.GetObject("itemsButton.Image")));
            this.itemsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemsButton.Name = "itemsButton";
            this.itemsButton.Size = new System.Drawing.Size(23, 23);
            this.itemsButton.Text = "Items";
            // 
            // plantsButton
            // 
            this.plantsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.plantsButton.Image = ((System.Drawing.Image)(resources.GetObject("plantsButton.Image")));
            this.plantsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.plantsButton.Name = "plantsButton";
            this.plantsButton.Size = new System.Drawing.Size(23, 23);
            this.plantsButton.Text = "Plants";
            // 
            // fluidButton
            // 
            this.fluidButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fluidButton.Image = ((System.Drawing.Image)(resources.GetObject("fluidButton.Image")));
            this.fluidButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fluidButton.Name = "fluidButton";
            this.fluidButton.Size = new System.Drawing.Size(23, 23);
            this.fluidButton.Text = "Fluid";
            // 
            // ropeButton
            // 
            this.ropeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ropeButton.Image = ((System.Drawing.Image)(resources.GetObject("ropeButton.Image")));
            this.ropeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ropeButton.Name = "ropeButton";
            this.ropeButton.Size = new System.Drawing.Size(23, 23);
            this.ropeButton.Text = "Rope";
            // 
            // BrushToolbar
            // 
            this.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.edgeButton,
            this.boxButton,
            this.circleButton,
            this.objectSpawnerButton,
            this.slidingBoxButton,
            this.pressurePlateButton,
            this.timerButton,
            this.ropeButton,
            this.itemsButton,
            this.plantsButton,
            this.fluidButton,
            this.playerSpawnButton,
            this.goalButton});
            this.ResumeLayout(false);

        }
    }
}
