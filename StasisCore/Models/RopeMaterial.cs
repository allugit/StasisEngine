using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class RopeMaterial
    {
        private List<RopeMaterialTexture> _textures;
        private int _interpolationCount;
        private RopeTextureStyle _ropeTextureStyle;

        public List<RopeMaterialTexture> textures { get { return _textures; } }
        public int interpolationCount { get { return _interpolationCount; } }
        public RopeTextureStyle ropeTextureStyle { get { return _ropeTextureStyle; } }

        public RopeMaterial(XElement data)
        {
            _interpolationCount = Loader.loadInt(data.Attribute("interpolation_count"), 3);
            _ropeTextureStyle = (RopeTextureStyle)Loader.loadEnum(typeof(RopeTextureStyle), data.Attribute("rope_texture_style"), (int)RopeTextureStyle.Random);
            _textures = new List<RopeMaterialTexture>();
            foreach (XElement textureData in data.Elements("Texture"))
                _textures.Add(new RopeMaterialTexture(textureData));
        }
    }
}
