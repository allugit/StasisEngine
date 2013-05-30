using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame
{
    public class PrimitiveRenderObject : IRenderablePrimitive
    {
        private Texture2D _texture;
        private VertexPositionTexture[] _vertices;
        private List<RenderableTriangle> _renderableTriangles;
        private Matrix _worldMatrix;
        private Matrix _originMatrix;
        private float _layerDepth;

        public Texture2D texture { get { return _texture; } }
        public Matrix worldMatrix { get { return _worldMatrix; } set { _worldMatrix = value; } }
        public Matrix originMatrix { get { return _originMatrix; } set { _originMatrix = value; } }
        public int primitiveCount { get { return _renderableTriangles.Count; } }
        public float layerDepth { get { return _layerDepth; } }
        public VertexPositionTexture[] vertices { get { return _vertices; } }
        public List<RenderableTriangle> renderableTriangles { get { return _renderableTriangles; } }

        public PrimitiveRenderObject(Texture2D texture, List<RenderableTriangle> renderableTriangle, float layerDepth)
        {
            _vertices = new VertexPositionTexture[renderableTriangle.Count * 3];
            _texture = texture;
            _renderableTriangles = renderableTriangle;
            _layerDepth = layerDepth;
            _originMatrix = Matrix.Identity;
        }
    }
}
