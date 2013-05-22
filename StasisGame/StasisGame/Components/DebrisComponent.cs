using FarseerPhysics.Dynamics;

namespace StasisGame.Components
{
    public class DebrisComponent : IComponent
    {
        public const int RESTITUTION_RESTORE_COUNT = 10;
        private int _timeToLive;
        private float _restitutionIncrement;
        private Fixture _fixture;
        private int _restitutionCount;

        public int timeToLive { get { return _timeToLive; } set { _timeToLive = value; } }
        public float restitutionIncrement { get { return _restitutionIncrement; } }
        public Fixture fixture { get { return _fixture; } }
        public int restitutionCount { get { return _restitutionCount; } set { _restitutionCount = value; } }
        public ComponentType componentType { get { return ComponentType.Debris; } }

        public DebrisComponent(Fixture fixture, int timeToLive, float restitutionIncrement)
        {
            _fixture = fixture;
            _timeToLive = timeToLive;
            _restitutionIncrement = restitutionIncrement;
            _restitutionCount = 0;
        }
    }
}
