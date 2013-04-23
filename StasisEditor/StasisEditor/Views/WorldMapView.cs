using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore;
using StasisEditor.Controllers;
using StasisEditor.Models;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public partial class WorldMapView : UserControl
    {
        private WorldMapController _controller;

        public WorldMapController controller { get { return _controller; } set { _controller = value; } }
        public BindingList<EditorWorldMap> worldMaps { set { worldMapListBox.DataSource = value; } }
        public EditorWorldMap selectedWorldMap { get { return worldMapListBox.SelectedItem as EditorWorldMap; } }

        public WorldMapView()
        {
            InitializeComponent();
            worldMapDisplay1.view = this;
        }

        public void selectWorldMap(EditorWorldMap worldMap)
        {
            worldMapListBox.SelectedIndex = _controller.worldMaps.IndexOf(worldMap);
        }

        private void worldMapListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            EditorWorldMap selectedWorldMap = worldMapListBox.SelectedItem as EditorWorldMap;

            worldMapPropertyGrid.SelectedObject = selectedWorldMap;
        }

        private void addWorldMapButton_Click(object sender, EventArgs e)
        {
            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                EditorWorldMap worldMap = _controller.createWorldMap(createResourceView.uid);
                worldMapListBox.RefreshItems();

                if (worldMap != null)
                    selectWorldMap(worldMap);
            }
        }

        private void removeWorldMapButton_Click(object sender, EventArgs e)
        {
            if (selectedWorldMap == null)
                return;

            try
            {
                _controller.removeWorldMap(selectedWorldMap.uid, true);
                worldMapListBox.RefreshItems();
            }
            catch (InvalidResourceException exception)
            {
                MessageBox.Show(exception.Message, "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ResourceNotFoundException exception)
            {
                MessageBox.Show(exception.Message, "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveWorldMapsButton_Click(object sender, EventArgs e)
        {
            _controller.saveWorldMaps();
        }

        private void createLevelIconButton_Click(object sender, EventArgs e)
        {
            EditorLevelIcon levelIcon = new EditorLevelIcon(selectedWorldMap, worldMapDisplay1.mouseWorld, "level_unfinished_icon", "level_finished_icon", "default_level", _controller.getUnusedId(selectedWorldMap));
            _controller.addLevelIcon(levelIcon);
            _controller.selectedControl = levelIcon;
        }

        private void createPathButton_Click(object sender, EventArgs e)
        {
            _controller.drawingWorldPath = true;
        }
    }
}
