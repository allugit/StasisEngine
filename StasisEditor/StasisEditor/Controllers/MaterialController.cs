using System;
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

    public class MaterialController : IController
    {
        private EditorController _editorController;
        private TerrainRenderer _terrainRenderer;
        private IMaterialView _materialView;
        private List<Material>[] _materials;

        public MaterialController(EditorController editorController)
        {
            _editorController = editorController;

            // Materials
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            _materials = new List<Material>[numMaterialTypes];
            for (int i = 0; i < numMaterialTypes; i++)
                _materials[i] = new List<Material>();

            // Test material data
            List<TerrainLayer> testLayers = new List<TerrainLayer>();
            testLayers.Add(new TerrainBaseLayer());
            testLayers.Add(new TerrainNoiseLayer());
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Rock", testLayers));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Dirt", TerrainLayer.copyFrom(testLayers)));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Snow", new List<TerrainLayer>()));
            _materials[(int)MaterialType.Trees].Add(new TreeMaterial("Acuminate"));
            _materials[(int)MaterialType.Fluid].Add(new FluidMaterial("Water"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterial("Rope Gun"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterial("Gravity Gun"));

            // Create terrain renderer
            _terrainRenderer = new TerrainRenderer(XNAResources.game as Game, XNAResources.spriteBatch);
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
            if (_materialView != null)
                _materialView.setChangesMade(status);
        }

        // getMaterials
        public ReadOnlyCollection<Material> getMaterials(MaterialType type)
        {
            return _materials[(int)type].AsReadOnly();
        }

        // openView
        public void openView()
        {
            _materialView = new MaterialView();
            _materialView.setController(this);
            _materialView.ShowDialog();
        }

        // preview
        public void preview(Material material)
        {
            switch(material.type)
            {
                case MaterialType.Terrain:
                    TerrainMaterial terrainMaterial = material as TerrainMaterial;

                    // Test data
                    TexturedVertexFormat[] vertices = new TexturedVertexFormat[3];
                    float vertexScale = 20f;
                    vertices[0].color = new Vector3(1, 0, 0);
                    vertices[0].position = new Vector3(0.5f, 0, 0) * vertexScale;
                    vertices[0].texCoord = new Vector2(0.5f, 0);
                    vertices[1].color = new Vector3(0, 1, 0);
                    vertices[1].position = new Vector3(1f, 1f, 0) * vertexScale;
                    vertices[1].texCoord = new Vector2(1f, 1f);
                    vertices[2].color = new Vector3(0, 0, 1);
                    vertices[2].position = new Vector3(0, 1f, 0) * vertexScale;
                    vertices[2].texCoord = new Vector2(0, 1f);

                    // Resize graphics device
                    int graphicsDeviceWidth = XNAResources.graphicsDevice.Viewport.Width;
                    int graphicsDeviceHeight = XNAResources.graphicsDevice.Viewport.Height;
                    float baseScale = 35f;
                    int textureWidth = (int)(1f * baseScale);
                    int textureHeight = (int)(1f * baseScale);
                    _editorController.resizeGraphicsDevice(textureWidth, textureHeight);

                    // Create result texture
                    Texture2D result = new Texture2D(XNAResources.graphicsDevice, textureWidth, textureHeight);

                    // Render layers
                    foreach (TerrainLayer layer in terrainMaterial.layers)
                        result = _terrainRenderer.renderLayer(result, layer, baseScale, vertices, 1);

                    // Restore graphics device
                    _editorController.resizeGraphicsDevice(graphicsDeviceWidth, graphicsDeviceHeight);
                    XNAResources.graphicsDevice.Clear(Color.Black);

                    // Open material preview
                    MaterialPreview materialPreview = new MaterialPreview(result, String.Format("{0} Preview", material.name));
                    materialPreview.Show();
                    break;

                default:
                    return;
            }
        }
    }
}
