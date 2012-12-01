using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StasisCore;
using StasisEditor.Views;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{

    public class MaterialController
    {
        private EditorController _editorController;
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
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Rock"));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Dirt"));
            _materials[(int)MaterialType.Terrain].Add(new TerrainMaterial("Snow"));
            _materials[(int)MaterialType.Trees].Add(new TreeMaterial("Acuminate"));
            _materials[(int)MaterialType.Fluid].Add(new FluidMaterial("Water"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterial("Rope Gun"));
            _materials[(int)MaterialType.Items].Add(new ItemMaterial("Gravity Gun"));
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
    }
}
