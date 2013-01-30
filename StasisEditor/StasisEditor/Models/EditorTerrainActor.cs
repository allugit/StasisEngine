using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorTerrainActor : EditorPolygonActor
    {
        public EditorTerrainActor(EditorLevel level)
            : base(level, ActorType.Terrain)
        {
        }

        public EditorTerrainActor(EditorLevel level, XElement data)
            : base(level, data)
        {
        }
    }
}
