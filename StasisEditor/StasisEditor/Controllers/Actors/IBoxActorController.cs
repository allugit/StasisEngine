using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public interface IBoxActorController : IActorController
    {
        float getAngle();
        float getHalfWidth();
        float getHalfHeight();
        void setAngle(float angle);
        void setHalfWidth(float halfWidth);
        void setHalfHeight(float halfHeight);
    }
}
