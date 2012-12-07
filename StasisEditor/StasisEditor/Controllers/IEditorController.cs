using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public interface IEditorController : IController
    {
        void resizeGraphicsDevice(int width, int height);

        float getScale();

        BindingList<TextureResource> getTextureResources();
        void addTextureResource(TextureResource resource);
        void removeTextureResource(TextureResource resource);

        void setActorToolbarEnabled(bool status);
    }
}
