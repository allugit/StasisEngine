using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    using Keys = System.Windows.Forms.Keys;

    public class WorldMapDisplay : GraphicsDeviceControl
    {
        private WorldMapView _view;
        private SpriteBatch _spriteBatch;
        private Rectangle _pointRect = new Rectangle(0, 0, 4, 4);
        private Rectangle _lineRect = new Rectangle(0, 0, 0, 2);
        private Vector2 _pointOrigin = new Vector2(2, 2);
        private Vector2 _lineOrigin = new Vector2(0, 1);
        private Texture2D _pixel;
        private Matrix _viewMatrix;
        private Matrix _invViewMatrix;
        private float _scale = 1f;
        private Vector2 _screenCenter;
        private bool _ctrl;
        private Point _mousePoint;
        private Point _oldMousePoint;

        public WorldMapView view { get { return _view; } set { _view = value; } }
        public Vector2 mouseWorld { get { return Vector2.Transform(new Vector2(_mousePoint.X, _mousePoint.Y), _invViewMatrix); } }

        public WorldMapDisplay()
        {
        }

        protected override void Initialize()
        {
            if (!IsDesignerHosted)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);

                System.Windows.Forms.Application.Idle += delegate { Invalidate(); };
                MouseMove += new System.Windows.Forms.MouseEventHandler(WorldMapDisplay_MouseMove);
                MouseClick += new System.Windows.Forms.MouseEventHandler(WorldMapDisplay_MouseClick);
                FindForm().KeyDown += new System.Windows.Forms.KeyEventHandler(Parent_KeyDown);
                FindForm().KeyUp += new System.Windows.Forms.KeyEventHandler(Parent_KeyUp);

                _pixel = new Texture2D(GraphicsDevice, 1, 1);
                _pixel.SetData<Color>(new[] { Color.White });
            }
        }

        void WorldMapDisplay_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _view.controller.handleClick(mouseWorld);
        }

        void WorldMapDisplay_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Vector2 delta;

            _oldMousePoint = _mousePoint;
            _mousePoint = new Point(e.X, e.Y);
            delta = new Vector2(_mousePoint.X - _oldMousePoint.X, _mousePoint.Y - _oldMousePoint.Y);

            if (_ctrl)
            {
                _screenCenter += delta;
            }
            else
            {
                _view.controller.moveSelectedControl(delta);
            }
        }

        // Key up
        void Parent_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                _ctrl = false;
        }

        // Key down
        void Parent_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                _ctrl = true;
        }

        public void drawPoint(Vector2 point, Color color)
        {
            _spriteBatch.Draw(_pixel, point, _pointRect, color, 0f, _pointOrigin, 1f, SpriteEffects.None, 0f);
        }

        public void drawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            Vector2 relative = pointB - pointA;
            float angle = (float)Math.Atan2(relative.Y, relative.X);

            _lineRect.Width = (int)relative.Length();
            _spriteBatch.Draw(_pixel, pointA, _lineRect, color, angle, _lineOrigin, 1f, SpriteEffects.None, 0f);
        }

        protected override void Draw()
        {
            if (!IsDesignerHosted)
            {
                _viewMatrix = Matrix.CreateTranslation(new Vector3(_screenCenter, 0));
                _viewMatrix *= Matrix.CreateScale(_scale);
                _viewMatrix *= Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0) / 2f);
                _invViewMatrix = Matrix.Invert(_viewMatrix);

                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, _viewMatrix);

                if (_view.selectedWorldMap != null)
                {
                    // Draw map texture
                    _spriteBatch.Draw(_view.selectedWorldMap.texture, Vector2.Zero, _view.selectedWorldMap.texture.Bounds, Color.White, 0f, new Vector2(_view.selectedWorldMap.texture.Width, _view.selectedWorldMap.texture.Height) / 2f, 1f / _scale, SpriteEffects.None, 1f);

                    // Draw level icons
                    foreach (LevelIcon levelIcon in _view.selectedWorldMap.levelIcons)
                    {
                        _spriteBatch.Draw(levelIcon.unfinishedIcon, levelIcon.position, levelIcon.unfinishedIcon.Bounds, Color.White, 0f, new Vector2(levelIcon.unfinishedIcon.Width, levelIcon.unfinishedIcon.Height) / 2f, 1f, SpriteEffects.None, 0.1f);
                    }

                    // Draw world path construction points
                    if (_view.controller.worldPathConstructionPoints.Count > 0)
                    {
                        drawLine(_view.controller.worldPathConstructionPoints[0], mouseWorld, Color.Blue);
                    }
                    foreach (Vector2 point in _view.controller.worldPathConstructionPoints)
                    {
                        drawPoint(point, Color.Blue);
                    }

                    // Draw world paths
                    Color pointColor = Color.LightBlue;
                    Color lineColor = Color.Blue * 0.5f;
                    Color controlLineColor = Color.Orange * 0.5f;
                    Color controlPointColor = Color.Yellow;
                    float interpolateIncrement = 0.001f;
                    foreach (WorldPath worldPath in _view.selectedWorldMap.worldPaths)
                    {
                        drawLine(worldPath.pointA.position, worldPath.pointB.position, lineColor);
                        drawLine(worldPath.pointA.position, worldPath.controlA.position, controlLineColor);
                        drawLine(worldPath.pointB.position, worldPath.controlB.position, controlLineColor);
                        drawPoint(worldPath.controlA.position, controlPointColor);
                        drawPoint(worldPath.controlB.position, controlPointColor);
                        drawPoint(worldPath.pointA.position, pointColor);
                        drawPoint(worldPath.pointB.position, pointColor);
                        for (float i = 0; i < 1f; i += interpolateIncrement)
                        {
                            Vector2 point = Vector2.CatmullRom(worldPath.controlA.position, worldPath.pointA.position, worldPath.pointB.position, worldPath.controlB.position, i);
                            drawPoint(point, Color.Yellow);
                        }
                    }
                }

                _spriteBatch.End();
            }
        }
    }
}
