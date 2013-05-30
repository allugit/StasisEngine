using System;

namespace StasisGame.Components
{
    public enum ComponentType
    {
        Physics,
        PrimitivesRender,
        Rope,
        WorldItemRender,
        Item,
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
        Explosion,
        SkipFluidResolution,
        DestructibleGeometry,
        Debris,
        ParticleInfluence,
        DecalRender
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
