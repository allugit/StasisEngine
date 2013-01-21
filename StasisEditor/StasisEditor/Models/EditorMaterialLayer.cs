using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorMaterialLayer
    {

        public static MaterialLayer load(XElement data)
        {
            MaterialLayer layer = null;
            switch (data.Attribute("type").Value)
            {
                case "root":
                    layer = new EditorMaterialGroupLayer(data);
                    break;

                case "group":
                    layer = new EditorMaterialGroupLayer(data);
                    break;

                case "texture":
                    layer = new EditorMaterialTextureLayer(data);
                    break;

                case "noise":
                    layer = new EditorMaterialNoiseLayer(data);
                    break;

                case "uniform_scatter":
                    layer = new EditorMaterialUniformScatterLayer(data);
                    break;

                case "radial_scatter":
                    layer = new EditorMaterialRadialScatterLayer(data);
                    break;
            }
            return layer;
        }

        public static MaterialLayer create(string type)
        {
            MaterialLayer layer = null;
            switch (type)
            {
                case "group":
                    layer = new EditorMaterialGroupLayer();
                    break;

                case "texture":
                    layer = new EditorMaterialTextureLayer();
                    break;

                case "noise":
                    layer = new EditorMaterialNoiseLayer();
                    break;

                case "uniform_scatter":
                    layer = new EditorMaterialUniformScatterLayer();
                    break;

                case "radial_scatter":
                    layer = new EditorMaterialRadialScatterLayer();
                    break;
            }
            return layer;
        }
    }
}
