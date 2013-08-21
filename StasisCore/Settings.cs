namespace StasisCore
{
    public class Settings
    {
        public const float BASE_SCALE = 35f;
    }

    public enum CollisionCategory
    {
        Player = 1,
        Rope = 2,
        StaticGeometry = 4,
        DynamicGeometry = 8,
        Wall = 16,
        Item = 32,
        Explosion = 64
    };

    public enum ActorType
    {
        Box,
        Circle,
        Terrain,
        Rope,
        Fluid,
        PlayerSpawn,
        Tree,
        Item,
        Circuit,
        Revolute,
        Prismatic,
        CollisionFilter,
        Goal,
        Decal,
        LevelTransition,
        Tooltip,
        Waypoint
    };

    public enum PlantType
    {
        Tree
    };

    public enum RopeTextureStyle
    {
        Random,
        Sequential
    };

    public enum LayerBlendType
    {
        Opaque,
        Overlay,
        Additive
    };

    public enum WorleyFeatureType
    {
        F1,
        F2,
        F2mF1
    };
}
