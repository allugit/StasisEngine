using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisEditor.Controllers;
using StasisEditor.Controllers.Actors;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public class LevelView : GraphicsDeviceControl
    {
        private LevelController _controller;
        private ContentManager _contentManager;
        private Texture2D _playerSpawnIcon;
        private Texture2D _timerIcon;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Texture2D _circle;
        private bool _draw = true;
        private bool _keysEnabled = true;
        private bool _mouseOverView;

        public bool active
        {
            get { return _draw && _keysEnabled; }
            set { _draw = value; _keysEnabled = value; }
        }
        public bool shift { get { return _controller.shift; } }
        public bool ctrl { get { return _controller.ctrl; } }

        // setController
        public void setController(LevelController controller)
        {
            _controller = controller;
        }

        // Initialize
        protected override void Initialize()
        {
            // Resources
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _contentManager = new ContentManager(Services, "Content");
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
            _circle = _contentManager.Load<Texture2D>("circle");
            _playerSpawnIcon = _contentManager.Load<Texture2D>("actor_controller_icons\\player_spawn");
            _timerIcon = _contentManager.Load<Texture2D>("actor_controller_icons\\timer");

            // Draw loop
            Application.Idle += delegate { Invalidate(); };

            // Input
            MouseMove += new MouseEventHandler(LevelView_MouseMove);
            MouseDown += new MouseEventHandler(LevelView_MouseDown);
            FindForm().KeyDown += new KeyEventHandler(Parent_KeyDown);
            FindForm().KeyUp += new KeyEventHandler(Parent_KeyUp);
            MouseEnter += new EventHandler(LevelView_MouseEnter);
            MouseLeave += new EventHandler(LevelView_MouseLeave);
            MouseWheel += new MouseEventHandler(LevelView_MouseWheel);
        }

        // Mouse wheel
        void LevelView_MouseWheel(object sender, MouseEventArgs e)
        {
            _controller.zoom(e.Delta);
        }

        // Mouse leave
        void LevelView_MouseLeave(object sender, EventArgs e)
        {
            _mouseOverView = false;
        }

        // Mouse enter
        void LevelView_MouseEnter(object sender, EventArgs e)
        {
            _mouseOverView = true;
        }

        // Key up
        void Parent_KeyUp(object sender, KeyEventArgs e)
        {
            if (_mouseOverView && _keysEnabled)
            {
                if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    _controller.shift = false;
                else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _controller.ctrl = false;
            }
        }

        // Mouse down
        void LevelView_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
            _controller.handleMouseDown(e);
        }

        // Key down
        void Parent_KeyDown(object sender, KeyEventArgs e)
        {
            if (_mouseOverView && _keysEnabled)
            {
                if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    _controller.shift = true;
                else if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _controller.ctrl = true;

                // Pass input to all actors' global key handler
                foreach (ActorController actorController in _controller.getActorControllers())
                    actorController.globalKeyDown(e.KeyCode);

                // Pass input to selected sub actor controllers
                foreach (ActorSubController subController in _controller.selectedSubControllers)
                    subController.keyDown(e.KeyCode);

                // Refresh actor properties
                _controller.refreshActorProperties();
            }
        }

        // Mouse move
        void LevelView_MouseMove(object sender, MouseEventArgs e)
        {
            _controller.handleMouseMove(e);
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            if (_draw && _controller.level != null)
            {
                _spriteBatch.Begin();

                // Draw grid
                drawGrid();

                // Draw mouse position
                drawMousePosition();

                // Draw actor controllers
                drawActorControllers();

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
            Microsoft.Xna.Framework.Rectangle destRect = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)(screenWidth + scale), (int)(screenHeight + scale));
            Vector2 gridOffset = new Vector2((worldOffset.X * scale) % scale, (worldOffset.Y * scale) % scale);
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(new Vector3(0.2f, 0.2f, 0.2f));

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += scale)
                _spriteBatch.Draw(_pixel, new Microsoft.Xna.Framework.Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += scale)
                _spriteBatch.Draw(_pixel, new Microsoft.Xna.Framework.Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);
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

        // drawActorControllers
        private void drawActorControllers()
        {
            List<ActorController> actorControllers = _controller.getActorControllers();
            foreach (ActorController actorController in actorControllers)
                actorController.draw();
        }

        // drawBox
        public void drawBox(Vector2 position, float halfWidth, float halfHeight, float angle, Color color)
        {
            float scale = _controller.getScale();
            Rectangle rectangle = new Rectangle(0, 0, (int)(halfWidth * 2 * scale), (int)(halfHeight * 2 * scale));
            _spriteBatch.Draw(_pixel, (position + _controller.getWorldOffset()) * scale, rectangle, color, angle, new Vector2(halfWidth, halfHeight) * scale, 1, SpriteEffects.None, 0);
        }

        // drawLine
        public void drawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            float scale = _controller.getScale();
            Vector2 relative = pointB - pointA;
            float length = relative.Length();
            float angle = (float)Math.Atan2(relative.Y, relative.X);
            Rectangle rect = new Rectangle(0, 0, (int)(length * scale), 2);
            _spriteBatch.Draw(_pixel, (pointA + _controller.getWorldOffset()) * scale, rect, color, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
        }

        // drawPoint
        public void drawPoint(Vector2 position, Color color)
        {
            drawCircle(position, 4f / _controller.getScale(), color);
        }

        // drawCircle
        public void drawCircle(Vector2 position, float radius, Color color)
        {
            float circleScale = radius / (((float)_circle.Width / 2) / _controller.getScale());
            _spriteBatch.Draw(_circle, (position + _controller.getWorldOffset()) * _controller.getScale(), _circle.Bounds, color, 0, new Vector2(_circle.Width, _circle.Height) / 2, circleScale, SpriteEffects.None, 0);
        }

        // drawIcon
        public void drawIcon(ActorType actorType, Vector2 position)
        {
            Texture2D texture = _pixel;

            switch (actorType)
            {
                case ActorType.PlayerSpawn:
                    texture = _playerSpawnIcon;
                    break;
            }

            Rectangle rect = texture == _pixel ? new Rectangle(0, 0, 24, 24) : texture.Bounds;

            _spriteBatch.Draw(texture, (position + _controller.getWorldOffset()) * _controller.getScale(), rect, Color.White, 0, new Vector2(rect.Width, rect.Height) / 2, 1f, SpriteEffects.None, 0);
        }
    }
}
