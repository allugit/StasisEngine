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
        CharacterMovement,
        BodyFocusPoint,
        Tree,
        IgnoreTreeCollision,
        GroundBody,
        EditorId,
        Revolute,
        Prismatic,
        GateOutput,
        Circuit,
        Inventory
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
