using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;

namespace StasisCore.Models
{
    public class TextureResource
    {
        private string _tag;
        private string _category;
        private string _extension;
        private string _loadedFrom;

        [DisplayName("Tag")]
        public string tag { get { return _tag; } set { _tag = value; } }

        [DisplayName("Category")]
        public string category { get { return _category; } set { _category = value; } }

        [Browsable(false)]
        public string extension { get { return _extension; } }

        [Browsable(false)]
        public string fileName { get { return string.Format("{0}{1}", _tag, _extension); } }

        [Browsable(false)]
        public string relativePath { get { return string.Format("{0}\\{1}", _category, fileName); } }

        [Browsable(false)]
        public string loadedFrom { get { return _loadedFrom; } }

        public TextureResource(string tag, string category, string extension)
        {
            _tag = tag;
            _category = category;
            _extension = extension;
        }

        // loadAll
        public static List<TextureResource> loadAll(string textureDirectory)
        {
            List<TextureResource> resources = new List<TextureResource>();

            // Get directories
            string[] dirPaths = Directory.GetDirectories(textureDirectory);
            foreach (string dirPath in dirPaths)
            {
                // Get files
                string[] files = Directory.GetFiles(dirPath);
                foreach (string file in files)
                {
                    // Create texture resource
                    resources.Add(loadFromFile(file));
                }
            }

            return resources;
        }

        // loadFromFile
        public static TextureResource loadFromFile(string filePath)
        {
            string[] dirNames = filePath.Split(new[] { '\\', '/' });
            string category = dirNames[dirNames.Length - 2];
            TextureResource resource = new TextureResource(Path.GetFileNameWithoutExtension(filePath), category, Path.GetExtension(filePath));
            resource._loadedFrom = filePath;
            return resource;
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
