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
using StasisCore;

namespace StasisEditor.Views
{
    using Vector2 = Microsoft.Xna.Framework.Vector2;
    using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
    using Keys = Microsoft.Xna.Framework.Input.Keys;
    using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

    public partial class BackgroundView : UserControl
    {
        private BackgroundController _controller;
        private bool _draw;
        private bool _keysEnabled;

        public bool active
        {
            get { return _draw && _keysEnabled; }
            set { _draw = value; _keysEnabled = value; }
        }
        public bool keysEnabled { get { return _keysEnabled; } }
        public BackgroundController controller { get { return _controller; } set { _controller = value; } }
        public BindingList<EditorBackground> backgrounds { set { backgroundList.DataSource = value; } }
        public EditorBackground selectedBackground { get { return backgroundList.SelectedItem as EditorBackground; } }
        public EditorBackgroundLayer selectedBackgroundLayer { get { return layerList.SelectedItem as EditorBackgroundLayer; } }
        

        public BackgroundView()
        {
            InitializeComponent();
            backgroundDisplay.view = this;
            verticalScrollList.SelectedIndex = 0;
            horizontalScrollList.SelectedIndex = 0;

            Application.Idle += new EventHandler(updateScreenPosition);
        }

        // Update screen position based on the scrolling types
        void updateScreenPosition(object sender, EventArgs e)
        {
            if (_controller != null)
            {
                Vector2 delta = Vector2.Zero;
                string verticalScroll = verticalScrollList.SelectedItem as string;
                string horizontalScroll = horizontalScrollList.SelectedItem as string;
                float speed = (float)scrollSpeed.Value;

                if (verticalScroll == "Up")
                    delta.Y -= speed;
                else if (verticalScroll == "Down")
                    delta.Y += speed;

                if (horizontalScroll == "Left")
                    delta.X -= speed;
                else if (horizontalScroll == "Right")
                    delta.X += speed;

                _controller.screenCenter += delta;
            }
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
            EditorBackground background = selectedBackground;

            if (background != null)
            {
                layerList.DataSource = selectedBackground.layers;
            }
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
            layerList.RefreshItems();
        }

        // Remove background layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            EditorBackground background = selectedBackground;
            EditorBackgroundLayer layer = selectedBackgroundLayer;
            int selectedIndex;

            if (background == null || layer == null)
                return;

            selectedIndex = layerList.Items.IndexOf(layer);
            background.layers.Remove(layer);
            layerList.DataSource = null;
            layerList.DataSource = background.layers;
        }

        // Property changed
        private void layerProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            layerList.RefreshItems();
        }

        // Save backgrounds
        private void saveBackgroundsButton_Click(object sender, EventArgs e)
        {
            _controller.saveBackgrounds();
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            EditorBackground background = selectedBackground;
            _controller.screenCenter = Vector2.Zero;

            if (background == null)
                return;

            try
            {
                background.loadTextures();
                backgroundDisplay.previewBackground(background.clone() as EditorBackground);
            }
            catch (ResourceNotFoundException ex)
            {
                MessageBox.Show(String.Format("Error previewing the background: {0}", ex.Message));
            }
        }
    }
}
