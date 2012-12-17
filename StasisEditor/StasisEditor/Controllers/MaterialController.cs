using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisEditor.Models;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{

    public class MaterialController : Controller
    {
        private EditorController _editorController;
        private TerrainRenderer _terrainRenderer;
        private MaterialView _materialView;
        private MaterialPreview _materialPreview;
        private List<EditorMaterial> _materials;
        private bool _autoUpdatePreview;

        public MaterialController(EditorController editorController, MaterialView materialView)
        {
            _editorController = editorController;
            _materialView = materialView;

            // Initialize material view
            materialView.setController(this);
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

        // setChangesMade
        public void setChangesMade(bool status)
        {
            // Update view
            if (_materialView != null)
                _materialView.setChangesMade(status);

            // Update if set to auto update
            if (_autoUpdatePreview)
            {
                EditorMaterial material = _materialView.getSelectedMaterial();
                if (material != null)
                    preview(material);
            }
        }

        // getMaterials
        public List<EditorMaterial> getMaterials(MaterialType type)
        {
            List<EditorMaterial> results = new List<EditorMaterial>();
            foreach (EditorMaterial material in _materials)
            {
                if (material.type == type)
                    results.Add(material);
            }
            return results;
        }

        // loadResources
        protected override void loadResources()
        {
            Debug.Assert(_materials == null);

            // Materials
            _materials = new List<EditorMaterial>();

            // Load resources
            string[] subDirectories = Directory.GetDirectories(EditorController.MATERIAL_RESOURCE_DIRECTORY);
            foreach (string subDirectory in subDirectories)
            {
                // Read files in sub directory
                string[] files = Directory.GetFiles(subDirectory);
                foreach (string file in files)
                {
                    // Load material
                    MaterialResource resource = MaterialResource.load(file);
                    _materials.Add(EditorMaterial.create(resource));
                }
            }
        }

        // saveResource
        public void saveResource(MaterialResource resource)
        {
            // Create material resource directory if necessary
            if (!Directory.Exists(EditorController.MATERIAL_RESOURCE_DIRECTORY))
                Directory.CreateDirectory(EditorController.MATERIAL_RESOURCE_DIRECTORY);

            // Create material sub folder directory
            string materialSubFolder = String.Format("{0}\\{1}", EditorController.MATERIAL_RESOURCE_DIRECTORY, resource.type.ToString());
            if (!Directory.Exists(materialSubFolder))
                Directory.CreateDirectory(materialSubFolder);

            // Save material file
            string fullPath = String.Format("{0}\\{1}.mat", materialSubFolder, resource.tag);
            XElement element = resource.toXML();
            element.Save(fullPath);

            // Clean up resources
            foreach (MaterialType materialType in Enum.GetValues(typeof(MaterialType)))
            {
                string directoryName = materialType.ToString();
                string materialDirectory = String.Format("{0}\\{1}", EditorController.MATERIAL_RESOURCE_DIRECTORY, directoryName);
                foreach (string file in Directory.GetFiles(materialDirectory))
                {
                    string materialTag = Path.GetFileNameWithoutExtension(file);

                    // Search materials for this tag
                    bool found = false;
                    foreach (EditorMaterial material in _materials)
                    {
                        if (material.tag == materialTag)
                        {
                            found = true;
                            break;
                        }
                    }

                    // Remove file if not found in material list -- This happens when materials are renamed
                    if (!found)
                        File.Delete(file);
                }
            }
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
        public void preview(EditorMaterial material)
        {
            switch(material.type)
            {
                case MaterialType.Terrain:
                    EditorTerrainMaterial terrainMaterial = material as EditorTerrainMaterial;

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

                    Texture2D materialTexture = _terrainRenderer.renderMaterial(terrainMaterial.terrainMaterialResource, textureWidth, textureHeight);

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
                        _materialPreview = new MaterialPreview(this, materialTexture, String.Format("{0} Preview", material.tag));
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
