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
        void addTerrainLayer(TerrainGroupLayerResource parent, TerrainLayerResource layer, int index);
        void removeTerrainLayer(TerrainGroupLayerResource parent, TerrainLayerResource layer);
        void moveTerrainLayerUp(TerrainMaterialResource material, TerrainLayerResource parent, TerrainLayerResource layer);
        void moveTerrainLayerDown(TerrainMaterialResource material, TerrainLayerResource parent, TerrainLayerResource layer);
    }
}
