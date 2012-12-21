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

        public List<string> textureUIDs { get { return _textureUIDs; } }
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
        public int ranomdAlpha { get { return _randomAlpha; } set { _randomAlpha = value; } }

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
