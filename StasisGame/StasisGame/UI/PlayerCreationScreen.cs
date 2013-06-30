using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class PlayerCreationScreen : Screen
    {
        private LoderGame _game;

        public PlayerCreationScreen(LoderGame game) : base(ScreenType.PlayerCreation)
        {
            _game = game;
        }
    }
}
