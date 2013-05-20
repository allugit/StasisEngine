namespace StasisGame.Components
{
    public enum ParticleInfluenceType
    {
        Physical,
        Rope,
        Character,
        Dynamite,
        Explosion
    }

    public class ParticleInfluenceTypeComponent : IComponent
    {
        private ParticleInfluenceType _type;

        public ParticleInfluenceType type { get { return _type; } }
        public ComponentType componentType { get { return ComponentType.ParticleInfluenceType; } }

        public ParticleInfluenceTypeComponent(ParticleInfluenceType type)
        {
            _type = type;
        }
    }
}
