using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Controllers;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class BlueprintScrap : Item
    {
        private Texture2D _scrapTexture;
        private Vector2 _textureCenter;
        private List<Vector2> _points;
        private Vector2 _currentCraftPosition;
        private float _currentCraftAngle;
        private string _scrapTextureUID;
        private string _blueprintUID;
        private Matrix _rotationMatrix;

        public Texture2D scrapTexture { get { return _scrapTexture; } }
        public Vector2 textureCenter { get { return _textureCenter; } }
        public List<Vector2> points { get { return _points; } set { _points = value; } }
        public Vector2 currentCraftPosition { get { return _currentCraftPosition; } set { _currentCraftPosition = value; } }
        public float currentCraftAngle
        {
            get { return _currentCraftAngle; }
            set 
            {
                _currentCraftAngle = value;
                _rotationMatrix = Matrix.CreateRotationZ(value);
            } 
        }
        public string scrapTextureUID { get { return _scrapTextureUID; } set { _scrapTextureUID = value; } }
        public string blueprintUID { get { return _blueprintUID; } set { _blueprintUID = value; } }

        // Create new
        public BlueprintScrap(string uid) : base(uid)
        {
            _points = new List<Vector2>();
            _scrapTextureUID = "default_texture";
            _blueprintUID = "";
            _currentCraftPosition = Vector2.Zero;
            _currentCraftAngle = 0;
            _textureCenter = Vector2.Zero;
            _rotationMatrix = Matrix.Identity;
        }

        // Create from xml
        public BlueprintScrap(XElement data) : base(data)
        {
            _scrapTextureUID = data.Attribute("scrap_texture_uid").Value;
            _scrapTexture = ResourceController.getTexture(_scrapTextureUID);
            _textureCenter = new Vector2(_scrapTexture.Width, _scrapTexture.Height) / 2;
            _blueprintUID = data.Attribute("blueprint_uid").Value;
            _currentCraftPosition = XmlLoadHelper.getVector2(data.Attribute("current_craft_position").Value);
            _currentCraftAngle = float.Parse(data.Attribute("current_craft_angle").Value);
            _rotationMatrix = Matrix.Identity;

            _points = new List<Vector2>();
            foreach (XElement childData in data.Elements("Point"))
                _points.Add(XmlLoadHelper.getVector2(childData.Value));
        }

        // hitTest -- http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        public bool hitTest(Vector2 point)
        {
            // Convert point to local space
            point = point - _currentCraftPosition;

            bool hit = false;
            for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                Vector2 pi = Vector2.Transform(points[i], _rotationMatrix);
                Vector2 pj = Vector2.Transform(points[j], _rotationMatrix);
                if (((pi.Y > point.Y) != (pj.Y > point.Y)) &&
                    (point.X < (pj.X - pi.X) * (point.Y - pi.Y) / (pj.Y - pi.Y) + pi.X))
                    hit = !hit;
            }

            return hit;
        }
    }
}
