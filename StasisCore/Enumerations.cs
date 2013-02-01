namespace StasisCore
{
    public enum ActorType
    {
        Box,
        Circle,
        MovingPlatform,
        PressurePlate,
        Terrain,
        Rope,
        Fluid,
        PlayerSpawn,
        Tree,
        Item,
        Circuit,
        Revolute
    };

    public enum PlantType
    {
        Tree
    };

    public enum LayerBlendType
    {
        Opaque,
        Overlay,
        Additive
    };

    public enum NoiseType
    {
        Perlin,
        Worley
    };

    public enum WorleyFeatureType
    {
        F1,
        F2,
        F2mF1
    };
}
