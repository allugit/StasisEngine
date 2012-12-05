using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public interface IMaterialController : IController
    {
        void setAutoUpdatePreview(bool status);
        bool getAutoUpdatePreview();
        void setChangesMade(bool status);
        bool getChangesMade();
        void preview(MaterialResource material);
        void previewClosed();
        ReadOnlyCollection<MaterialResource> getMaterials(MaterialType type);
        void addTerrainLayer(TerrainMaterialResource material, TerrainLayerType layerType, TerrainLayerResource parent = null);
        void removeTerrainLayer(TerrainMaterialResource material, TerrainLayerResource parent, TerrainLayerResource layer);
    }
}
