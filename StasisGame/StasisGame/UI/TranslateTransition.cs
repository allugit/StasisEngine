using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class TranslateTransition : Transition
    {
        private ITranslatable _target;
        private int _fromX;
        private int _fromY;
        private int _toX;
        private int _toY;
        private float _dX;
        private float _dY;

        public TranslateTransition(ITranslatable obj, int fromX, int fromY, int toX, int toY, bool queue = true, float speed = 0.05f, Action onBegin = null, Action onEnd = null) :
            base(obj, queue, speed, onBegin, onEnd)
        {
            _target = obj;
            _fromX = fromX;
            _fromY = fromY;
            _toX = toX;
            _toY = toY;
            _dX = (float)(_toX - _fromX) * speed;
            _dY = (float)(_toY - _fromY) * speed;
        }

        public override void begin()
        {
            _target.translationX = _fromX;
            _target.translationY = _fromY;
            base.begin();
        }

        public override void end()
        {
            _target.translationX = _toX;
            _target.translationY = _toY;
            base.end();
        }

        public override void update()
        {
            _progress += _speed;
            _target.translationX += _dX;
            _target.translationY += _dY;
        }

        public override void draw()
        {
        }
    }
}
