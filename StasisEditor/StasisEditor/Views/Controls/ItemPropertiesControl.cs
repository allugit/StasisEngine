using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class ItemPropertiesControl : UserControl
    {
        private ItemView _itemView;

        public ItemPropertiesControl(ItemView itemView, EditorItem item)
        {
            _itemView = itemView;

            InitializeComponent();
            Dock = DockStyle.Fill;

            propertyGrid.SelectedObject = item;
        }

        // Property value changed
        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _itemView.setChangesMade(true);
        }
    }
}
