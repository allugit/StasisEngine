using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Controllers;
using StasisCore.Resources;

namespace StasisEditor.Views.Controls
{
    public class EditBlueprintScrapShapeGraphics : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private string _textureUID;
        private Texture2D _texture;
        private Vector2 _textureCenter;
        private Texture2D _pixel;
        private Vector2 _mouse;
        private EditBlueprintScrapShape _editBlueprintScrapShape;

        public List<Vector2> points { get { return _editBlueprintScrapShape.points; } }

        public EditBlueprintScrapShapeGraphics()
            : base()
        {
            System.Windows.Forms.Application.Idle += delegate { Invalidate(); };
            MouseMove += new System.Windows.Forms.MouseEventHandler(EditBlueprintScrapShapeGraphics_MouseMove);
            MouseDown += new System.Windows.Forms.MouseEventHandler(EditBlueprintScrapShapeGraphics_MouseDown);
        }

        // Click
        public void click()
        {
            // Add point
            points.Add(_mouse - _textureCenter);

            // Enable done button
            _editBlueprintScrapShape.enableSaveButton(points.Count > 2);
        }

        // Mouse down
        void EditBlueprintScrapShapeGraphics_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            click();
        }

        // Mouse move
        void EditBlueprintScrapShapeGraphics_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _mouse = new Vector2(e.Location.X, e.Location.Y);
        }

        // setTexture
        public void setTexture(string textureUID)
        {
            _textureUID = textureUID;
        }

        // Initialize
        protected override void Initialize()
        {
            _editBlueprintScrapShape = Parent as EditBlueprintScrapShape;
            System.Diagnostics.Debug.Assert(Parent is EditBlueprintScrapShape);
            System.Diagnostics.Debug.Assert(_editBlueprintScrapShape != null);

            System.Diagnostics.Debug.Assert(_textureUID != null);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });

            try
            {
                ResourceController.graphicsDevice = GraphicsDevice;
                _texture = ResourceController.getTexture(_textureUID);
                _textureCenter = new Vector2(_texture.Width, _texture.Height) / 2;
            }
            catch (ResourceNotFoundException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Resource Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                FindForm().Close();
                return;
            }

            // Resize form...
            int originalWidth = Width;
            int originalHeight = Height;
            Width = _texture.Width;
            Height = _texture.Height;
            int deltaWidth = Width - originalWidth;
            int deltaHeight = Height - originalHeight;
            Parent.Width += deltaWidth;
            Parent.Height += deltaHeight;
        }

        // Draw
        protected override void Draw()
        {
            System.Diagnostics.Debug.Assert(_texture != null);
            System.Diagnostics.Debug.Assert(_spriteBatch != null);
            System.Diagnostics.Debug.Assert(points != null);

            GraphicsDevice.Clear(Color.Black);

            if (_texture != null)
            {
                try
                {
                    _spriteBatch.Begin();

                    // Draw texture
                    _spriteBatch.Draw(_texture, _textureCenter, _texture.Bounds, Color.White, 0, _textureCenter, 1f, SpriteEffects.None, 0);

                    // Draw mouse position
                    _spriteBatch.Draw(_pixel, new Vector2(_mouse.X, _mouse.Y), new Rectangle(0, 0, 4, 4), Color.Yellow, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);

                    // Draw lines
                    for (int i = 0; i < points.Count; i++)
                    {
                        // Point
                        _spriteBatch.Draw(_pixel, points[i] + _textureCenter, new Rectangle(0, 0, 4, 4), Color.Orange, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);

                        // Lines
                        if (i > 0)
                            drawLine(points[i - 1] + _textureCenter, points[i] + _textureCenter, Color.Purple);
                    }

                    if (points.Count > 0)
                    {
                        // Draw line between last point and first point
                        drawLine(points[0] + _textureCenter, points[points.Count - 1] + _textureCenter, Color.DarkBlue);
                    }

                    _spriteBatch.End();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
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
