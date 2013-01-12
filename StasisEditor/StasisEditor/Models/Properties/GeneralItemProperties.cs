using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisEditor.Models.Properties
{
    public class GeneralItemProperties : ActorProperties
    {
        private int _quantity;
        public int quantity { get { return _quantity; } set { _quantity = value; } }

        public GeneralItemProperties(int quantity)
            : base()
        {
            _quantity = quantity;
        }

        public override ActorProperties clone()
        {
            return new GeneralItemProperties(_quantity);
        }
    }
}
