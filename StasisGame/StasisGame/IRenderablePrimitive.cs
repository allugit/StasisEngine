using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame
{
    public interface IRenderablePrimitive
    {
        Texture2D texture { get; }
        VertexPositionColorTexture[] vertices { get; }
        Matrix worldMatrix { get; }
        int primitiveCount { get; }
        float layerDepth { get; }
    }
}
