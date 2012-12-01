using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public interface IMaterialView : IBaseView
    {
        DialogResult ShowDialog();
        void setController(MaterialController controller);
    }
}
