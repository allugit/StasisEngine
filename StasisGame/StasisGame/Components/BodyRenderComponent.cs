using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;

namespace StasisGame.Components
{
    public class BodyRenderComponent : IComponent
    {
        private Texture2D _texture;
        private CustomVertexFormat[] _vertices;
        private Matrix _worldMatrix;
        private int _primitiveCount;
        private float _layerDepth;

        public ComponentType componentType { get { return ComponentType.BodyRender; } }

        public BodyRenderComponent(Texture2D texture, CustomVertexFormat[] vertices, Matrix worldMatrix, int primitiveCount, float layerDepth)
        {
            _texture = texture;
            _vertices = vertices;
            _worldMatrix = worldMatrix;
            _primitiveCount = primitiveCount;
            _layerDepth = layerDepth;
        }
    }
}
