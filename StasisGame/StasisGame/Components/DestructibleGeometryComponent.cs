using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class DestructibleGeometryComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.DestructibleGeometry; } }

        public DestructibleGeometryComponent()
        {
        }
    }
}
