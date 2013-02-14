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
        Item,
        Input,
        CharacterRender
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
