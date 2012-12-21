using System;
using StasisCore.Controllers;

namespace StasisCore.Resources
{
    abstract public class BaseResourceObject
    {
        protected string _uid;

        public string uid { get { return _uid; } }

        public BaseResourceObject(string uid)
        {
            _uid = uid;

            // Validate uniqueness
            if (ResourceController.resourceLoaded(_uid))
                throw new DuplicateResourceException();
        }
    }
}
