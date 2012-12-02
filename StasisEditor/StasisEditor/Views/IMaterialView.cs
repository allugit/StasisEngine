using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisCore.Models;

namespace StasisEditor.Views
{
    public interface IMaterialView : IBaseView
    {
        DialogResult ShowDialog();
        void Show();
        bool Focus();
        void setController(IMaterialController controller);
        void setChangesMade(bool status);
        void setAutoUpdatePreview(bool status);
        Material getSelectedMaterial();
    }
}
