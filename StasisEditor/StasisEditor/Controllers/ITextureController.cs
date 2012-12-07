using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public interface ITextureController : IController
    {
        void viewClosed();
        BindingList<TextureResource> getTextureResources();
        void addTextureResources(string[] fileNames);
        void removeTextureResource(TextureResource resource);
        void removeTextureResource(List<TextureResource> resources);
        void relocateTextureResource(TextureResource resource);
    }
}
