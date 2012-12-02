using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    public class TextureResource
    {
        private string _tag;
        private string _category;

        public string tag { get { return _tag; } set { _tag = value; } }
        public string category { get { return _category; } set { _category = value; } }

        public TextureResource(string tag, string category)
        {
            _tag = tag;
            _category = category;
        }
    }
}
