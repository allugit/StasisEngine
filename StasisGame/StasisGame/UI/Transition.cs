using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    abstract public class Transition
    {
        protected ITransition _obj;
        protected Screen _screen;
        protected SpriteBatch _spriteBatch;
        protected float _progress;
        protected float _speed;
        protected bool _queue;
        private Action _onBegin;
        private Action _onEnd;

        public float progress { get { return _progress; } }

        public bool finished { get { return _progress >= 1f; } }
        public bool starting { get { return _progress == 0f; } }
        public bool queued { get { return _queue; } }

        public Transition(ITransition obj, bool queue, float speed, Action onBegin, Action onEnd)
        {
            _obj = obj;
            _screen = _obj.screen;
            _spriteBatch = _screen.screenSystem.spriteBatch;
            _queue = queue;
            _speed = speed;
            _onBegin = onBegin;
            _onEnd = onEnd;
        }

        virtual public void begin()
        {
            if (_onBegin != null)
                _onBegin();
        }

        virtual public void end()
        {
            if (_onEnd != null)
                _onEnd();
        }

        abstract public void update();
        abstract public void draw();
    }
}
