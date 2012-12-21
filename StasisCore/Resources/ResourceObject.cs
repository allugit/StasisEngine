using System;
using System.Xml.Linq;

namespace StasisCore.Resources
{
    public class ResourceObject : BaseResourceObject
    {
        protected XElement _data;

        public ResourceObject(XElement data)
            : base(data.Attribute("uid").Value)
        {
            _data = data;
        }
    }
}
