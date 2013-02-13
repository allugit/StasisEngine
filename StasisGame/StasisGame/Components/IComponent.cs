using System;

namespace StasisGame.Components
{
    public enum ComponentType
    {
        Physics,
        BodyRender,
        RopePhysics,
        RopeRender,
        WorldItemRender,
        Item
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
