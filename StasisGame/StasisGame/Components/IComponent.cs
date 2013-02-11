using System;

namespace StasisGame.Components
{
    public enum ComponentType
    {
        Physics
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
