using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StasisEditor.View;

namespace StasisEditor.Controller
{
    public class XNAController : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private EditorController _editorController;

        public XNAController()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Hide the main XNA game window
            System.Windows.Forms.Control.FromHandle(Window.Handle).VisibleChanged += new EventHandler(visibleChanged);
        }

        // Keep the main XNA game window hidden
        private void visibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle(Window.Handle).Visible)
                System.Windows.Forms.Control.FromHandle(Window.Handle).Visible = false;
        }

        // resizeGraphicsDevice
        public void resizeGraphicsDevice(int width, int height)
        {
            // Resize graphics device
            if (width > 0 && height > 0)
            {
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
                graphics.ApplyChanges();
            }
        }

        // Initialize
        protected override void Initialize()
        {
            // XNA
            IsMouseVisible = true;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            // Base initialize -- calls LoadContent
            base.Initialize();

            // Create the editor view and controller
            EditorView editorView = new EditorView();
            _editorController = new EditorController(this, editorView);
        }

        // LoadContent
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            XNAResources.initialize(this);
        }

        // UnloadContent
        protected override void UnloadContent()
        {
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            // Update input
            Input.update();

            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            _editorController.handleXNADraw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
