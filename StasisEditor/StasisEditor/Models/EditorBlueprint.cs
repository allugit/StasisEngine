using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorBlueprint : Blueprint
    {
        public EditorBlueprint(string uid)
            : base(uid)
        {
        }

        public EditorBlueprint(XElement data, List<BlueprintScrap> scraps, List<BlueprintSocket> sockets)
            : base(data, scraps, sockets)
        {
        }

        public override string ToString()
        {
            return _uid;
        }
    }
}
