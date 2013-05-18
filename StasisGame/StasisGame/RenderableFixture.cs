using System;
using System.Collections.Generic;
using StasisCore;

namespace StasisGame
{
    public class RenderableFixture
    {
        public CustomVertexFormat[] vertices;

        public RenderableFixture(CustomVertexFormat v1, CustomVertexFormat v2, CustomVertexFormat v3)
        {
            vertices = new CustomVertexFormat[3] { v1, v2, v2 };
        }
    }
}
