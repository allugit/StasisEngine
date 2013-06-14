using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    using Keys = System.Windows.Forms.Keys;

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
            if (!IsDesignerHosted)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _backgroundRenderer = new BackgroundRenderer(_spriteBatch);

                System.Windows.Forms.Application.Idle += delegate { Invalidate(); };
                MouseMove += new System.Windows.Forms.MouseEventHandler(BackgroundDisplay_MouseMove);
                FindForm().KeyDown += new System.Windows.Forms.KeyEventHandler(Parent_KeyDown);
                FindForm().KeyUp += new System.Windows.Forms.KeyEventHandler(Parent_KeyUp);
            }
        }

        void BackgroundDisplay_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Update mouse position
            _view.controller.mouse = e.Location;
            Vector2 delta = _view.controller.mouseDelta;

            if (_view.controller.ctrl)
            {
                // Move screen
                _view.controller.screenCenter += delta;
            }
        }

        // Key up
        void Parent_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (_view.keysEnabled)
            {
                if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _view.controller.ctrl = false;
            }
        }

        // Key down
        void Parent_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (_view.keysEnabled)
            {
                if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    _view.controller.ctrl = true;
            }
        }

        public void previewBackground(EditorBackground background)
        {
            _backgroundRenderer.background = background;
        }

        protected override void Draw()
        {
            if (!IsDesignerHosted)
            {
                if (_backgroundRenderer.background != null)
                    _backgroundRenderer.update(_view.controller.editorController.scale, _view.controller.screenCenter);

                GraphicsDevice.Clear(Color.Black);

                if (_backgroundRenderer.background != null)
                {
                    _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    _backgroundRenderer.drawFirstHalf();
                    _spriteBatch.End();

                    Vector2 previousScreenCenter = _view.controller.editorController.levelController.screenCenter;
                    _view.controller.editorController.levelController.screenCenter = _backgroundRenderer.screenOffset;
                    _view.controller.editorController.levelController.view.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    if (_view.controller.editorController.levelController.level != null)
                    {
                        foreach (List<EditorActor> actors in _view.controller.editorController.levelController.level.sortedActors.Values)
                        {
                            foreach (EditorActor actor in actors)
                            {
                                actor.draw();
                            }
                        }
                    }
                    _view.controller.editorController.levelController.view.spriteBatch.End();
                    _view.controller.editorController.levelController.screenCenter = previousScreenCenter;

                    _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    _backgroundRenderer.drawSecondHalf();
                    _spriteBatch.End();
                }
            }
        }
    }
}
