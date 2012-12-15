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
        private EditBlueprintScrapShape _editBlueprintScrapView;

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

        // setEditBlueprintScrapView
        public void setEditBlueprintScrapView(EditBlueprintScrapShape view)
        {
            _editBlueprintScrapView = view;
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
            saveButton.Enabled = status;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            if (_editBlueprintScrapView != null)
                _editBlueprintScrapView.handleXNADraw();
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
            propertiesContainer.Controls.Add(new ItemPropertiesControl(this, itemResource.generalProperties));

            switch (itemResource.type)
            {
                case ItemType.RopeGun:
                    RopeGunItemResource ropeGunItemResource = itemResource as RopeGunItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, ropeGunItemResource.ropeGunProperties));
                    break;

                case ItemType.GravityGun:
                    GravityGunItemResource gravityGunItemResource = itemResource as GravityGunItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, gravityGunItemResource.gravityGunProperties));
                    break;

                case ItemType.Grenade:
                    GrenadeItemResource grenadeItemResource = itemResource as GrenadeItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, grenadeItemResource.grenadeProperties));
                    break;

                case ItemType.HealthPotion:
                    HealthPotionItemResource healthPotionItemResource = itemResource as HealthPotionItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, healthPotionItemResource.healthPotionProperties));
                    break;

                case ItemType.TreeSeed:
                    TreeSeedItemResource treeSeedItemResource = itemResource as TreeSeedItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, treeSeedItemResource.generalPlantProperties));
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, treeSeedItemResource.treeProperties));
                    break;

                case ItemType.BlueprintScrap:
                    BlueprintScrapItemResource blueprintScrapItemResource = itemResource as BlueprintScrapItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, blueprintScrapItemResource.blueprintScrapProperties));
                    propertiesContainer.Controls.Add(new CreateBlueprintScrapShapeButton(this, blueprintScrapItemResource));
                    break;

                case ItemType.Blueprint:
                    BlueprintItemResource blueprintItemResource = itemResource as BlueprintItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(this, blueprintItemResource.blueprintProperties));
                    break;
            }
        }
    }
}
