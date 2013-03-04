using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Models;
using StasisEditor.Views.Controls;
using StasisCore.Controllers;

namespace StasisEditor.Views
{
    public partial class BackgroundView : UserControl
    {
        private BackgroundController _controller;

        public BackgroundController controller { get { return _controller; } set { _controller = value; } }
        public BindingList<EditorBackground> backgrounds { set { backgroundList.DataSource = value; } }
        public EditorBackground selectedBackground { get { return backgroundList.SelectedItem as EditorBackground; } }
        public EditorBackgroundLayer selectedBackgroundLayer { get { return layerList.SelectedItem as EditorBackgroundLayer; } }

        public BackgroundView()
        {
            InitializeComponent();
        }

        // Add new background
        private void addBackgroundButton_Click(object sender, EventArgs e)
        {
            CreateResourceView createResourceView = new CreateResourceView();
            if (createResourceView.ShowDialog() == DialogResult.OK)
            {
                EditorBackground background = new EditorBackground(createResourceView.uid);
                _controller.backgrounds.Add(background);
            }
        }

        // Remove background
        private void removeBackgroundButton_Click(object sender, EventArgs e)
        {
            if (selectedBackground != null)
            {
                _controller.backgrounds.Remove(selectedBackground);
            }
        }

        // Selected background changed
        private void backgroundList_SelectedValueChanged(object sender, EventArgs e)
        {
            layerList.DataSource = selectedBackground.layers;
        }

        // Selected layer changed
        private void layerList_SelectedValueChanged(object sender, EventArgs e)
        {
            layerProperties.SelectedObject = selectedBackgroundLayer;
        }

        // Add background layer
        private void addBackgroundLayer_Click(object sender, EventArgs e)
        {
            EditorBackground background = selectedBackground;
            EditorBackgroundLayer layer;

            if (background == null)
                return;

            layer = new EditorBackgroundLayer();
            background.layers.Add(layer);

            layerList.SelectedItem = layer;
        }

        // Remove background layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            EditorBackground background = selectedBackground;
            EditorBackgroundLayer layer = selectedBackgroundLayer;

            if (background == null || layer == null)
                return;

            background.layers.Remove(layer);
        }
    }
}
