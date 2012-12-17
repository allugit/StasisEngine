using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Controllers;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public partial class ItemSelectBox : Form
    {
        private ItemController _itemController;

        public ItemSelectBox(ItemController itemController)
        {
            _itemController = itemController;

            InitializeComponent();

            allItemsListBox.DataSource = _itemController.getItems();
        }

        // List select button clicked
        private void listSelectButton_Click(object sender, EventArgs e)
        {
            EditorItem item = allItemsListBox.SelectedItem as EditorItem;

            // Check world texture
            Texture2D worldTexture = StasisCore.Controllers.TextureController.getTexture(item.itemResource.worldTextureTag);
            if (worldTexture == null)
            {
                MessageBox.Show("Item has no world texture.", "World Texture Not Found");
                return;
            }

            Console.WriteLine("select item.");
        }

        // Tag select button clicked
        private void tagSelectButton_Click(object sender, EventArgs e)
        {
            EditorItem item = _itemController.getItem(tagTextbox.Text);

            // Check tag
            if (item == null)
            {
                MessageBox.Show("That item does not exist.", "Item Not Found");
                return;
            }

            // Check world texture
            Texture2D worldTexture = StasisCore.Controllers.TextureController.getTexture(item.itemResource.worldTextureTag);
            if (worldTexture == null)
            {
                MessageBox.Show("Item has no world texture.", "World Texture Not Found");
                return;
            }

            Console.WriteLine("select item.");
        }
    }
}
