using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.UI
{
    public class LevelScreen : Screen
    {
        private LoderGame _game;
        private Level _level;
        private Texture2D _pixel;

        public LevelScreen(LoderGame game, Level level)
            : base(ScreenType.Level)
        {
            _game = game;
            _level = level;
            _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });

            _UIComponents.Add(new LargeHealthBar(_game.spriteBatch));
        }

        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            base.draw();
        }
    }
}
