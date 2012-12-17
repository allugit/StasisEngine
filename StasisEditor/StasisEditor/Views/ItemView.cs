using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;
using StasisCore.Models;

namespace StasisEditor.Views
{
    public partial class ItemView : UserControl
    {
        private ItemController _controller;
        private BindingSource _itemListSource;
        private EditBlueprintScrapShape _editBlueprintScrapShapeView;
        private EditBlueprintScrapSocketsView _editBlueprintScrapSocketsView;

        public ItemView()
        {
            InitializeComponent();

            // Initialize items binding source
            _itemListSource = new BindingSource();
            itemListBox.DataSource = _itemListSource;

            // Set item types
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
                itemTypeListBox.Items.Add(type);
        }

        // setController
        public void setController(ItemController controller)
        {
            _controller = controller;
        }

        // getController
        public ItemController getController()
        {
            return _controller;
        }

        // setEditBlueprintScrapShapeView
        public void setEditBlueprintScrapShapeView(EditBlueprintScrapShape view)
        {
            _editBlueprintScrapShapeView = view;
        }

        // setEditBlueprintScrapSocketsView
        public void setEditBlueprintScrapSocketsView(EditBlueprintScrapSocketsView view)
        {
            _editBlueprintScrapSocketsView = view;
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
            saveButton.Enabled = status;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            if (_editBlueprintScrapShapeView != null)
                _editBlueprintScrapShapeView.handleXNADraw();

            if (_editBlueprintScrapSocketsView != null)
                _editBlueprintScrapSocketsView.handleXNADraw();
        }

        // updateMousePosition
        public void updateMousePosition()
        {
            if (_editBlueprintScrapShapeView != null)
                _editBlueprintScrapShapeView.updateMousePosition();

            if (_editBlueprintScrapSocketsView != null)
                _editBlueprintScrapSocketsView.updateMousePosition();
        }

        // Selected item type changed
        private void itemTypeListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            _itemListSource.DataSource = _controller.getItems((ItemType)itemTypeListBox.SelectedItem);
        }

        // Selected item changed
        private void itemListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            // Clear properties container
            propertiesContainer.Controls.Clear();

            if (itemListBox.SelectedItem == null)
                return;

            // Add item properties to the property container
            ItemResource itemResource = itemListBox.SelectedItem as ItemResource;
            propertiesContainer.Controls.Add(new ItemPropertiesControl(this, itemResource));

            switch (itemResource.type)
            {
                case ItemType.BlueprintScrap:
                    BlueprintScrapItemResource blueprintScrapItemResource = itemResource as BlueprintScrapItemResource;
                    propertiesContainer.Controls.Add(new CreateBlueprintScrapShapeButton(this, blueprintScrapItemResource));
                    break;

                case ItemType.Blueprint:
                    BlueprintItemResource blueprintItemResource = itemResource as BlueprintItemResource;
                    List<BlueprintScrapItemResource> associatedScraps = _controller.getBlueprintScrapResources(blueprintItemResource.tag);
                    propertiesContainer.Controls.Add(new ViewBlueprintAssociateScraps(associatedScraps));
                    propertiesContainer.Controls.Add(new EditBlueprintScrapSocketsButton(this, associatedScraps));
                    break;
            }
        }
    }
}
