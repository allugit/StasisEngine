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

        // setChangesMade
        public void setChangesMade(bool status)
        {
            saveButton.Enabled = status;
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
            }
        }
    }
}
