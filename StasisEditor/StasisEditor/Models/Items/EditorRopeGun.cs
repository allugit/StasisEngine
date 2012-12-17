using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorRopeGun : EditorItem
    {
        private RopeGunItemResource _ropeGunItemResource;

        public RopeGunItemResource ropeGunItemResource { get { return _ropeGunItemResource; } }

        public EditorRopeGun(ItemResource resource)
            : base(resource)
        {
            _ropeGunItemResource = resource as RopeGunItemResource;
        }
    }
}
