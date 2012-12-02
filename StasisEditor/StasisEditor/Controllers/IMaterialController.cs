using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public interface IMaterialController : IController
    {
        void setAutoUpdatePreview(bool status);
        bool getAutoUpdatePreview();
        void preview(Material material);
        void previewClosed();
        ReadOnlyCollection<Material> getMaterials(MaterialType type);
    }
}
