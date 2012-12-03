using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TextureResource
    {
        protected string _tag;
        protected string _category;
        protected string _extension;

        [DisplayName("Tag")]
        public string tag { get { return _tag; } set { _tag = value; } }

        [DisplayName("Category")]
        public string category { get { return _category; } set { _category = value; } }

        [Browsable(false)]
        public string extension { get { return _extension; } }

        [Browsable(false)]
        public string fileName { get { return string.Format("{0}.{1}", _tag, _extension); } }

        public TextureResource(string tag, string category, string extension)
        {
            _tag = tag;
            _category = category;
            _extension = extension;
        }
    }
}
