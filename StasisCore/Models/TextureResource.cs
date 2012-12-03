using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TextureResource
    {
        private string _tag;
        private string _category;

        [DisplayName("Tag")]
        public string tag { get; set; }

        [DisplayName("Category")]
        public string category { get; set; }

        public TextureResource(string tag, string category)
        {
            _tag = tag;
            _category = category;
        }
    }
}
