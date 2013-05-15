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
        Inventory,
        Toolbar,
        Aim,
        WorldPosition,
        IgnoreRopeRaycast,
        RopeGrab,
        RegionGoal,
        Wall,
        Dynamite,
        Explosion
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
