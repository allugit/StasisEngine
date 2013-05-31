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
using StasisCore.Models;
using StasisEditor.Models;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{

    public class MaterialController : Controller
    {
        private EditorController _editorController;
        private MaterialView _materialView;
        private BindingList<EditorMaterial> _materials;
        private MaterialLayer _copiedMaterialLayer;

        public BindingList<EditorMaterial> materials { get { return _materials; } }
        public MaterialLayer copiedMaterialLayer { get { return _copiedMaterialLayer; } set { _copiedMaterialLayer = value; } }
        public EditorController editorController { get { return _editorController; } }

        public MaterialController(EditorController editorController, MaterialView materialView)
        {
            _editorController = editorController;
            _materialView = materialView;
            _materials = new BindingList<EditorMaterial>();
            List<XElement> materialData;

            // Load materials
            ResourceManager.loadAllMaterials(new FileStream(ResourceManager.materialPath, FileMode.Open));
            materialData = ResourceManager.materialResources;
            foreach (XElement data in materialData)
                _materials.Add(new EditorMaterial(data));

            // Initialize material view
            materialView.setController(this);
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
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
            EditorResourceManager.saveMaterialResources(new List<Material>(_materials), true);
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

            /*
            try
            {
                if (destroy)
                    ResourceManager.destroy(uid);
            }
            catch (ResourceNotFoundException e)
            {
                System.Windows.Forms.MessageBox.Show(String.Format("Could not destroy resource.\n{0}", e.Message), "Resource Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }*/
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

        // Clone material
        public void cloneMaterial(EditorMaterial source)
        {
            EditorMaterial material = source.clone();
            string newUID = material.uid + "_copy";
            while (EditorResourceManager.exists(newUID))
                newUID = newUID + "_copy";
            while (materialExists(newUID))
                newUID = newUID + "_copy";
            material.uid = newUID;
            _materials.Add(material);
        }
    }
}
