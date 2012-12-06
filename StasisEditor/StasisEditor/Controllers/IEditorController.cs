using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public interface IEditorController : IController
    {
        BindingList<TextureResource> getTextureResources();
        void resizeGraphicsDevice(int width, int height);
        void addTextureResource(TextureResource resource);
        void removeTextureResource(TextureResource resource);
        float getScale();
    }
}
