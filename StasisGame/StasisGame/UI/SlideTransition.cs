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
        private int _dX;
        private int _dY;

        public SlideTransition(Screen screen, int fromX, int fromY, int toX, int toY, bool queue = true, float speed = 0.05f) :
            base(screen, queue, speed)
        {
        }

        public override void begin()
        {
        }

        public override void end()
        {
        }

        public override void update()
        {
        }

        public override void draw()
        {
        }
    }
}
