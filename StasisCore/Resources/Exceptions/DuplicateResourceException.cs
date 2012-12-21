using System;

namespace StasisCore.Resources
{
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException(string uid)
            : base(String.Format("A resource with uid [{0}] has already been loaded.", uid))
        {
        }
    }
}
