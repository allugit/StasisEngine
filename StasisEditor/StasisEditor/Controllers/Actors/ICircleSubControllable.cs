using System;
using System.Collections.Generic;

namespace StasisEditor.Controllers.Actors
{
    public interface ICircleSubControllable : ISubControllable
    {
        float getRadius();
        void setRadius(float radius);
    }
}
