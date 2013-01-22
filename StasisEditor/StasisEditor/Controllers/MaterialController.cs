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
        private BindingList<EditorMaterial> _materials;
        private bool _autoUpdatePreview;
        private MaterialLayer _copiedMaterialLayer;
        private List<Vector2> _testPolygonPoints;

        public BindingList<EditorMaterial> materials { get { return _materials; } }
        public MaterialLayer copiedMaterialLayer { get { return _copiedMaterialLayer; } set { _copiedMaterialLayer = value; } }
        public List<Vector2> testPolygonPoints { get { return _testPolygonPoints; } }

        public MaterialController(EditorController editorController, MaterialView materialView)
        {
            _editorController = editorController;
            _materialView = materialView;
            _materials = new BindingList<EditorMaterial>();

            // Load materials
            List<ResourceObject> resources = ResourceController.loadMaterials();
            foreach (ResourceObject resource in resources)
                _materials.Add(new EditorMaterial(resource.data));

            // Initialize material view
            materialView.setController(this);
            materialView.setAutoUpdatePreview(true);

            // Initialize preview polygon points
            _testPolygonPoints = new List<Vector2>();
            _testPolygonPoints.Add(new Vector2(-3.5f, 0) / 4.5f);
            _testPolygonPoints.Add(new Vector2(-1, 1) / 4.5f);
            _testPolygonPoints.Add(new Vector2(0, 3) / 4.5f);
            _testPolygonPoints.Add(new Vector2(2, 2.5f) / 4.5f);
            _testPolygonPoints.Add(new Vector2(3, 0) / 4.5f);
            _testPolygonPoints.Add(new Vector2(4, -1) / 4.5f);
            _testPolygonPoints.Add(new Vector2(3.5f, -3) / 4.5f);
            _testPolygonPoints.Add(new Vector2(1, -3.5f) / 4.5f);
            _testPolygonPoints.Add(new Vector2(0.5f, -3) / 4.5f);
            _testPolygonPoints.Add(new Vector2(-1, -4) / 4.5f);
            _testPolygonPoints.Add(new Vector2(-2.5f, -2.5f) / 4.5f);
            _testPolygonPoints.Add(new Vector2(-3.5f, -3) / 4.5f);
            _testPolygonPoints.Add(new Vector2(-4.5f, -1.5f) / 4.5f);
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
                    preview(material, _testPolygonPoints);
            }
        }

        // Check if material exists
        public bool materialExists(string uid)
        {
            foreach (EditorMaterial material in _materials)
            {
                if (uid == material.uid)
                    return true;
            }
            return false;
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

            EditorMaterial material = new EditorMaterial(uid);
            _materials.Add(material);
        }

        // removeMaterial
        public void removeMaterial(string uid, bool destroy = true)
        {
            EditorMaterial materialToRemove = null;
            foreach (EditorMaterial material in _materials)
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
        public void preview(Material material, List<Vector2> polygonPoints)
        {
            // Render material
            if (_materialPreview == null)
            {
                _materialPreview = new MaterialPreview(this);
                _materialPreview.Show();
                _materialPreview.updateMaterial(material, polygonPoints);
            }
            else
            {
                _materialPreview.updateMaterial(material, polygonPoints);
            }
        }

        // previewClosed
        public void previewClosed()
        {
            _materialPreview = null;
        }

        // Clone material
        public void cloneMaterial(EditorMaterial source)
        {
            EditorMaterial material = source.clone();
            string newUID = material.uid + "_copy";
            while (ResourceController.exists(newUID))
                newUID = newUID + "_copy";
            while (materialExists(newUID))
                newUID = newUID + "_copy";
            material.uid = newUID;
            _materials.Add(material);
        }
    }
}
