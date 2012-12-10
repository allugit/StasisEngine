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

        // Selected item type changed
        private void itemTypeListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            _itemListSource.DataSource = _controller.getItems((ItemType)itemTypeListBox.SelectedItem);
        }

        // Selected item changed
        private void itemListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (itemListBox.SelectedItem == null)
                return;

            // Clear properties container
            propertiesContainer.Controls.Clear();

            // Add item properties to the property container
            ItemResource itemResource = itemListBox.SelectedItem as ItemResource;
            propertiesContainer.Controls.Add(new ItemPropertiesControl(itemResource.generalProperties));

            switch (itemResource.type)
            {
                case ItemType.RopeGun:
                    RopeGunItemResource ropeGunItemResource = itemResource as RopeGunItemResource;
                    propertiesContainer.Controls.Add(new ItemPropertiesControl(ropeGunItemResource.ropeGunProperties));
                    break;
            }
        }
    }
}
