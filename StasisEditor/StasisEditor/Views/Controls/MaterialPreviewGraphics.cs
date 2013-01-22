using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Poly2Tri;
using StasisCore;
using StasisCore.Models;
using StasisCore.Resources;
using StasisCore.Controllers;

namespace StasisEditor.Views.Controls
{
    public class MaterialPreviewGraphics : GraphicsDeviceControl
    {
        public struct CustomVertexFormat
        {
            public Vector3 position;
            public Vector2 texCoord;
            public Vector3 color;

            public CustomVertexFormat(Vector3 position, Vector2 texCoord, Vector3 color)
            {
                this.position = position;
                this.texCoord = texCoord;
                this.color = color;
            }

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * 3 + sizeof(float) * 2, VertexElementFormat.Vector3, VertexElementUsage.Color, 0));
        };

        private MaterialRenderer _materialRenderer;
        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private CustomVertexFormat[] _vertices;
        private int _primitiveCount;
        private int _vertexCount;
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;

        public MaterialPreviewGraphics()
            : base()
        {
        }

        protected override void Initialize()
        {
            _contentManager = new ContentManager(Services, "Content");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            try
            {
                _materialRenderer = new MaterialRenderer(GraphicsDevice, _contentManager, _spriteBatch);
            }
            catch (ContentLoadException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            Console.WriteLine("Material preview graphics completely initialized.");
        }

        public void setMaterial(Material material, List<Vector2> polygonPoints, float growthFactor)
        {
            try
            {
                _texture = _materialRenderer.renderMaterial(material, polygonPoints, growthFactor, 512, 512);

                if (polygonPoints != null && polygonPoints.Count >= 3)
                {
                    _vertices = new CustomVertexFormat[5000];
                    _vertexCount = 0;

                    List<PolygonPoint> P2TPoints = new List<PolygonPoint>();
                    foreach (Vector2 point in polygonPoints)
                        P2TPoints.Add(new PolygonPoint(point.X, point.Y));
                    Polygon polygon = new Polygon(P2TPoints);
                    P2T.Triangulate(polygon);
                    _primitiveCount = polygon.Triangles.Count;

                    Vector2 topLeft = polygonPoints[0];
                    Vector2 bottomRight = polygonPoints[0];
                    foreach (DelaunayTriangle triangle in polygon.Triangles)
                    {
                        foreach (TriangulationPoint point in triangle.Points)
                        {
                            topLeft.X = Math.Min(point.Xf, topLeft.X);
                            topLeft.Y = Math.Min(point.Yf, topLeft.Y);
                            bottomRight.X = Math.Max(point.Xf, bottomRight.X);
                            bottomRight.Y = Math.Max(point.Yf, bottomRight.Y);
                        }
                    }

                    foreach (DelaunayTriangle triangle in polygon.Triangles)
                    {
                        Vector2 p1 = new Vector2(triangle.Points[0].Xf, triangle.Points[0].Yf);
                        Vector2 p2 = new Vector2(triangle.Points[1].Xf, triangle.Points[1].Yf);
                        Vector2 p3 = new Vector2(triangle.Points[2].Xf, triangle.Points[2].Yf);

                        _vertices[_vertexCount++] = new CustomVertexFormat(
                            new Vector3(p1, 0),
                            (p1 - topLeft) / (bottomRight - topLeft),
                            Vector3.One);
                        _vertices[_vertexCount++] = new CustomVertexFormat(
                            new Vector3(p2, 0),
                            (p2 - topLeft) / (bottomRight - topLeft),
                            Vector3.One);
                        _vertices[_vertexCount++] = new CustomVertexFormat(
                            new Vector3(p3, 0),
                            (p3 - topLeft) / (bottomRight - topLeft),
                            Vector3.One);
                    }
                }
                else
                {
                    _vertices = null;
                }

                
                Invalidate();
            }
            catch (ResourceNotFoundException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Resource Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw texture
            if (_texture != null && _vertices == null)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_texture, _texture.Bounds, Color.White);
                _spriteBatch.End();
                return;
            }

            // Draw texture on a polygon
            if (_texture != null && _vertices != null)
            {
                //_viewMatrix = Matrix.CreateScale(32f) * Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0) / 2);
                _viewMatrix = Matrix.CreateScale(new Vector3(256f, -256f, 1f));
                _projectionMatrix = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 1);

                GraphicsDevice.Textures[0] = _texture;
                Effect primitivesEffect = _materialRenderer.primitivesEffect;
                primitivesEffect.Parameters["world"].SetValue(Matrix.Identity);
                primitivesEffect.Parameters["view"].SetValue(_viewMatrix);
                primitivesEffect.Parameters["projection"].SetValue(_projectionMatrix);
                primitivesEffect.CurrentTechnique.Passes["textured_primitives"].Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, _primitiveCount, CustomVertexFormat.VertexDeclaration);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _contentManager.Unload();

            base.Dispose(disposing);
        }
    }
}
