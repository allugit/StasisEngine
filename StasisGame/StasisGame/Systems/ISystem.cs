using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.Systems
{
    public enum SystemType
    {
        Render,
        Input,
        Physics,
        Fluid,
        Player,
        CharacterMovement,
        Camera
    };

    public interface ISystem
    {
        void update();
        SystemType systemType { get; }
        int defaultPriority { get; }
    }
}
