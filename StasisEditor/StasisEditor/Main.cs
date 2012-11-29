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
        public SpriteBatch spriteBatch;
        public static Editor editor;
        public static Texture2D pixel;

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
            graphics.ApplyChanges();

            // Base initialize -- calls LoadContent
            base.Initialize();
        }

        // LoadContent
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(editor.main.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            editor.loadContent();
        }

        // UnloadContent
        protected override void UnloadContent()
        {
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            editor.update();

            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            editor.draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
