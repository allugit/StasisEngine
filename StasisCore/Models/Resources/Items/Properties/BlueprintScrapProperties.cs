using System;
using System.Collections.Generic;

namespace StasisCore.Models
{
    public class BlueprintScrapProperties : ItemProperties
    {
        private string _blueprintTag;
        private string _scrapTextureTag;

        public string scrapTextureTag { get { return _scrapTextureTag; } set { _scrapTextureTag = value; } }
        public string blueprintTag { get { return _blueprintTag; } set { _blueprintTag = value; } }

        public BlueprintScrapProperties(string scrapTextureTag, string blueprintTag)
            : base()
        {
            _scrapTextureTag = scrapTextureTag;
            _blueprintTag = blueprintTag;
        }

        // ToString
        public override string ToString()
        {
            return "Blueprint Scrap Properties";
        }

        // clone
        public override ItemProperties clone()
        {
            return new BlueprintScrapProperties(_scrapTextureTag, _blueprintTag);
        }
    }
}
