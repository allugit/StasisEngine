using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Poly2Tri;
using StasisCore;

namespace StasisGame
{
    public class RenderableTriangle
    {
        public CustomVertexFormat[] vertices;
        public Fixture fixture;

        public RenderableTriangle(CustomVertexFormat v1, CustomVertexFormat v2, CustomVertexFormat v3)
        {
            vertices = new CustomVertexFormat[3] { v1, v2, v3 };
        }

        public RenderableTriangle(DelaunayTriangle triangle, Vector2 topLeft, Vector2 bottomRight, bool fixVerticeRotation = false, float angleCorrection = 0f)
        {
            Vector2 p1 = new Vector2(triangle.Points[0].Xf, triangle.Points[0].Yf);
            Vector2 p2 = new Vector2(triangle.Points[1].Xf, triangle.Points[1].Yf);
            Vector2 p3 = new Vector2(triangle.Points[2].Xf, triangle.Points[2].Yf);

            // Fix rotation of vertices if necessary (boxes need this)
            if (fixVerticeRotation)
            {
                p1 = Vector2.Transform(p1, Matrix.CreateRotationZ(-angleCorrection));
                p2 = Vector2.Transform(p2, Matrix.CreateRotationZ(-angleCorrection));
                p3 = Vector2.Transform(p3, Matrix.CreateRotationZ(-angleCorrection));
            }

            vertices = new CustomVertexFormat[3];
            vertices[0] = new CustomVertexFormat(
                new Vector3(p1, 0),
                (p1 - topLeft) / (bottomRight - topLeft),
                Vector3.One);
            vertices[1] = new CustomVertexFormat(
                new Vector3(p2, 0),
                (p2 - topLeft) / (bottomRight - topLeft),
                Vector3.One);
            vertices[2] = new CustomVertexFormat(
                new Vector3(p3, 0),
                (p3 - topLeft) / (bottomRight - topLeft),
                Vector3.One);
        }

        public RenderableTriangle(Fixture triangleFixture, Vector2 topLeft, Vector2 bottomRight)
        {
            PolygonShape polygonShape = triangleFixture.Shape as PolygonShape;

            vertices = new CustomVertexFormat[3];
            for (int i = 0; i < 3; i++)
            {
                Vector2 p = polygonShape.Vertices[i];
                vertices[i] = new CustomVertexFormat(
                    new Vector3(p, 0),
                    (p - topLeft) / (bottomRight - topLeft),
                    Vector3.One);
            }
        }
    }
}
