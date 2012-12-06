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
            Console.WriteLine("Initializing material controller.");
            _editorController = editorController;
            _materialView = materialView;

            // Materials
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            _materials = new List<MaterialResource>[numMaterialTypes];
            for (int i = 0; i < numMaterialTypes; i++)
                _materials[i] = new List<MaterialResource>();

            // Test material data
            TerrainRootLayerResource rootLayer = new TerrainRootLayerResource();
            rootLayer.layers.Add(new TerrainTextureLayerResource(new TextureProperties(TerrainBlendType.Opaque, 1f, 1f, "rock")));
            rootLayer.layers.Add(new TerrainNoiseLayerResource(new NoiseProperties(NoiseType.Perlin, TerrainBlendType.Overlay, WorleyFeature.F1, Vector2.Zero, 1, Vector2.Zero, 1.1f, 0.5f, 2f, 1f, Color.Black, Color.White, 1)));
            rootLayer.layers.Add(new TerrainGroupLayerResource(
                new List<TerrainLayerResource>(new TerrainLayerResource[] {
                    new TerrainTextureLayerResource(new TextureProperties(TerrainBlendType.Opaque, 1f, 1f, "rock_3")),
                    new TerrainNoiseLayerResource(new NoiseProperties(NoiseType.Worley, TerrainBlendType.Overlay, WorleyFeature.F1, Vector2.Zero, 1, Vector2.Zero, 1.1f, 0.5f, 2f, 2f, Color.Black, Color.White, 1))
                }),
                new GroupProperties(TerrainBlendType.Overlay), false));

            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterialResource("Rock", rootLayer));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterialResource("Dirt", rootLayer));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterialResource("Snow", rootLayer));
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
        public void addTerrainLayer(TerrainGroupLayerResource parent, TerrainLayerResource layer, int index)
        {
            parent.layers.Insert(index, layer);
        }

        // removeTerrainLayer
        public void removeTerrainLayer(TerrainGroupLayerResource parent, TerrainLayerResource layer)
        {
            parent.layers.Remove(layer);
        }

        // moveTerrainLayerUp
        public void moveTerrainLayerUp(TerrainGroupLayerResource parent, TerrainLayerResource layer)
        {
            Debug.Assert(parent.layers.Contains(layer));

            // Store current layer index
            int index = parent.layers.IndexOf(layer);

            // Remove layer from parent list
            parent.layers.Remove(layer);

            // Insert at the position before its last position
            parent.layers.Insert(index - 1, layer);
        }

        // moveTerrainLayerDown
        public void moveTerrainLayerDown(TerrainGroupLayerResource parent, TerrainLayerResource layer)
        {
            Debug.Assert(parent.layers.Contains(layer));

            // Store current layer index
            int index = parent.layers.IndexOf(layer);

            // Remove layer from parent list
            parent.layers.Remove(layer);

            // Insert at the position after its last position
            parent.layers.Insert(index + 1, layer);
        }

        // preview
        public void preview(MaterialResource material)
        {
            switch(material.type)
            {
                case MaterialType.Terrain:
                    TerrainMaterialResource terrainMaterial = material as TerrainMaterialResource;

                    // Test data
                    /*
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
                    */

                    // Resize graphics device
                    int graphicsDeviceWidth = XNAResources.graphicsDevice.Viewport.Width;
                    int graphicsDeviceHeight = XNAResources.graphicsDevice.Viewport.Height;
                    //float baseScale = 35f;
                    //int textureWidth = (int)(1f * baseScale);
                    //int textureHeight = (int)(1f * baseScale);
                    int textureWidth = 512;
                    int textureHeight = 512;
                    _editorController.resizeGraphicsDevice(textureWidth, textureHeight);

                    Texture2D materialTexture = _terrainRenderer.renderMaterial(terrainMaterial, textureWidth, textureHeight);

                    /*
                    // Create canvas
                    Texture2D canvas = _terrainRenderer.createCanvas(35, vertices);

                    // Render layers
                    foreach (TerrainLayerResource layer in terrainMaterial.layers)
                        canvas = _terrainRenderer.renderLayer(canvas, layer);
                    */

                    if (_materialPreview == null)
                    {
                        // Open material preview
                        _materialPreview = new MaterialPreview(this, materialTexture, String.Format("{0} Preview", material.name));
                        _materialPreview.Show();
                    }
                    else
                    {
                        // Put preview window on top
                        //_materialPreview.Focus();
                        _materialPreview.updatePreview(materialTexture);
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
