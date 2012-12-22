using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public partial class BlueprintView : UserControl
    {
        private BlueprintController _controller;
        private EditBlueprintScrapShape _editBlueprintScrapShapeView;
        private EditBlueprintSocketsView _editBlueprintSocketsView;

        public EditBlueprintScrapShape editBlueprintScrapShapeView { get { return _editBlueprintScrapShapeView; } set { _editBlueprintScrapShapeView = value; } }
        public EditBlueprintSocketsView editBlueprintSocketsView { get { return _editBlueprintSocketsView; } set { _editBlueprintSocketsView = value; } }
        public Blueprint selectedBlueprint { get { return blueprintList.SelectedItem as Blueprint; } }
        public BlueprintScrap selectedScrap { get { return scrapList.SelectedItem as BlueprintScrap; } }

        public BlueprintView()
        {
            InitializeComponent();
        }

        // Set controller
        public void setController(BlueprintController controller)
        {
            _controller = controller;
            blueprintList.DataSource = _controller.blueprints;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            if (_editBlueprintScrapShapeView != null)
                _editBlueprintScrapShapeView.handleXNADraw();

            if (_editBlueprintSocketsView != null)
                _editBlueprintSocketsView.handleXNADraw();
        }

        // updateMousePosition
        public void updateMousePosition()
        {
            if (_editBlueprintScrapShapeView != null)
                _editBlueprintScrapShapeView.updateMousePosition();

            if (_editBlueprintSocketsView != null)
                _editBlueprintSocketsView.updateMousePosition();
        }

        // selectBlueprint
        public void selectBlueprint(Blueprint blueprint)
        {
            blueprintList.SelectedIndex = _controller.blueprints.IndexOf(blueprint);
        }

        // selectBlueprintScrap
        public void selectBlueprintScrap(BlueprintScrap scrap)
        {
            scrapList.SelectedIndex = selectedBlueprint.scraps.IndexOf(scrap);
        }

        // Add blueprint
        private void addBlueprintButton_Click(object sender, EventArgs e)
        {
            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                Blueprint blueprint = _controller.createBlueprint(createResourceView.uid);
                blueprintList.RefreshItems();

                if (blueprint != null)
                    selectBlueprint(blueprint);
            }
        }

        // Add blueprint scrap
        private void addScrapButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(selectedBlueprint != null);

            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                BlueprintScrap scrap = _controller.createBlueprintScrap(selectedBlueprint, createResourceView.uid);
                scrapList.RefreshItems();

                if (scrap != null)
                    selectBlueprintScrap(scrap);
            }
        }

        // Selected blueprint changed
        private void blueprintList_SelectedValueChanged(object sender, EventArgs e)
        {
            removeBlueprintButton.Enabled = selectedBlueprint != null;
            arrangeScrapsButton.Enabled = selectedBlueprint != null;
            addScrapButton.Enabled = selectedBlueprint != null;
            scrapList.DataSource = selectedBlueprint.scraps;
        }

        // Selected scrap changed
        private void scrapList_SelectedValueChanged(object sender, EventArgs e)
        {
            removeScrapButton.Enabled = selectedScrap != null;
            defineShapeButton.Enabled = selectedScrap != null;
        }
    }
}
