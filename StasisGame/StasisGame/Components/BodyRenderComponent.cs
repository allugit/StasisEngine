using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;

namespace StasisGame.Components
{
    public class BodyRenderComponent : IComponent, IRenderablePrimitive
    {
        private Texture2D _texture;
        private CustomVertexFormat[] _vertices;
        private List<RenderableTriangle> _renderableTriangles;
        private Matrix _worldMatrix;
        private float _layerDepth;

        public ComponentType componentType { get { return ComponentType.BodyRender; } }
        public Texture2D texture { get { return _texture; } }
        public Matrix worldMatrix { get { return _worldMatrix; } set { _worldMatrix = value; } }
        public int primitiveCount { get { return _renderableTriangles.Count; } }
        public float layerDepth { get { return _layerDepth; } }
        public CustomVertexFormat[] vertices { get { return _vertices; } }
        public List<RenderableTriangle> renderableTriangles { get { return _renderableTriangles; } }

        //public BodyRenderComponent(Texture2D texture, CustomVertexFormat[] vertices, Matrix worldMatrix, int primitiveCount, float layerDepth)
        public BodyRenderComponent(Texture2D texture, List<RenderableTriangle> renderableFixtures, float layerDepth)
        {
            _texture = texture;
            _renderableTriangles = renderableFixtures;
            _layerDepth = layerDepth;
            _vertices = new CustomVertexFormat[_renderableTriangles.Count * 3];
        }
    }
}
