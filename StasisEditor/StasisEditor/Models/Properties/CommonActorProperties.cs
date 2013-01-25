using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using StasisCore;

namespace StasisEditor.Models
{
    public class CommonActorProperties : ActorProperties
    {
        private float _depth;

        public float depth { get { return _depth; } set { _depth = value; } }
        [Browsable(false)]
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[]
                {
                    new XAttribute("depth", _depth)
                };
            }
        }

        public CommonActorProperties(float depth = 0f)
            : base()
        {
            _depth = depth;
        }

        public CommonActorProperties(XElement data)
        {
            _depth = Loader.loadFloat(data.Attribute("depth"), 0f);
        }

        public override ActorProperties clone()
        {
            return new CommonActorProperties(_depth);
        }
    }
}
