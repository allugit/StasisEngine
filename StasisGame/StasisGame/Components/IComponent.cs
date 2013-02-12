using System;

namespace StasisGame.Components
{
    public enum ComponentType
    {
        Physics,
        BodyRender
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
