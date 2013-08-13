using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class ItemState
    {
        private int _quantity;
        private float _currentRange;

        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public float currentRange { get { return _currentRange; } set { _currentRange = value; } }

        public ItemState(int quantity, float currentRange)
        {
            _quantity = quantity;
            _currentRange = currentRange;
        }
    }
}
