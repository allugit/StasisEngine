using System;
using System.Collections.Generic;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    public interface ITextureController : IController
    {
        void viewClosed();
        void createNewTextureResources(List<TemporaryTextureResource> list);
    }
}
