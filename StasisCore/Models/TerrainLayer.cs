using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisCore.Models
{
    abstract public class TerrainLayer
    {
        protected TerrainLayerResource _resource;
        public TerrainLayerResource resource { get { return _resource; } set { _resource = value; } }

        public TerrainLayer(TerrainLayerResource resource)
        {
            _resource = resource;
        }

        // initializeResource
        abstract public void initializeResource();
    }
}
