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
        private Screen _screen;
        private Texture2D _pixel;

        public FadeOutTransition(ScreenSystem screenSystem, SpriteBatch spriteBatch, Screen screen, Color color, bool queue = true, float speed = 0.05f) : base(screenSystem, spriteBatch, queue, speed)
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
            _screenSystem.removeScreen(_screen);
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
