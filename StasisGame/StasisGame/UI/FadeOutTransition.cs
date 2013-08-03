using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;
using StasisCore;

namespace StasisGame.UI
{
    public class FadeOutTransition : Transition
    {
        private Color _color;
        private Texture2D _pixel;

        public FadeOutTransition(Screen screen, Color color, bool queue = true, float speed = 0.05f) : base(screen, queue, speed)
        {
            _color = color;
            _screen = screen;
            _pixel = ResourceManager.getTexture("pixel");
        }

        public override void begin()
        {
        }

        public override void end()
        {
            _screen.screenSystem.removeScreen(_screen);
        }

        public override void update()
        {
            _progress += _speed;
        }

        public override void draw()
        {
            _spriteBatch.Draw(_pixel, _spriteBatch.GraphicsDevice.Viewport.Bounds, _color * _progress);
        }
    }
}
