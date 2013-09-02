using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.Components
{
    public class RegionComponent : IComponent
    {
        private string _uid;

        public ComponentType componentType { get { return ComponentType.Region; } }
        public string uid { get { return _uid; } set { _uid = value; } }

        public RegionComponent(string uid)
        {
            _uid = uid;
        }
    }
}
