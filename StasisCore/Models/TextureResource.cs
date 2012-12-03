using System;
using System.IO;
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
        public string fileName { get { return string.Format("{0}{1}", _tag, _extension); } }

        public TextureResource(string tag, string category, string extension)
        {
            _tag = tag;
            _category = category;
            _extension = extension;
        }

        // loadFromDirectory
        public static List<TextureResource> loadFromDirectory(string directoryPath)
        {
            List<TextureResource> resources = new List<TextureResource>();

            // Get directories
            string[] dirPaths = Directory.GetDirectories(directoryPath);
            foreach (string dirPath in dirPaths)
            {
                // Get files
                string[] files = Directory.GetFiles(dirPath);
                foreach (string file in files)
                {
                    // Create texture resource
                    string[] dirNames = dirPath.Split(new[] { '\\', '/' });
                    string category = dirNames[dirNames.Length - 1];
                    resources.Add(new TextureResource(Path.GetFileNameWithoutExtension(file), category, Path.GetExtension(file)));
                }
            }

            return resources;
        }

        // copyFrom
        public static List<TextureResource> copyFrom(IList<TextureResource> list)
        {
            List<TextureResource> copy = new List<TextureResource>(list.Count);
            foreach (TextureResource resource in list)
                copy.Add(resource.clone());
            return copy;
        }

        // clone
        public TextureResource clone()
        {
            return new TextureResource(_tag, _category, _extension);
        }
    }
}
