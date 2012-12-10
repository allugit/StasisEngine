using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;

namespace StasisEditor.Views
{
    public partial class ItemView : UserControl
    {
        public ItemView()
        {
            InitializeComponent();

            // Set item types
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
                itemTypeListBox.Items.Add(type);
        }
    }
}
