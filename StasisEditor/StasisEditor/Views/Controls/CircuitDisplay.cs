using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Views.Controls;
using StasisEditor.Controllers;

namespace StasisEditor.Views.Controls
{
    public class CircuitDisplay : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;
        private Texture2D _pixel;
        private Texture2D _and;
        private Texture2D _or;
        private Texture2D _not;
        private CircuitsView _view;

        public CircuitDisplay()
            : base()
        {
        }

        // Initialize
        protected override void Initialize()
        {
            if (!IsDesignerHosted)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _contentManager = new ContentManager(Services, "Content");
                _view = Parent.Parent.Parent as CircuitsView;
                System.Diagnostics.Debug.Assert(Parent.Parent.Parent is CircuitsView);

                // Resources
                _pixel = new Texture2D(GraphicsDevice, 1, 1);
                _pixel.SetData<Color>(new[] { Color.White });
                _and = _contentManager.Load<Texture2D>("logic_gate_icons\\and");
                _or = _contentManager.Load<Texture2D>("logic_gate_icons\\or");
                _not = _contentManager.Load<Texture2D>("logic_gate_icons\\not");

                // Events
                Application.Idle += delegate { Invalidate(); };
                MouseMove += new System.Windows.Forms.MouseEventHandler(CircuitsView_MouseMove);
                FindForm().KeyDown += new KeyEventHandler(Parent_KeyDown);
                FindForm().KeyUp += new KeyEventHandler(Parent_KeyUp);
            }
        }

        // Mouse move
        void CircuitsView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _view.controller.handleMouseMove(e);
        }

        // Key up
        void Parent_KeyUp(object sender, KeyEventArgs e)
        {
            if (_view.keysEnabled)
            {
                if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    _view.controller.shift = false;
                else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _view.controller.ctrl = false;
            }
        }

        // Key down
        void Parent_KeyDown(object sender, KeyEventArgs e)
        {
            if (_view.keysEnabled)
            {
                if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    _view.controller.shift = true;
                else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _view.controller.ctrl = true;
            }
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            if (_view != null && _view.draw)
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
            Vector2 worldOffset = _view.controller.getWorldOffset();
            float scale = _view.controller.getScale();
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
            float scale = _view.controller.getScale();
            Vector2 worldOffset = _view.controller.getWorldOffset();
            Vector2 worldMouse = _view.controller.getWorldMouse();

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
