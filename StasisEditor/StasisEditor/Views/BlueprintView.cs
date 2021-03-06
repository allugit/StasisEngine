﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;
using StasisEditor.Models;
using StasisCore;

namespace StasisEditor.Views
{
    public partial class BlueprintView : UserControl
    {
        private BlueprintController _controller;
        private EditBlueprintScrapShape _editBlueprintScrapShapeView;
        private EditBlueprintSocketsView _editBlueprintSocketsView;

        public EditBlueprintScrapShape editBlueprintScrapShapeView { get { return _editBlueprintScrapShapeView; } set { _editBlueprintScrapShapeView = value; } }
        public EditBlueprintSocketsView editBlueprintSocketsView { get { return _editBlueprintSocketsView; } set { _editBlueprintSocketsView = value; } }
        public EditorBlueprint selectedBlueprint { get { return blueprintList.SelectedItem as EditorBlueprint; } }
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

        // Get controller
        public BlueprintController getController()
        {
            return _controller;
        }

        // selectBlueprint
        public void selectBlueprint(EditorBlueprint blueprint)
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
                EditorBlueprint blueprint = _controller.createBlueprint(createResourceView.uid);
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
            // Update blueprint controls
            removeBlueprintButton.Enabled = selectedBlueprint != null;
            arrangeScrapsButton.Enabled = selectedBlueprint != null;
            addScrapButton.Enabled = selectedBlueprint != null;
            blueprintProperties.SelectedObject = selectedBlueprint;

            // Update scrap controls
            scrapList.DataSource = selectedBlueprint == null ? null : selectedBlueprint.scraps;
            scrapProperties.SelectedObject = (selectedBlueprint == null || selectedScrap == null) ? null : selectedScrap;
        }

        // Selected scrap changed
        private void scrapList_SelectedValueChanged(object sender, EventArgs e)
        {
            removeScrapButton.Enabled = selectedScrap != null;
            defineShapeButton.Enabled = selectedScrap != null;
            scrapProperties.SelectedObject = selectedScrap;
        }

        // Remove blueprint
        private void removeBlueprintButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(selectedBlueprint != null);

            try
            {
                _controller.removeBlueprint(selectedBlueprint.uid, true);
                //propertiesContainer.Controls.Clear();
                removeBlueprintButton.Enabled = false;
                blueprintList.RefreshItems();
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

        // Remove scrap
        private void removeScrapButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(selectedScrap != null);

            try
            {
                _controller.removeBlueprintScrap(selectedBlueprint, selectedScrap.uid);
                //propertiesContainer.Controls.Clear();
                removeScrapButton.Enabled = false;
                scrapList.RefreshItems();
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

        // Edit scrap shape
        private void defineShapeButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(selectedScrap != null);

            //try
            //{
                // Create edit view
                editBlueprintScrapShapeView = new EditBlueprintScrapShape(this, selectedScrap);

                // Open edit view
                if (editBlueprintScrapShapeView.ShowDialog() == DialogResult.OK)
                {
                    // Set scrap points
                    selectedScrap.points = editBlueprintScrapShapeView.getPoints();
                }

                // Close edit view
                editBlueprintScrapShapeView = null;
            //}
            //catch (ResourceNotFoundException ex)
            //{
            //    MessageBox.Show(ex.Message, "Resource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        // Arrange scraps button clicked
        private void arrangeScrapsButton_Click(object sender, EventArgs e)
        {

            // Create a copy of the blueprint
            EditorBlueprint copy = _controller.initializeBlueprint(selectedBlueprint.data);

            // Create edit view
            editBlueprintSocketsView = new EditBlueprintSocketsView(copy);

            // Open view
            if (editBlueprintSocketsView.ShowDialog() == DialogResult.OK)
            {
                _controller.swapBlueprint(selectedBlueprint, copy);
            }

            // Reset scraps
            foreach (BlueprintScrap scrap in selectedBlueprint.scraps)
            {
                scrap.currentCraftPosition = Vector2.Zero;
                scrap.connected.Clear();
            }

            // Reset sockets
            foreach (BlueprintSocket socket in selectedBlueprint.sockets)
            {
                socket.satisfied = false;
                socket.opposingSocket.satisfied = false;
            }

            // Close view
            editBlueprintSocketsView = null;
        }

        // Save button
        private void saveButton_Click(object sender, EventArgs e)
        {
            _controller.saveBlueprints();
        }
    }
}
