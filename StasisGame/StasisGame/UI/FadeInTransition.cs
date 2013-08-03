using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;
using StasisCore;

namespace StasisGame.UI
{
    public class FadeInTransition : Transition
    {
        private Color _color;
        private Texture2D _pixel;

        public FadeInTransition(Screen screen, Color color, bool queue = true, float speed = 0.05f, Action onBegin = null, Action onEnd = null)
            : base(screen, queue, speed, onBegin, onEnd)
        {
            _color = color;
            _screen = screen;
            _pixel = ResourceManager.getTexture("pixel");
        }

        public override void update()
        {
            _progress += _speed;
        }

        public override void draw()
        {
            _spriteBatch.Draw(_pixel, _spriteBatch.GraphicsDevice.Viewport.Bounds, _color * (1f - _progress));
        }
    }
}
