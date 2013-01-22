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
            switch (data.Attribute("type").Value)
            {
                case "root":
                    return new EditorMaterialGroupLayer(data);

                case "group":
                    return new EditorMaterialGroupLayer(data);

                case "texture":
                    return new EditorMaterialTextureLayer(data);

                case "noise":
                    return new EditorMaterialNoiseLayer(data);

                case "uniform_scatter":
                    return new EditorMaterialUniformScatterLayer(data);

                case "radial_scatter":
                    return new EditorMaterialRadialScatterLayer(data);

                case "edge_scatter":
                    return new EditorMaterialEdgeScatterLayer(data);
            }
            return null;
        }

        public static MaterialLayer create(string type)
        {
            switch (type)
            {
                case "group":
                    return new EditorMaterialGroupLayer();

                case "texture":
                    return new EditorMaterialTextureLayer();

                case "noise":
                    return new EditorMaterialNoiseLayer();

                case "uniform_scatter":
                    return new EditorMaterialUniformScatterLayer();

                case "radial_scatter":
                    return new EditorMaterialRadialScatterLayer();

                case "edge_scatter":
                    return new EditorMaterialEdgeScatterLayer();
            }
            return null;
        }
    }
}
