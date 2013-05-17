using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class IgnoreParticleInfluenceComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.IgnoreParticleInfluence; } }

        public IgnoreParticleInfluenceComponent()
        {
        }
    }
}
