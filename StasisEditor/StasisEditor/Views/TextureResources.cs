using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisEditor.Views
{
    public class TextureResources
    {
        private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

        // getAll
        public static List<TextureResource> getAllResources()
        {
            List<TextureResource> resources = new List<TextureResource>();

            // Create directory if necessary
            if (!Directory.Exists("../StasisGame/textures"))
                Directory.CreateDirectory("../StasisGame/textures");

            if (File.Exists("../StasisGame/textures/resources.xml"))
            {
                XmlDocument resDoc = new XmlDocument();
                resDoc.Load("../StasisGame/textures/resources.xml");
                throw new NotImplementedException();
            }

            return resources;
        }
    }
}
