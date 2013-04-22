using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class WorldMap
    {
        protected Texture2D _texture;

        virtual public Texture2D texture { get { return _texture; } }

        public WorldMap()
        {
        }

        public WorldMap(XElement data)
        {
            _texture = ResourceManager.getTexture(Loader.loadString(data.Attribute("texture_uid"), "world_map"));
        }
    }
}
