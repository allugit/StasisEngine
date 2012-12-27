using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Controllers;
using StasisEditor.Controllers.Actors;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public class LevelView : GraphicsDeviceControl
    {
        private LevelController _controller;
        private Texture2D _playerSpawnIcon;
        private Texture2D _timerIcon;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;

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
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });

            // Draw loop
            Application.Idle += delegate { Invalidate(); };

            // Input
            MouseMove += new MouseEventHandler(LevelView_MouseMove);
        }

        // Mouse move
        void LevelView_MouseMove(object sender, MouseEventArgs e)
        {
            _controller.mouse = e.Location;
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            if (_controller.level != null)
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
            List<ActorResourceController> actorControllers = _controller.getActorControllers();
            foreach (ActorResourceController actorController in actorControllers)
                actorController.draw();
        }
    }
}
