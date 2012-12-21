using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Resources
{
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException()
            : base("Tried to load a resource with a uid that has already been loaded")
        {
        }
    }
}
