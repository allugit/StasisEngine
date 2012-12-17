using System;
using System.Collections.Generic;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorGravityGun : EditorItem
    {
        private GravityGunItemResource _gravityGunItemResource;

        public GravityGunItemResource gravityGunItemResource { get { return _gravityGunItemResource; } }

        public EditorGravityGun(ItemResource resource)
            : base(resource)
        {
            _gravityGunItemResource = resource as GravityGunItemResource;
        }
    }
}
