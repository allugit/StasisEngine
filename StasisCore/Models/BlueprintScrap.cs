using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    /*
     * Blueprint scrap -- A piece of a blueprint. It consists of:
     * - Connections - Links between scraps that form whenever sockets are correctly positioned
     */
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
        private List<BlueprintScrap> _connected;

        public Texture2D scrapTexture { get { return _scrapTexture; } set { _scrapTexture = value; } }
        public Vector2 textureCenter { get { return _textureCenter; } set { _textureCenter = value; } }
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
        public Matrix rotationMatrix { get { return _rotationMatrix; } }
        public List<BlueprintScrap> connected { get { return _connected; } }

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("type", "BlueprintScrap");
                d.SetAttributeValue("current_craft_angle", _currentCraftAngle);
                d.SetAttributeValue("current_craft_position", _currentCraftPosition);
                d.SetAttributeValue("scrap_texture_uid", _scrapTextureUID);
                d.SetAttributeValue("blueprint_uid", _blueprintUID);
                foreach (Vector2 point in _points)
                    d.Add(new XElement("Point", point));

                return d;
            }
        }

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
            _connected = new List<BlueprintScrap>();
        }

        // Create from xml
        public BlueprintScrap(XElement data) : base(data)
        {
            _scrapTextureUID = data.Attribute("scrap_texture_uid").Value;
            _scrapTexture = ResourceManager.getTexture(_scrapTextureUID);
            _textureCenter = new Vector2(_scrapTexture.Width, _scrapTexture.Height) / 2;
            _blueprintUID = data.Attribute("blueprint_uid").Value;
            _currentCraftPosition = Loader.loadVector2(data.Attribute("current_craft_position"), Vector2.Zero);
            _currentCraftAngle = Loader.loadFloat(data.Attribute("current_craft_angle"), 0);
            _rotationMatrix = Matrix.Identity;

            _points = new List<Vector2>();
            foreach (XElement childData in data.Elements("Point"))
                _points.Add(Loader.loadVector2(childData.Value, Vector2.Zero));

            _connected = new List<BlueprintScrap>();
        }

        // connectScrap
        public void connectScrap(BlueprintScrap scrap)
        {
            _connected.Add(scrap);
        }

        // getConnected
        public List<BlueprintScrap> getConnected()
        {
            List<BlueprintScrap> list = new List<BlueprintScrap>();
            recursiveGetConnected(list);
            list.Remove(this);
            return list;
        }

        // recursiveGetConnected
        private void recursiveGetConnected(List<BlueprintScrap> results)
        {
            //Console.WriteLine("results: {0}", results.Count);
            foreach (BlueprintScrap scrap in _connected)
            {
                if (!results.Contains(scrap))
                {
                    results.Add(scrap);
                    scrap.recursiveGetConnected(results);
                }
            }
        }

        // move
        public void move(Vector2 delta, bool moveConnected)
        {
            // Move
            _currentCraftPosition += delta;

            if (moveConnected)
            {
                List<BlueprintScrap> allConnections = getConnected();
                foreach (BlueprintScrap scrap in allConnections)
                    scrap.move(delta, false);
            }
        }

        // rotate
        public void rotate(float delta)
        {
            // Rotate
            currentCraftAngle += delta;

            // Rotate connected
            List<BlueprintScrap> allConnections = getConnected();
            foreach (BlueprintScrap scrap in allConnections)
            {
                // Update scrap position
                Vector2 relative = scrap.currentCraftPosition - currentCraftPosition;
                scrap.currentCraftPosition = currentCraftPosition + Vector2.Transform(relative, Matrix.CreateRotationZ(delta));

                // Update scrap angle
                scrap.currentCraftAngle += delta;
            }
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
