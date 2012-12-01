using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StasisEditor.Views;
using StasisEditor.Controls;
using StasisEditor.Models;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers
{
    public enum MaterialType
    {
        Terrain = 0,
        Trees,
        Fluid,
        Items
    };

    public class EditorController
    {
        private XNAController _xnaController;
        private IEditorView _editorView;

        private Level _level;
        private ILevelView _levelView;
        public Level level { get { return _level; } }
        private IMaterialView _materialView;
        private List<TerrainMaterial> _terrainMaterials;
        private List<TreeMaterial> _treeMaterials;
        private List<FluidMaterial> _fluidMaterials;
        private List<ItemMaterial> _itemMaterials;
        public ReadOnlyCollection<TerrainMaterial> terrainMaterials { get { return _terrainMaterials.AsReadOnly(); } }
        public ReadOnlyCollection<TreeMaterial> treeMaterials { get { return _treeMaterials.AsReadOnly(); } }
        public ReadOnlyCollection<FluidMaterial> fluidMaterials { get { return _fluidMaterials.AsReadOnly(); } }
        public ReadOnlyCollection<ItemMaterial> itemMaterials { get { return _itemMaterials.AsReadOnly(); } }

        private bool _isMouseOverView;
        private System.Drawing.Point _mouse;
        private float _scale = 35f;
        private Vector2 _screenCenter;

        public bool isMouseOverView { get { return _isMouseOverView; } }
        public float scale { get { return _scale; } }
        public Vector2 worldOffset { get { return _screenCenter + (new Vector2(_levelView.getWidth(), _levelView.getHeight()) / 2) / scale; } }
        public Vector2 worldMouse { get { return new Vector2(_mouse.X, _mouse.Y) / scale - worldOffset; } }

        public EditorController(XNAController xnaController, IEditorView editorView)
        {
            _xnaController = xnaController;
            _editorView = editorView;
            _editorView.setController(this);

            // Materials
            _terrainMaterials = new List<TerrainMaterial>();
            _treeMaterials = new List<TreeMaterial>();
            _fluidMaterials = new List<FluidMaterial>();
            _itemMaterials = new List<ItemMaterial>();

            // Test materials
            _terrainMaterials.Add(new TerrainMaterial("Rock"));
            _terrainMaterials.Add(new TerrainMaterial("Dirt"));
            _terrainMaterials.Add(new TerrainMaterial("Snow"));
            _treeMaterials.Add(new TreeMaterial());
            _fluidMaterials.Add(new FluidMaterial());
            _itemMaterials.Add(new ItemMaterial());
            _itemMaterials.Add(new ItemMaterial());
        }

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            _xnaController.resizeGraphicsDevice(width, height);
        }

        // createNewLevel
        public void createNewLevel()
        {
            Debug.Assert(_level == null);

            // Create model
            _level = new Level();

            // Create level view
            _levelView = new LevelView();
            _levelView.setController(this);
            
            // Add level view to editor view
            _editorView.addLevelView(_levelView);
            _editorView.addLevelSettings(level);

            // Create brush toolbar
            _editorView.addBrushToolbar();

            // Modify menu items
            _editorView.enableNewLevel(false);
            _editorView.enableCloseLevel(true);
            _editorView.enableLoadLevel(false);
            _editorView.enableSaveLevel(true);
        }

        // closeLevel
        public void closeLevel()
        {
            // Remove level views
            _editorView.removeLevelSettings();
            _editorView.removeLevelView();

            // Remove toolbar
            _editorView.removeBrushToolbar();

            // Modify menu
            _editorView.enableNewLevel(true);
            _editorView.enableCloseLevel(false);
            _editorView.enableLoadLevel(true);
            _editorView.enableSaveLevel(false);

            // Remove model
            _level = null;
        }

        // openMaterialView
        public void openMaterialView()
        {
            _materialView = new MaterialView();
            _materialView.setController(this);
            _materialView.ShowDialog();
        }

        // mouseMove
        public void mouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            int x = Math.Min(Math.Max(0, e.X), _levelView.getWidth());
            int y = Math.Min(Math.Max(0, e.Y), _levelView.getHeight());

            bool ctrl = Input.newKey.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl);

            if (ctrl)
            {
                int deltaX = _mouse.X - x;
                int deltaY = _mouse.Y - y;

                _screenCenter -= new Vector2(deltaX, deltaY) / scale;
            }

            _mouse.X = x;
            _mouse.Y = y;
        }

        // mouseEnter
        public void mouseEnter()
        {
            _isMouseOverView = true;
        }

        // mouseLeave
        public void mouseLeave()
        {
            _isMouseOverView = false;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            if (_levelView != null)
                _levelView.handleXNADraw();
        }

        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
