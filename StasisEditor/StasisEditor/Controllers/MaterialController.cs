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

    public class MaterialController
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

            // Test materials
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Rock", new List<TerrainLayer>()));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Dirt", new List<TerrainLayer>()));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Snow", new List<TerrainLayer>()));
            _materials[(int)MaterialType.Trees].Add(new TreeMaterial("Acuminate"));
            _materials[(int)MaterialType.Fluid].Add(new FluidMaterial("Water"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterial("Rope Gun"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterial("Gravity Gun"));

            // Create terrain renderer
            _terrainRenderer = new TerrainRenderer(XNAResources.game as Game, XNAResources.spriteBatch);
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
                    _editorController.resizeGraphicsDevice((int)(1f * _editorController.scale), (int)(1f * _editorController.scale));

                    // Render base pass
                    Texture2D result = _terrainRenderer.primitivesPass(null, _editorController.scale, vertices, 1);

                    // Render noise pass
                    //result = _terrainRenderer.noisePass(result, NoiseType.Perlin, Vector2.Zero, 2f, 1.5f, 0.5f, 0.8f, 1f, 1f, Color.Black, Color.White, 10);
                    //result = _terrainRenderer.noisePass(result, NoiseType.Perlin, Vector2.Zero, 1f, new Vector2(0.3f, 0.1f), 0.24f, 0.4f, 0.6f, 1f, 1f, Color.Black, Color.White, 10);
                    //result = _terrainRenderer.noisePass(result, NoiseType.Perlin, Vector2.Zero, 1f, new Vector2(-0.3f, -0.25f), 0.54f, 0.6f, 0.8f, 1f, 1f, Color.Black, Color.White, 3);
                    //result = _terrainRenderer.noisePass(result, NoiseType.Worley, Vector2.Zero, 2f, Vector2.Zero, 1.1f, 0.4f, 0.8f, 1f, 1f, Color.Black, Color.White, 1);

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
