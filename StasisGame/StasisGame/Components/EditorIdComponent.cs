using System;

namespace StasisGame.Components
{
    public class EditorIdComponent : IComponent
    {
        private int _id;

        public ComponentType componentType { get { return ComponentType.EditorId; } }
        public int id { get { return _id; } }

        public EditorIdComponent(int id)
        {
            _id = id;
        }
    }
}
