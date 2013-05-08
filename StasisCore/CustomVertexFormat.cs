using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore
{
    public struct CustomVertexFormat : IVertexType
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

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return CustomVertexFormat.VertexDeclaration; }
        }
    };
}
