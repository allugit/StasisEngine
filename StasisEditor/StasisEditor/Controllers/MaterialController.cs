using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Controllers;
using StasisCore.Models;
using StasisCore.Resources;
using StasisEditor.Models;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{

    public class MaterialController : Controller
    {
        private EditorController _editorController;
        private MaterialView _materialView;
        private MaterialPreview _materialPreview;
        private BindingList<Material> _materials;
        private bool _autoUpdatePreview;

        public BindingList<Material> materials { get { return _materials; } }

        public MaterialController(EditorController editorController, MaterialView materialView)
        {
            _editorController = editorController;
            _materialView = materialView;
            _materials = new BindingList<Material>();

            // Load materials
            List<ResourceObject> resources = ResourceController.loadMaterials();
            foreach (ResourceObject resource in resources)
                _materials.Add(new Material(resource));

            // Initialize material view
            materialView.setController(this);
            materialView.setAutoUpdatePreview(true);

            // Create terrain renderer
            //_materialRenderer = new MaterialRenderer(XNAResources.game as Game, XNAResources.spriteBatch);
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
            // Update if set to auto update
            if (_autoUpdatePreview)
            {
                Material material = _materialView.selectedMaterial;
                if (material != null)
                    preview(material);
            }
        }

        // saveMaterials
        public void saveMaterials()
        {
            ResourceController.saveMaterialResources(new List<Material>(_materials));
        }

        // createMaterial
        public void createMaterial(string uid)
        {
            // Check unsaved materials
            foreach (Material m in _materials)
            {
                if (m.uid == uid)
                {
                    System.Windows.Forms.MessageBox.Show(String.Format("An unsaved resource with the uid [{0}] already exists.", uid), "Material Error", System.Windows.Forms.MessageBoxButtons.OK);
                    return;
                }
            }

            Material material = new Material(uid);
            _materials.Add(material);
        }

        // removeMaterial
        public void removeMaterial(string uid, bool destroy = true)
        {
            Material materialToRemove = null;
            foreach (Material material in _materials)
            {
                if (material.uid == uid)
                {
                    materialToRemove = material;
                    break;
                }
            }

            Debug.Assert(materialToRemove != null);

            _materials.Remove(materialToRemove);

            try
            {
                if (destroy)
                    ResourceController.destroy(uid);
            }
            catch (ResourceNotFoundException e)
            {
                System.Windows.Forms.MessageBox.Show(String.Format("Could not destroy resource.\n{0}", e.Message), "Resource Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        // addTerrainLayer
        public void addTerrainLayer(MaterialGroupLayer parent, MaterialLayer layer, int index)
        {
            parent.layers.Insert(index, layer);
        }

        // removeTerrainLayer
        public void removeTerrainLayer(MaterialGroupLayer parent, MaterialLayer layer)
        {
            parent.layers.Remove(layer);
        }

        // moveTerrainLayerUp
        public void moveTerrainLayerUp(MaterialGroupLayer parent, MaterialLayer layer)
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
        public void moveTerrainLayerDown(MaterialGroupLayer parent, MaterialLayer layer)
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
        public void preview(Material material)
        {
            // Render material
            if (_materialPreview == null)
            {
                // Open material preview
                _materialPreview = new MaterialPreview(this, material);
                _materialPreview.Show();
                Console.WriteLine("material preview created and shown.");
                _materialPreview.updateMaterial(material);
            }
            else
            {
                // Put preview window on top
                //_materialPreview.Focus();
                _materialPreview.updateMaterial(material);
                //_materialView.Focus();
            }

            /*
            // Resize graphics device
            int graphicsDeviceWidth = XNAResources.graphicsDevice.Viewport.Width;
            int graphicsDeviceHeight = XNAResources.graphicsDevice.Viewport.Height;
            int textureWidth = 512;
            int textureHeight = 512;
            _editorController.resizeGraphicsDevice(textureWidth, textureHeight);

            // Render material
            Texture2D materialTexture = null;
            try
            {
                materialTexture = _materialRenderer.renderMaterial(material, textureWidth, textureHeight);
            }
            catch (ResourceNotFoundException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Resource Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            if (_materialPreview == null)
            {
                // Open material preview
                _materialPreview = new MaterialPreview(this, materialTexture, String.Format("{0} Preview", material.uid));
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
            */
        }

        // previewClosed
        public void previewClosed()
        {
            _materialPreview = null;
        }
    }
}
