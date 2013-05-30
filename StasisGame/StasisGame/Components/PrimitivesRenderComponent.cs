using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;

namespace StasisGame.Components
{
    public class PrimitivesRenderComponent : IComponent
    {
        private List<PrimitiveRenderObject> _primitiveRenderObjects;

        public ComponentType componentType { get { return ComponentType.PrimitivesRender; } }
        public List<PrimitiveRenderObject> primitiveRenderObjects { get { return _primitiveRenderObjects; } }

        public PrimitivesRenderComponent(List<PrimitiveRenderObject> primitiveRenderObjects)
        {
            _primitiveRenderObjects = primitiveRenderObjects;
        }

        public PrimitivesRenderComponent(PrimitiveRenderObject primitiveRenderObject)
        {
            _primitiveRenderObjects = new List<PrimitiveRenderObject>();
            _primitiveRenderObjects.Add(primitiveRenderObject);
        }
    }
}
