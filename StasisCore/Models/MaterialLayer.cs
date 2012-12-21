using System;
using System.Xml.Linq;

namespace StasisCore.Models
{
    abstract public class MaterialLayer
    {
        protected bool _enabled;
        protected string _type;

        public bool enabled { get { return _enabled; } }
        public string type { get { return _type; } }

        public MaterialLayer(XElement data)
        {
            _type = data.Attribute("type").Value;
            _enabled = bool.Parse(data.Attribute("enabled").Value);
        }

        public static MaterialLayer create(XElement data)
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
    }
}
