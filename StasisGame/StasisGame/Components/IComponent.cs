using System;

namespace StasisGame.Components
{
    public enum ComponentType
    {
        Physics,
        BodyRender,
        RopePhysics,
        RopeRender
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
