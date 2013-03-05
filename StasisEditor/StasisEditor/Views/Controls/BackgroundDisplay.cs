using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Controllers;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public class BackgroundDisplay : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private BackgroundRenderer _backgroundRenderer;
        private BackgroundView _view;

        public BackgroundView view { get { return _view; } set { _view = value; } }

        public BackgroundDisplay()
        {
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _backgroundRenderer = new BackgroundRenderer(_spriteBatch);

            System.Windows.Forms.Application.Idle += delegate { Invalidate(); };
        }

        public void previewBackground(EditorBackground background)
        {
            _backgroundRenderer.background = background;
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            if (_backgroundRenderer.background != null)
            {
                _backgroundRenderer.draw(view.screenOffset);
            }

            _spriteBatch.End();
        }
    }
}
