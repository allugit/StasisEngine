using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StasisEditor.Levels
{
    public class LevelSurface : PictureBox
    {
        public Level level;

        public LevelSurface(Level level)
            : base()
        {
            this.level = level;

            Width = level.editor.levelContainer.Width;
            Height = level.editor.levelContainer.Height;
            Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
        }
    }
}
