using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisEditor.Views;
using StasisEditor.Models;
using StasisEditor.Controls;

namespace StasisEditor.Controllers
{

    public class MaterialController : IMaterialController
    {
        private EditorController _editorController;
        private TerrainRenderer _terrainRenderer;
        private IMaterialView _materialView;
        private MaterialPreview _materialPreview;
        private List<MaterialResource>[] _materials;
        private bool _changesMade;
        private bool _autoUpdatePreview;

        public MaterialController(EditorController editorController, IMaterialView materialView)
        {
            _editorController = editorController;
            _materialView = materialView;

            // Materials
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            _materials = new List<MaterialResource>[numMaterialTypes];
            for (int i = 0; i < numMaterialTypes; i++)
                _materials[i] = new List<MaterialResource>();

            // Test material data
            List<TerrainLayerResource> testLayers = new List<TerrainLayerResource>();
            testLayers.Add(
                new TerrainTextureLayerResource(
                    null,
                    new TextureProperties(TerrainBlendType.Opaque, 1, 1, "rock")));
            testLayers.Add(
                new TerrainNoiseLayerResource(
                    new List<TerrainLayerResource>(new[] { new TerrainNoiseLayerResource(null, null) }),
                    null));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterialResource("Rock", testLayers));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterialResource("Dirt", TerrainLayerResource.copyFrom(testLayers)));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterialResource("Snow", new List<TerrainLayerResource>()));
            _materials[(int)MaterialType.Trees].Add(new TreeMaterialResource("Acuminate"));
            _materials[(int)MaterialType.Fluid].Add(new FluidMaterialResource("Water"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterialResource("Rope Gun"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterialResource("Gravity Gun"));

            // Initialize material view
            materialView.setController(this);
            materialView.copyMaterials();
            materialView.setAutoUpdatePreview(true);

            // Create terrain renderer
            _terrainRenderer = new TerrainRenderer(XNAResources.game as Game, XNAResources.spriteBatch);
        }

        // setAutoUpdatePreview
        public void setAutoUpdatePreview(bool status)
        {
            _autoUpdatePreview = status;
        }

        // getAutoUpdatePreview
        public bool getAutoUpdatePreview()
        {
            return _autoUpdatePreview;
        }

        // getChangesMade
        public bool getChangesMade()
        {
            return _changesMade;
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
            // Changes were made
            _changesMade = status;
            if (_materialView != null)
                _materialView.setChangesMade(status);

            // Update if set to auto update
            if (_autoUpdatePreview)
            {
                MaterialResource material = _materialView.getSelectedMaterial();
                if (material != null)
                    preview(material);
            }
        }

        // getMaterials
        public ReadOnlyCollection<MaterialResource> getMaterials(MaterialType type)
        {
            return _materials[(int)type].AsReadOnly();
        }

        // addTerrainLayer
        public void addTerrainLayer(TerrainMaterialResource material, TerrainLayerResource layer, TerrainLayerResource parent = null)
        {
            List<TerrainLayerResource> parentList = parent == null ? material.layers : parent.layers;
            parentList.Add(layer);
        }

        // removeTerrainLayer
        public void removeTerrainLayer(TerrainMaterialResource material, TerrainLayerResource parent, TerrainLayerResource layer)
        {
            List<TerrainLayerResource> parentList = parent == null ? material.layers : parent.layers;
            parentList.Remove(layer);
        }

        // moveTerrainLayerUp
        public void moveTerrainLayerUp(TerrainMaterialResource material, TerrainLayerResource parent, TerrainLayerResource layer)
        {
            List<TerrainLayerResource> parentList = parent == null ? material.layers : parent.layers;
            int currentIndex = parentList.IndexOf(layer);
            Debug.Assert(currentIndex != 0);

            // Remove layer from parent list
            parentList.Remove(layer);

            // Insert at the position before its last position
            parentList.Insert(currentIndex - 1, layer);
        }

        // moveTerrainLayerDown
        public void moveTerrainLayerDown(TerrainMaterialResource material, TerrainLayerResource parent, TerrainLayerResource layer)
        {
            List<TerrainLayerResource> parentList = parent == null ? material.layers : parent.layers;
            int currentIndex = parentList.IndexOf(layer);
            Debug.Assert(currentIndex != parentList.Count - 1);

            // Remove layer from parent list
            parentList.Remove(layer);

            // Insert at the position after its last position
            parentList.Insert(currentIndex + 1, layer);
        }

        // preview
        public void preview(MaterialResource material)
        {
            switch(material.type)
            {
                case MaterialType.Terrain:
                    TerrainMaterialResource terrainMaterial = material as TerrainMaterialResource;

                    // Test data
                    TexturedVertexFormat[] vertices = new TexturedVertexFormat[3];
                    float vertexScale = 20f;
                    vertices[0].color = new Vector3(1, 1, 1);
                    vertices[0].position = new Vector3(0.5f, 0, 0) * vertexScale;
                    vertices[0].texCoord = new Vector2(0.5f, 0);
                    vertices[1].color = new Vector3(1, 1, 1);
                    vertices[1].position = new Vector3(1f, 1f, 0) * vertexScale;
                    vertices[1].texCoord = new Vector2(1f, 1f);
                    vertices[2].color = new Vector3(1, 1, 1);
                    vertices[2].position = new Vector3(0, 1f, 0) * vertexScale;
                    vertices[2].texCoord = new Vector2(0, 1f);

                    // Resize graphics device
                    int graphicsDeviceWidth = XNAResources.graphicsDevice.Viewport.Width;
                    int graphicsDeviceHeight = XNAResources.graphicsDevice.Viewport.Height;
                    float baseScale = 35f;
                    int textureWidth = (int)(1f * baseScale);
                    int textureHeight = (int)(1f * baseScale);
                    _editorController.resizeGraphicsDevice(textureWidth, textureHeight);

                    // Create canvas
                    Texture2D canvas = _terrainRenderer.createCanvas(35, vertices);

                    // Render layers
                    foreach (TerrainLayerResource layer in terrainMaterial.layers)
                        canvas = _terrainRenderer.renderLayer(canvas, layer);

                    if (_materialPreview == null)
                    {
                        // Open material preview
                        _materialPreview = new MaterialPreview(this, canvas, String.Format("{0} Preview", material.name));
                        _materialPreview.Show();
                    }
                    else
                    {
                        // Put preview window on top
                        //_materialPreview.Focus();
                        _materialPreview.updatePreview(canvas);
                        //_materialView.Focus();
                    }

                    // Restore graphics device
                    _editorController.resizeGraphicsDevice(graphicsDeviceWidth, graphicsDeviceHeight);
                    XNAResources.graphicsDevice.Clear(Color.Black);

                    break;

                default:
                    return;
            }
        }

        // previewClosed
        public void previewClosed()
        {
            _materialPreview = null;
        }
    }
}
