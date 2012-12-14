using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class BlueprintScrapProperties : ItemProperties
    {
        private string _scrapTextureTag;

        public string scrapTextureTag { get { return _scrapTextureTag; } set { _scrapTextureTag = value; } }

        public BlueprintScrapProperties(string scrapTextureTag)
            : base()
        {
            _scrapTextureTag = scrapTextureTag;
        }

        // ToString
        public override string ToString()
        {
            return "Blueprint Scrap Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new BlueprintScrapProperties(_scrapTextureTag);
        }
    }
}
