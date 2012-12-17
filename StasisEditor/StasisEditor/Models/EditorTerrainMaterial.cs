using System;
using System.Collections.Generic;
using System.ComponentModel;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorTerrainMaterial : EditorMaterial
    {
        private TerrainMaterialResource _terrainMaterialResource;

        [Browsable(false)]
        public TerrainMaterialResource terrainMaterialResource { get { return _terrainMaterialResource; } }

        [Browsable(false)]
        public TerrainRootLayerResource rootLayer { get { return _terrainMaterialResource.rootLayer; } }

        public EditorTerrainMaterial(MaterialResource resource)
            : base(resource)
        {
            _terrainMaterialResource = resource as TerrainMaterialResource;
        }
    }
}
