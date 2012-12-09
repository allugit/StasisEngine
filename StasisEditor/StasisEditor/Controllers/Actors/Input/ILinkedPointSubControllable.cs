using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public interface ILinkedPointSubControllable : ISubControllable
    {
        void setNewLinkedPointSubControllerHead(LinkedPointSubController newHead);
    }
}
