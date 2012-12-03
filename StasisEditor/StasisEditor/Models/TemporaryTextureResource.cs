using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class TemporaryTextureResource : TextureResource
    {
        private string _filePath;

        [Browsable(false)]
        public string filePath;

        public TemporaryTextureResource(string filePath)
            : base("", "", Path.GetExtension(filePath))
        {
            _filePath = filePath;
        }

        // copyFrom
        public static List<TemporaryTextureResource> copyFrom(List<TemporaryTextureResource> list)
        {
            List<TemporaryTextureResource> copy = new List<TemporaryTextureResource>();
            foreach (TemporaryTextureResource resource in list)
                copy.Add(resource.clone() as TemporaryTextureResource);
            return copy;
        }

        // clone
        public override TextureResource clone()
        {
            TemporaryTextureResource resource = base.clone() as TemporaryTextureResource;
            resource._filePath = _filePath;
            return resource;
        }
    }
}
