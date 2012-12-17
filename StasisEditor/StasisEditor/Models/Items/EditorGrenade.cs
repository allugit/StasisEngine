using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorGrenade : EditorItem
    {
        private GrenadeItemResource _grenadeItemResource;

        public GrenadeItemResource grenadeItemResource { get { return _grenadeItemResource; } }

        public EditorGrenade(ItemResource resource)
            : base(resource)
        {
            _grenadeItemResource = resource as GrenadeItemResource;
        }
    }
}
