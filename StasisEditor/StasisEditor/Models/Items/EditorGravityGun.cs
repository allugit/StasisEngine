using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore.Resources;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorGravityGun : EditorItem
    {
        private GravityGunItemResource _gravityGunItemResource;

        [Browsable(false)]
        public GravityGunItemResource gravityGunItemResource { get { return _gravityGunItemResource; } }

        [CategoryAttribute("Gun Properties")]
        [DisplayName("Well Gun")]
        public bool wellGun { get { return _gravityGunItemResource.wellGun; } set { _gravityGunItemResource.wellGun = value; } }

        [CategoryAttribute("Gun Properties")]
        [DisplayName("Range")]
        public float range { get { return _gravityGunItemResource.range; } set { _gravityGunItemResource.range = value; } }

        [CategoryAttribute("Gun Properties")]
        [DisplayName("Well Radius")]
        public float radius { get { return _gravityGunItemResource.radius; } set { _gravityGunItemResource.radius = value; } }

        [CategoryAttribute("Gun Properties")]
        [DisplayName("Strength")]
        public float strength { get { return _gravityGunItemResource.strength; } set { _gravityGunItemResource.strength = value; } }

        public EditorGravityGun(ItemResource resource)
            : base(resource)
        {
            _gravityGunItemResource = resource as GravityGunItemResource;
        }

        // toXML
        public override XElement toXML()
        {
            return _gravityGunItemResource.toXML();
        }
    }
}
