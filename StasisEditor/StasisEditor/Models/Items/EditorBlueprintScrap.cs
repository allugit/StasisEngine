﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Models
{
    public class EditorBlueprintScrap : EditorItem
    {
        private BlueprintScrapItemResource _blueprintScrapResource;
        private Texture2D _texture;
        private Matrix _rotationMatrix;
        private Vector2 _textureCenter;

        public BlueprintScrapItemResource blueprintScrapResource { get { return _blueprintScrapResource; } }
        public Vector2 position { get { return _blueprintScrapResource.craftingPosition; } set { _blueprintScrapResource.craftingPosition = value; } }
        public List<Vector2> points { get { return _blueprintScrapResource.points; } set { _blueprintScrapResource.points = value; } }
        public Texture2D texture { get { return _texture; } }
        public Vector2 textureCenter { get { return _textureCenter; } }

        public EditorBlueprintScrap(ItemResource resource) 
            : base(resource)
        {
            _blueprintScrapResource = resource as BlueprintScrapItemResource;

            // Load texture
            _texture = StasisCore.Controllers.TextureController.getTexture(_blueprintScrapResource.scrapTextureTag);
            Debug.Assert(_texture != null);
            _textureCenter = new Vector2(_texture.Width, _texture.Height) / 2;

            // Initialize crafting properties
            _blueprintScrapResource.craftingPosition = _textureCenter;

            // Initialize rotation matrix
            _rotationMatrix = Matrix.Identity;
        }

        // hitTest -- http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        public bool hitTest(Vector2 point)
        {
            // Convert point to local space
            point = point - position;

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