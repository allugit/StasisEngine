using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public class Background
    {
        protected List<BackgroundLayer> _layers;

        public List<BackgroundLayer> layers { get { return _layers; } }
        virtual public XElement data
        {
            get
            {
                List<XElement> layerData = new List<XElement>();
                foreach (BackgroundLayer layer in _layers)
                    layerData.Add(layer.data);
                return new XElement("Background",
                    layerData);
            }
        }

        public Background()
        {
            _layers = new List<BackgroundLayer>();
        }

        public Background(XElement data)
        {
            _layers = new List<BackgroundLayer>();
            createLayers(data);
        }

        public void loadTextures()
        {
            foreach (BackgroundLayer layer in _layers)
                layer.loadTexture();
        }

        virtual public void createLayers(XElement data)
        {
            foreach (XElement layerData in data.Elements("BackgroundLayer"))
                _layers.Add(new BackgroundLayer(layerData));
        }

        virtual public Background clone()
        {
            Background copy = new Background(data);
            copy.loadTextures();

            return copy;
        }
    }
}
