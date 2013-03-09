using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Resources;
using StasisCore.Controllers;
using StasisEditor.Controllers;
using StasisEditor.Models;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    using Vector2 = Microsoft.Xna.Framework.Vector2;
    using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
    using Keys = Microsoft.Xna.Framework.Input.Keys;
    using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

    public partial class BackgroundView : UserControl
    {
        private BackgroundController _controller;
        private Vector2 _screenOffset;

        public BackgroundController controller { get { return _controller; } set { _controller = value; } }
        public BindingList<EditorBackground> backgrounds { set { backgroundList.DataSource = value; } }
        public EditorBackground selectedBackground { get { return backgroundList.SelectedItem as EditorBackground; } }
        public EditorBackgroundLayer selectedBackgroundLayer { get { return layerList.SelectedItem as EditorBackgroundLayer; } }
        public Vector2 screenOffset { get { return _screenOffset; } set { _screenOffset = value; } }

        public BackgroundView()
        {
            InitializeComponent();
            backgroundDisplay.view = this;

            Application.Idle += new EventHandler(updateInput);
        }

        void updateInput(object sender, EventArgs e)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _screenOffset += new Vector2(1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _screenOffset -= new Vector2(1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _screenOffset += new Vector2(0, 1);
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                _screenOffset -= new Vector2(0, 1);
            }
            if (keyboardState.IsKeyDown(Keys.Home))
            {
                _screenOffset = Vector2.Zero;
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
