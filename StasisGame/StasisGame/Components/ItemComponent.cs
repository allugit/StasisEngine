using System;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class ItemComponent : IComponent
    {
        private int _quantity;

        public ComponentType componentType { get { return ComponentType.Item; } }

        public ItemComponent(int quantity)
        {
            _quantity = quantity;
        }
    }
}
