using System;
using System.Collections.Generic;
using StasisEditor.Controller;
using Microsoft.Xna.Framework;

namespace StasisEditor.View
{
    public interface ILevelView
    {
        void setController(LevelController controller);
        void setSize(System.Drawing.Point size);
        void setSize(int width, int height);
        int getWidth();
        int getHeight();
        void handleXNADraw();
    }
}
