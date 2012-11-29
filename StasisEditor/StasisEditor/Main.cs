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

namespace StasisEditor
{
    public class Main : Game
    {
        public GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static GraphicsDevice graphicsDevice;
        public static Editor editor;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            editor = new Editor(this);
        }

        // Initialize
        protected override void Initialize()
        {
            // XNA
            IsMouseVisible = true;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            // Base initialize -- calls LoadContent
            base.Initialize();
        }

        // LoadContent
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice = GraphicsDevice;
        }

        // UnloadContent
        protected override void UnloadContent()
        {
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
