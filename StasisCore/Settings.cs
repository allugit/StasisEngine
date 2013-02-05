namespace StasisCore
{
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
        Prismatic
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
