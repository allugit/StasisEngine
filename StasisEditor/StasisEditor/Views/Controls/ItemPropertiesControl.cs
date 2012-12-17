using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    public partial class ItemPropertiesControl : UserControl
    {
        private ItemView _itemView;

        public ItemPropertiesControl(ItemView itemView, ItemResource itemResource)
        {
            _itemView = itemView;

            InitializeComponent();
            Dock = DockStyle.Fill;

            propertyGrid.SelectedObject = itemResource;
        }

        // Property value changed
        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _itemView.setChangesMade(true);
        }
    }
}
