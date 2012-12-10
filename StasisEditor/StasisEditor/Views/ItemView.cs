using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
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

        // Item type changed
        private void itemTypeListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            _itemListSource.DataSource = _controller.getItems((ItemType)itemTypeListBox.SelectedItem);
        }
    }
}
