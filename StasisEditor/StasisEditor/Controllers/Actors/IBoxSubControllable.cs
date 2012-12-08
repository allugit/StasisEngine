using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public interface IBoxSubControllable : ISubControllable
    {
        Vector2 getPosition();
        float getAngle();
        float getHalfWidth();
        float getHalfHeight();

        void setPosition(Vector2 position);
        void setAngle(float angle);
        void setHalfWidth(float halfWidth);
        void setHalfHeight(float halfHeight);
    }
}
