using System;
using System.Collections.Generic;
using StasisEditor.Controller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.View
{
    public class XNAResources
    {
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
        public static SpriteBatch spriteBatch;
        public static Texture2D pixel;
        public static SpriteFont arial;

        public static void initialize(XNAController controller)
        {
            graphics = controller.graphics;
            graphicsDevice = controller.GraphicsDevice;
            spriteBatch = controller.spriteBatch;

            // Pixel
            pixel = new Texture2D(controller.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            // Arial
            arial = controller.Content.Load<SpriteFont>("arial");
        }
    }
}
