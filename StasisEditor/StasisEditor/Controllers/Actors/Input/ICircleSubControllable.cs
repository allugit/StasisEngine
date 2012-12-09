using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    public interface ICircleSubControllable : ISubControllable
    {
        Vector2 getPosition();
        float getRadius();

        void setPosition(Vector2 position);
        void setRadius(float radius);
    }
}
