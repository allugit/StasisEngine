using System;

namespace StasisGame.Components
{
    public enum ComponentType
    {
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
