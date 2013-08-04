using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class Overlay : IAlphaFadable
    {
        private SpriteBatch _spriteBatch;
        private Screen _screen;
        private Texture2D _pixel;
        private Color _color;
        private float _alpha = 1f;

        public float alpha { get { return _alpha; } set { _alpha = value; } }
        public Screen screen { get { return _screen; } }

        public Overlay(Screen screen, Color color)
        {
            _screen = screen;
            _spriteBatch = screen.screenSystem.spriteBatch;
            _color = color;
            _pixel = ResourceManager.getTexture("pixel");
        }

        public void draw()
        {
            _spriteBatch.Draw(_pixel, _spriteBatch.GraphicsDevice.Viewport.Bounds, _color * _alpha);
        }
    }
}
