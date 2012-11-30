using System;
using System.Collections.Generic;
using StasisEditor.Controllers;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace StasisEditor.Views
{
    public interface ILevelView : IBaseView
    {
        void setController(EditorController controller);
        int getWidth();
        int getHeight();
        void handleXNADraw();
    }
}
