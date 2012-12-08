using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public interface IGeneralSubControllable : ISubControllable
    {
        Vector2 getPosition();
        void setPosition(Vector2 position);
    }
}
