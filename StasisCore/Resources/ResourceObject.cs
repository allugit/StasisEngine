using System;
using System.Xml.Linq;
using StasisCore.Controllers;

namespace StasisCore.Resources
{
    public class ResourceObject
    {
        protected string _uid;
        protected XElement _data;

        public string uid { get { return _uid; } }
        public XElement data { get { return _data; } }

        public ResourceObject(XElement data)
            : this(data.Attribute("uid").Value)
        {
            _data = data;
        }

        public ResourceObject(string uid)
        {
            _uid = uid;

            // Validate uniqueness
            if (ResourceController.isResourceLoaded(_uid))
                throw new DuplicateResourceException(_uid);
        }
    }
}
