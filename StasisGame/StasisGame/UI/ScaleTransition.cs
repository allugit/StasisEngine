using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.UI
{
    public class ScaleTransition : Transition
    {
        private IScalable _target;
        private float _from;
        private float _to;
        private float _d;

        public ScaleTransition(IScalable obj, float from, float to, bool queue = true, float speed = 0.05f, Action onBegin = null, Action onEnd = null)
            : base(obj, queue, speed, onBegin, onEnd)
        {
            _target = obj;
            _from = from;
            _to = to;
            _d = (_to - _from) * speed;
        }

        public override void begin()
        {
            _target.scale = _from;
            base.begin();
        }

        public override void end()
        {
            _target.scale = _to;
            base.end();
        }

        public override void update()
        {
            _progress += _speed;
            _target.scale += _d;
        }

        public override void draw()
        {
        }
    }
}
