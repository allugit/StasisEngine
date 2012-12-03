using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    public interface ITextureController : IController
    {
        void viewClosed();
        void createNewTextureResources(List<TemporaryTextureResource> list);
        ReadOnlyCollection<TextureResource> getTextureResources();
        void removeTextureResource(TextureResource resource);
        void removeTextureResource(List<TextureResource> resources);
    }
}
