using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Controls
{
    public class LevelSettings : PropertyGrid
    {
        private Level _level;

        public LevelSettings(Level level, int width, int height) : base()
        {
            _level = level;
            Width = width;
            Height = height;
            SelectedObject = _level;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LevelSettings
            // 
            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.ResumeLayout(false);
        }
    }
}
