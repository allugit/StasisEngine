using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterial : Material
    {
        public EditorMaterial(string uid)
            : base(uid)
        {
        }

        public EditorMaterial(XElement data)
            : base(data)
        {
        }

        public override string ToString()
        {
            return _uid;
        }
    }
}
