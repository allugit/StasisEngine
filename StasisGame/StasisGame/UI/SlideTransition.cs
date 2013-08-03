using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class SlideTransition : Transition
    {
        private int _fromX;
        private int _fromY;
        private int _toX;
        private int _toY;
        private float _dX;
        private float _dY;

        public SlideTransition(Screen screen, int fromX, int fromY, int toX, int toY, bool queue = false, float speed = 0.05f) :
            base(screen, queue, speed)
        {
            _fromX = fromX;
            _fromY = fromY;
            _toX = toX;
            _toY = toY;
            _dX = (float)(_toX - _fromX) * speed;
            _dY = (float)(_toY - _fromY) * speed;
        }

        public override void begin()
        {
            _screen.slideX = _fromX;
            _screen.slideY = _fromY;
        }

        public override void end()
        {
            _screen.slideX = 0;
            _screen.slideY = 0;
        }

        public override void update()
        {
            _progress += _speed;
            _screen.slideX += _dX;
            _screen.slideY += _dY;
        }

        public override void draw()
        {
        }
    }
}
