namespace StasisGame.Components
{
    public enum ParticleInfluenceType
    {
        Physical,
        Character,
        Dynamite,
        Rope,
        Explosion
    }

    public class ParticleInfluenceComponent : IComponent
    {
        private ParticleInfluenceType _type;
        private int _particleCount;

        public ParticleInfluenceType type { get { return _type; } }
        public int particleCount { get { return _particleCount; } set { _particleCount = value; } }
        public ComponentType componentType { get { return ComponentType.ParticleInfluence; } }

        public ParticleInfluenceComponent(ParticleInfluenceType type)
        {
            _type = type;
        }
    }
}
