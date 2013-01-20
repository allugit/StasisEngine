using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class MaterialScatterLayer : MaterialLayer
    {
        private List<string> _textureUIDs;
        private ScatterTextureOrder _textureOrder;
        private ScatterStyle _scatterStyle;
        private Vector2 _interestPoint;
        private bool _restictDistance;
        private float _minDistance;
        private float _maxDistance;
        private Color _baseColor;
        private int _randomRed;
        private int _randomGreen;
        private int _randomBlue;
        private int _randomAlpha;

        virtual public List<string> textureUIDs { get { return _textureUIDs; } }
        public ScatterTextureOrder textureOrder { get { return _textureOrder; } set { _textureOrder = value; } }
        public ScatterStyle scatterStyle { get { return _scatterStyle; } set { _scatterStyle = value; } }
        public Vector2 interestPoint { get { return _interestPoint; } set { _interestPoint = value; } }
        public bool restrictDistance { get { return _restictDistance; } set { _restictDistance = value; } }
        public float minDistance { get { return _minDistance; } set { _minDistance = value; } }
        public float maxDistance { get { return _maxDistance; } set { _maxDistance = value; } }
        public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }
        public int randomRed { get { return _randomRed; } set { _randomRed = value; } }
        public int randomGreen { get { return _randomGreen; } set { _randomGreen = value; } }
        public int randomBlue { get { return _randomBlue; } set { _randomBlue = value; } }
        public int randomAlpha { get { return _randomAlpha; } set { _randomAlpha = value; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                foreach (string textureUID in _textureUIDs)
                    d.Add(new XElement("Texture", new XAttribute("uid", textureUID)));

                d.SetAttributeValue("texture_order", textureOrder.ToString().ToLower());
                d.SetAttributeValue("scatter_style", _scatterStyle.ToString().ToLower());
                d.SetAttributeValue("interest_point", _interestPoint);
                d.SetAttributeValue("restrict_distance", _restictDistance);
                d.SetAttributeValue("min_distance", _minDistance);
                d.SetAttributeValue("max_distance", _maxDistance);
                d.SetAttributeValue("base_color", _baseColor);
                d.SetAttributeValue("random_red", _randomRed);
                d.SetAttributeValue("random_green", _randomGreen);
                d.SetAttributeValue("random_blue", _randomBlue);
                d.SetAttributeValue("random_alpha", _randomAlpha);

                return d;
            }
        }

        // Create new
        public MaterialScatterLayer()
            : base("scatter", true)
        {
            _textureUIDs = new List<string>();
            _scatterStyle = ScatterStyle.Anywhere;
            _interestPoint = Vector2.Zero;
            _restictDistance = false;
            _minDistance = 0f;
            _maxDistance = 2048f;
            _baseColor = Color.White;
            _randomRed = 0;
            _randomGreen = 0;
            _randomBlue = 0;
            _randomAlpha = 0;
        }

        // Create from xml
        public MaterialScatterLayer(XElement data) : base(data)
        {
            _textureUIDs = new List<string>();
            foreach (XElement textureXml in data.Elements("Texture"))
                _textureUIDs.Add(textureXml.Attribute("uid").Value);

            _textureOrder = (ScatterTextureOrder)Enum.Parse(typeof(ScatterTextureOrder), data.Attribute("texture_order").Value, true);
            _scatterStyle = (ScatterStyle)Enum.Parse(typeof(ScatterStyle), data.Attribute("scatter_style").Value, true);
            _interestPoint = XmlLoadHelper.getVector2(data.Attribute("interest_point").Value);
            _restictDistance = bool.Parse(data.Attribute("restrict_distance").Value);
            _minDistance = float.Parse(data.Attribute("min_distance").Value);
            _maxDistance = float.Parse(data.Attribute("max_distance").Value);
            _baseColor = XmlLoadHelper.getColor(data.Attribute("base_color").Value);
            _randomRed = int.Parse(data.Attribute("random_red").Value);
            _randomGreen = int.Parse(data.Attribute("random_green").Value);
            _randomBlue = int.Parse(data.Attribute("random_blue").Value);
            _randomAlpha = int.Parse(data.Attribute("random_alpha").Value);
        }
    }
}
