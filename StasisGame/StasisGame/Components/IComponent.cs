using System;

namespace StasisGame.Components
{
    public enum ComponentType
    {
        Physics,
        PrimitivesRender,
        Rope,
        Item,
        CharacterRender,
        CharacterMovement,
        BodyFocusPoint,
        Tree,
        IgnoreTreeCollision,
        GroundBody,
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
        Metamer,
        FollowMetamer,
        Tooltip,
        LevelTransition,
        Waypoints,
        AIWanderBehavior,
        CharacterDialogue,
        InDialogue
    };

    public interface IComponent
    {
        ComponentType componentType { get; }
    }
}
