using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorRopeGun : EditorItem
    {
        private RopeGunItemResource _ropeGunItemResource;

        [Browsable(false)]
        public RopeGunItemResource ropeGunItemResource { get { return _ropeGunItemResource; } }

        [CategoryAttribute("Gun Properties")]
        [DisplayName("Double Anchor")]
        public bool doubleAnchor { get { return _ropeGunItemResource.doubleAnchor; } set { _ropeGunItemResource.doubleAnchor = value; } }

        [CategoryAttribute("Gun Properties")]
        [DisplayName("Range")]
        public float range { get { return _ropeGunItemResource.range; } set { _ropeGunItemResource.range = value; } }

        public EditorRopeGun(ItemResource resource)
            : base(resource)
        {
            _ropeGunItemResource = resource as RopeGunItemResource;
        }
    }
}
