using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisCore.Resources;
using StasisCore.Controllers;

namespace StasisEditor.Views.Controls
{
    public class MaterialPreviewGraphics : GraphicsDeviceControl
    {
        private MaterialRenderer _materialRenderer;
        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;

        public MaterialPreviewGraphics()
            : base()
        {
        }

        protected override void Initialize()
        {
            _contentManager = new ContentManager(Services, "Content");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            try
            {
                _materialRenderer = new MaterialRenderer(GraphicsDevice, _contentManager, _spriteBatch);
            }
            catch (ContentLoadException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            Console.WriteLine("Material preview graphics completely initialized.");
        }

        public void setMaterial(Material material, float growthFactor)
        {
            try
            {
                _texture = _materialRenderer.renderMaterial(material, growthFactor, 512, 512);
                Invalidate();
            }
            catch (ResourceNotFoundException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Resource Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            if (_texture != null)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_texture, _texture.Bounds, Color.White);
                _spriteBatch.End();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _contentManager.Unload();

            base.Dispose(disposing);
        }
    }
}
