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
        private ItemProperties _itemProperties;

        public ItemPropertiesControl(ItemProperties itemProperties)
        {
            _itemProperties = itemProperties;

            InitializeComponent();
            Dock = DockStyle.Top;

            propertyGrid.SelectedObject = itemProperties;
            propertiesName.Text = itemProperties.ToString();
        }
    }
}
