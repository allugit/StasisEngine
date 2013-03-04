using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor.Views.Controls
{
    public class BackgroundDisplay : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;

        public BackgroundDisplay()
        {
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.End();
        }
    }
}
