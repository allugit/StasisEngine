using System;
using System.Collections.Generic;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public interface ITextureView : IBaseView
    {
        void setController(ITextureController controller);
        void refreshGrid();
        void Show();
        bool Focus();
    }
}
