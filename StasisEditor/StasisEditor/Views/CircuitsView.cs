using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
        private bool _draw;
        private Texture2D _pixel;
        private bool _keysEnabled;

        public bool active
        { 
            get { return _draw && _keysEnabled; }
            set { _draw = value; _keysEnabled = value; }
        }

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

            Application.Idle += delegate { Invalidate(); };
            MouseMove += new System.Windows.Forms.MouseEventHandler(CircuitsView_MouseMove);
            FindForm().KeyDown += new KeyEventHandler(Parent_KeyDown);
            FindForm().KeyUp += new KeyEventHandler(Parent_KeyUp);
        }

        // setController
        public void setController(CircuitController controller)
        {
            _controller = controller;
        }

        // Mouse move
        void CircuitsView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _controller.handleMouseMove(e);
        }

        // Key up
        void Parent_KeyUp(object sender, KeyEventArgs e)
        {
            if (_keysEnabled)
            {
                if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    _controller.shift = false;
                else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _controller.ctrl = false;
            }
        }

        // Key down
        void Parent_KeyDown(object sender, KeyEventArgs e)
        {
            if (_keysEnabled)
            {
                if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    _controller.shift = true;
                else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _controller.ctrl = true;
            }
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            if (_draw)
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
