namespace StasisCore
{
    public enum ActorType
    {
        BoxActor,
        CircleActor,
        MovingPlatform,
        PressurePlate,
        Terrain,
        Rope,
        Fluid,
        PlayerSpawn,
        Tree
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

    public enum ScatterTextureOrder
    {
        Sequential,
        Random
    };

    public enum ScatterStyle
    {
        Anywhere,
        AroundInterestPoint
    };

    public enum WorleyFeatureType
    {
        F1,
        F2,
        F2mF1
    };
}
