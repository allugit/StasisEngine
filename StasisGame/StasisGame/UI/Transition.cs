using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    abstract public class Transition
    {
        protected ScreenSystem _screenSystem;
        protected SpriteBatch _spriteBatch;
        protected float _progress = 0f;
        protected float _speed = 0.05f;

        public float progress { get { return _progress; } }

        public bool finished { get { return _progress >= 1f; } }
        public bool starting { get { return _progress == 0f; } }

        public Transition(ScreenSystem screenSystem, SpriteBatch spriteBatch)
        {
            _screenSystem = screenSystem;
            _spriteBatch = spriteBatch;
        }

        abstract public void begin();
        abstract public void end();

        abstract public void update();
        abstract public void draw();
    }
}
