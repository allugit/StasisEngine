using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class ItemState
    {
        private int _quantity;
        private float _currentRangeLimit;
        private bool _inWorld;

        public int quantity { get { return _quantity; } set { _quantity = value; } }
        public float currentRangeLimit { get { return _currentRangeLimit; } set { _currentRangeLimit = value; } }
        public bool inWorld { get { return _inWorld; } set { _inWorld = value; } }

        public ItemState(int quantity, float currentRangeLimit, bool inWorld)
        {
            _quantity = quantity;
            _currentRangeLimit = currentRangeLimit;
            _inWorld = inWorld;
        }
    }
}
