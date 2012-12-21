using System;

namespace StasisCore.Resources
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string uid)
            : base(String.Format("Resource [{0}] was not found", uid))
        {
        }
    }
}
