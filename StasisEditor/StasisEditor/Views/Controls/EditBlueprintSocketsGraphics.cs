using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;
using StasisCore.Controllers;

namespace StasisEditor.Views.Controls
{
    public class EditBlueprintSocketsGraphics : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Blueprint _blueprint;
        private Vector2 _mouse;
        private EditBlueprintSocketsView _view;
        private Dictionary<string, Vector2> _scrapPositions;

        public Blueprint blueprint { get { return _blueprint; } set { _blueprint = value; } }

        public EditBlueprintSocketsGraphics()
            : base()
        {
            _scrapPositions = new Dictionary<string, Vector2>();

            System.Windows.Forms.Application.Idle += delegate { Invalidate(); };
            MouseMove += new System.Windows.Forms.MouseEventHandler(EditBlueprintSocketsGraphics_MouseMove);
            MouseDown += new System.Windows.Forms.MouseEventHandler(EditBlueprintSocketsGraphics_MouseDown);
        }

        // Mouse down
        void EditBlueprintSocketsGraphics_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Hit test scraps
            BlueprintScrap target = null;
            for (int i = 0; i < _blueprint.scraps.Count; i++)
            {
                if (_blueprint.scraps[i].hitTest(_mouse))
                {
                    target = _blueprint.scraps[i];
                    break;
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_view.selectedScrap == null)
                {
                    // Select scrap
                    _view.selectedScrap = target;
                }
                else
                {
                    // Place selected scrap
                    _view.selectedScrap = null;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (_view.socketTargetA == null)
                {
                    // Set first socket target
                    _view.socketTargetA = target;
                }
                else
                {
                    // Create socket on first target
                    Vector2 relativePoint = _scrapPositions[target.uid] - _scrapPositions[_view.socketTargetA.uid];
                    BlueprintSocket firstSocket = new BlueprintSocket(_view.socketTargetA, target, relativePoint);
                    _blueprint.sockets.Add(firstSocket);

                    // Create socket on second target
                    BlueprintSocket secondSocket = new BlueprintSocket(target, _view.socketTargetA, -relativePoint);
                    _blueprint.sockets.Add(secondSocket);

                    // Set opposing sockets
                    firstSocket.opposingSocket = secondSocket;
                    secondSocket.opposingSocket = firstSocket;

                    // Clear socket target
                    _view.socketTargetA = null;
                }
            }
        }

        // Mouse move
        void EditBlueprintSocketsGraphics_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Vector2 oldMouse = _mouse;
            _mouse = new Vector2(e.X, e.Y);

            if (_view.selectedScrap != null)
                _scrapPositions[_view.selectedScrap.uid] += _mouse - oldMouse;
        }

        // Initialize
        protected override void Initialize()
        {
            _view = Parent as EditBlueprintSocketsView;

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });

            // Initialize blueprint scraps
            foreach (BlueprintScrap scrap in _blueprint.scraps)
            {
                // Texture
                if (scrap.scrapTexture == null)
                    scrap.scrapTexture = ResourceController.getTexture(scrap.scrapTextureUID);
                scrap.textureCenter = new Vector2(scrap.scrapTexture.Width, scrap.scrapTexture.Height) / 2;

                // Copy scrap positions
                _scrapPositions[scrap.uid] = scrap.currentCraftPosition;
            }
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            if (_blueprint != null)
            {
                _spriteBatch.Begin();

                // Draw scraps
                for (int i = _blueprint.scraps.Count - 1; i >= 0; i--)
                    _spriteBatch.Draw(_blueprint.scraps[i].scrapTexture, _scrapPositions[_blueprint.scraps[i].uid], _blueprint.scraps[i].scrapTexture.Bounds, Color.White, 0f, _blueprint.scraps[i].textureCenter, 1f, SpriteEffects.None, 0);

                // Draw scrap sockets
                foreach (BlueprintSocket socket in _blueprint.sockets)
                {
                    drawLine(
                        _scrapPositions[socket.scrapA.uid],
                        _scrapPositions[socket.scrapB.uid] + socket.relativePoint,
                        Color.Green);
                }

                // Draw mouse position
                _spriteBatch.Draw(_pixel, new Vector2(_mouse.X, _mouse.Y), new Rectangle(0, 0, 4, 4), Color.Yellow, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);

                if (_view.socketTargetA != null)
                {
                    // Draw socket line
                    drawLine(_scrapPositions[_view.socketTargetA.uid], _mouse, Color.Blue);
                }

                _spriteBatch.End();
            }
        }

        // drawLine
        private void drawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            Vector2 relative = pointB - pointA;
            float length = relative.Length();
            float angle = (float)Math.Atan2(relative.Y, relative.X);
            Rectangle rect = new Rectangle(0, 0, (int)length, 2);
            _spriteBatch.Draw(_pixel, pointA, rect, color, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
        }
    }
}
