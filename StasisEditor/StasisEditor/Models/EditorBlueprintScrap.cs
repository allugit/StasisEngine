using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorBlueprintScrap : BlueprintScrap
    {
        public EditorBlueprintScrap(string uid) : base(uid)
        {
        }

        public EditorBlueprintScrap(XElement data)
            : base(data)
        {
        }

        public override string ToString()
        {
            return _uid;
        }
    }
}
