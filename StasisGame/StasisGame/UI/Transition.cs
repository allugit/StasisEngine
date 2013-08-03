using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    abstract public class Transition
    {
        protected Screen _screen;
        protected SpriteBatch _spriteBatch;
        protected float _progress;
        protected float _speed;
        protected bool _queue;

        public float progress { get { return _progress; } }

        public bool finished { get { return _progress >= 1f; } }
        public bool starting { get { return _progress == 0f; } }
        public bool queued { get { return _queue; } }

        public Transition(Screen screen, bool queue, float speed)
        {
            _screen = screen;
            _spriteBatch = screen.screenSystem.spriteBatch;
            _queue = queue;
            _speed = speed;
        }

        abstract public void begin();
        abstract public void end();

        abstract public void update();
        abstract public void draw();
    }
}
