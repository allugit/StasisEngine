using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorGrenade : EditorItem
    {
        private GrenadeItemResource _grenadeItemResource;

        [Browsable(false)]
        public GrenadeItemResource grenadeItemResource { get { return _grenadeItemResource; } }

        [CategoryAttribute("Grenade Properties")]
        [DisplayName("Sticky Grenades")]
        public bool sticky { get { return _grenadeItemResource.sticky; } set { _grenadeItemResource.sticky = value; } }

        [CategoryAttribute("Grenade Properties")]
        [DisplayName("Explosion Radius")]
        public float radius { get { return _grenadeItemResource.radius; } set { _grenadeItemResource.radius = value; } }

        [CategoryAttribute("Grenade Properties")]
        [DisplayName("Explosion Strength")]
        public float strength { get { return _grenadeItemResource.strength; } set { _grenadeItemResource.strength = value; } }

        public EditorGrenade(ItemResource resource)
            : base(resource)
        {
            _grenadeItemResource = resource as GrenadeItemResource;
        }
    }
}
