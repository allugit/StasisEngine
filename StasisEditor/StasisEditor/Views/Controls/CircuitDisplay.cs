using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;
using StasisEditor.Views.Controls;
using StasisEditor.Controllers;

namespace StasisEditor.Views.Controls
{
    public class CircuitDisplay : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;
        private Texture2D _pixel;
        private Texture2D _circle;
        private Texture2D _and;
        private Texture2D _or;
        private Texture2D _not;
        private Texture2D _input;
        private Texture2D _output;
        private CircuitsView _view;
        private Gate _selectedGate;

        public Gate selectedGate { get { return _selectedGate; } set { _selectedGate = value; } }

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
                _input = _contentManager.Load<Texture2D>("logic_gate_icons\\input");
                _output = _contentManager.Load<Texture2D>("logic_gate_icons\\output");
                _circle = _contentManager.Load<Texture2D>("circle");

                // Events
                Application.Idle += delegate { Invalidate(); };
                MouseMove += new System.Windows.Forms.MouseEventHandler(CircuitsView_MouseMove);
                MouseDown += new MouseEventHandler(CircuitDisplay_MouseDown);
                FindForm().KeyDown += new KeyEventHandler(Parent_KeyDown);
                FindForm().KeyUp += new KeyEventHandler(Parent_KeyUp);
            }
        }

        // Mouse move
        void CircuitsView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Update mouse position
            _view.controller.mouse = e.Location;
            Vector2 worldDelta = _view.controller.getWorldMouse() - _view.controller.getOldWorldMouse();

            if (_view.controller.ctrl)
            {
                // Move screen
                _view.controller.screenCenter += worldDelta;
            }
            else
            {
                // Move gate
                if (selectedGate != null)
                {
                    selectedGate.position += worldDelta;
                }
            }
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

        // Mouse down
        void CircuitDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            Circuit circuit = _view.selectedCircuit;
            if (circuit == null)
                return;

            if (selectedGate == null)
            {
                // Hit test gates
                float maxDistance = 20f / _view.controller.getScale();
                foreach (Gate gate in circuit.gates)
                {
                    Vector2 relative = _view.controller.getWorldMouse() - gate.position;
                    if (relative.Length() <= maxDistance)
                    {
                        _selectedGate = gate;
                    }
                }
            }
            else
            {
                // Drop selected gate
                selectedGate = null;
            }

        }

        // Draw
        protected override void Draw()
        {
            if (_view != null && _view.draw)
            {
                if (_view.selectedCircuit == null)
                {
                    GraphicsDevice.Clear(Color.Black);
                }
                else
                {
                    GraphicsDevice.Clear(Color.DarkSlateBlue);

                    _spriteBatch.Begin();

                    // Draw grid
                    drawGrid();

                    // Draw mouse position
                    drawMousePosition();

                    // Draw selected circuit
                    drawCircuit();

                    _spriteBatch.End();
                }
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

        // drawCircuit
        private void drawCircuit()
        {
            Circuit circuit = _view.selectedCircuit;

            foreach (Gate gate in circuit.gates)
            {
                switch (gate.type)
                {
                    case "input":
                        _spriteBatch.Draw(_input, (gate.position + _view.controller.getWorldOffset()) * _view.controller.getScale(), _input.Bounds, Color.White, 0, new Vector2(_input.Width, _input.Height) / 2, 1f, SpriteEffects.None, 0);
                        break;

                    case "output":
                        _spriteBatch.Draw(_output, (gate.position + _view.controller.getWorldOffset()) * _view.controller.getScale(), _output.Bounds, Color.White, 0, new Vector2(_output.Width, _output.Height) / 2, 1f, SpriteEffects.None, 0);
                        break;

                    case "and":
                        _spriteBatch.Draw(_and, (gate.position + _view.controller.getWorldOffset()) * _view.controller.getScale(), _and.Bounds, Color.White, 0, new Vector2(_and.Width, _and.Height) / 2, 1f, SpriteEffects.None, 0);
                        break;

                    case "not":
                        _spriteBatch.Draw(_not, (gate.position + _view.controller.getWorldOffset()) * _view.controller.getScale(), _not.Bounds, Color.White, 0, new Vector2(_not.Width, _not.Height) / 2, 1f, SpriteEffects.None, 0);
                        break;

                    case "or":
                        _spriteBatch.Draw(_or, (gate.position + _view.controller.getWorldOffset()) * _view.controller.getScale(), _or.Bounds, Color.White, 0, new Vector2(_or.Width, _or.Height) / 2, 1f, SpriteEffects.None, 0);
                        break;
                }
            }
        }
    }
}
