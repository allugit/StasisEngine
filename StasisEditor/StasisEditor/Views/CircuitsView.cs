using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Views.Controls;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public class CircuitsView : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private CircuitController _controller;
        private bool _active;
        private Texture2D _pixel;

        public bool active { get { return _active; } set { _active = value; } }

        public CircuitsView()
            : base()
        {
            
        }

        // Initialize
        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
        }

        // setController
        public void setController(CircuitController controller)
        {
            _controller = controller;
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            if (_active)
            {
                _spriteBatch.Begin();

                // Draw grid
                drawGrid();

                // Draw mouse position
                drawMousePosition();

                _spriteBatch.End();
            }
        }

        // drawGrid
        private void drawGrid()
        {
            // Draw grid
            int screenWidth = Width;
            int screenHeight = Height;
            Vector2 worldOffset = _controller.getWorldOffset();
            float scale = _controller.getScale();
            Rectangle destRect = new Rectangle(0, 0, (int)(screenWidth + scale), (int)(screenHeight + scale));
            Vector2 gridOffset = new Vector2((worldOffset.X * scale) % scale, (worldOffset.Y * scale) % scale);
            Color color = Color.Black;

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += scale)
                _spriteBatch.Draw(_pixel, new Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += scale)
                _spriteBatch.Draw(_pixel, new Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);
        }

        // drawMousePosition
        private void drawMousePosition()
        {
            float scale = _controller.getScale();
            Vector2 worldOffset = _controller.getWorldOffset();
            Vector2 worldMouse = _controller.getWorldMouse();

            _spriteBatch.Draw(
                _pixel,
                (worldMouse + worldOffset) * scale,
                new Microsoft.Xna.Framework.Rectangle(0, 0, 8, 8),
                Microsoft.Xna.Framework.Color.Yellow, 0, new Vector2(4, 4),
                1f,
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                0);

        }
    }
}
