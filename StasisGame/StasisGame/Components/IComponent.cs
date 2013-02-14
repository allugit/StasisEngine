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
        CharacterRender,
        Player,
        CharacterMovement
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
