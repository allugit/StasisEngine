using System;
using System.Xml.Linq;

namespace StasisCore.Models
{
    abstract public class MaterialLayer
    {
        protected bool _enabled;
        protected string _type;

        public bool enabled { get { return _enabled; } set { _enabled = value; } }
        public string type { get { return _type; } }

        // Create new
        public MaterialLayer(string type, bool enabled)
        {
            _type = type;
            _enabled = enabled;
        }

        // Create from xml
        public MaterialLayer(XElement data)
        {
            _type = data.Attribute("type").Value;
            _enabled = bool.Parse(data.Attribute("enabled").Value);
        }

        public static MaterialLayer load(XElement data)
        {
            MaterialLayer layer = null;
            switch (data.Attribute("type").Value)
            {
                case "root":
                    layer = new MaterialRootLayer(data);
                    break;

                case "group":
                    layer = new MaterialGroupLayer(data);
                    break;

                case "texture":
                    layer = new MaterialTextureLayer(data);
                    break;

                case "noise":
                    layer = new MaterialNoiseLayer(data);
                    break;

                case "scatter":
                    layer = new MaterialScatterLayer(data);
                    break;
            }
            return layer;
        }

        public static MaterialLayer create(string type)
        {
            MaterialLayer layer = null;
            switch (type)
            {
                case "root":
                    layer = new MaterialRootLayer();
                    break;

                case "group":
                    layer = new MaterialGroupLayer();
                    break;

                case "texture":
                    layer = new MaterialTextureLayer();
                    break;

                case "noise":
                    layer = new MaterialNoiseLayer();
                    break;

                case "scatter":
                    layer = new MaterialScatterLayer();
                    break;
            }
            return layer;
        }
    }
}
