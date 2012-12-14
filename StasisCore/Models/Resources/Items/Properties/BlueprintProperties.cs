using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class BlueprintProperties : ItemProperties
    {
        private string _itemTag;

        public string itemTag { get { return _itemTag; } set { _itemTag = value; } }

        public BlueprintProperties(string itemTag)
            : base()
        {
            _itemTag = itemTag;
        }

        // ToString
        public override string ToString()
        {
            return "Blueprint Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new BlueprintProperties(_itemTag);
        }
    }
}
