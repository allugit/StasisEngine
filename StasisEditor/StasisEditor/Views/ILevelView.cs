using System;
using System.Collections.Generic;
using StasisEditor.Controllers;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace StasisEditor.Views
{
    public interface ILevelView : IBaseView
    {
        void setController(ILevelController controller);
        int getWidth();
        int getHeight();
        void handleXNADraw();
    }
}
