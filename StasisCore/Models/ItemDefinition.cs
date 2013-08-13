using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class ItemDefinition
    {
        private string _uid;
        private bool _hasAimingComponent;
        private float _minRange;
        private float _maxRange;
        private bool _stackable;
        private string _inventoryTextureUid;
        private string _worldTextureUid;

        public string uid { get { return _uid; } set { _uid = value; } }
        public bool hasAimingComponent { get { return _hasAimingComponent; } set { _hasAimingComponent = value; } }
        public float minRange { get { return _minRange; } set { _minRange = value; } }
        public float maxRange { get { return _maxRange; } set { _maxRange = value; } }
        public bool stackable { get { return _stackable; } set { _stackable = value; } }
        public string inventoryTextureUid { get { return _inventoryTextureUid; } set { _inventoryTextureUid = value; } }
        public string worldTextureUid { get { return _worldTextureUid; } set { _worldTextureUid = value; } }

        public ItemDefinition(string uid, bool hasAimingComponent, float minRange, float maxRange, bool stackable, string inventoryTextureUid, string worldTextureUid)
        {
            _uid = uid;
            _hasAimingComponent = hasAimingComponent;
            _minRange = minRange;
            _maxRange = maxRange;
            _inventoryTextureUid = inventoryTextureUid;
            _worldTextureUid = worldTextureUid;
            _stackable = stackable;
        }
    }
}
