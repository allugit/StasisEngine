using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class StonePane : Pane
    {
        public StonePane(Screen screen, UIAlignment alignment, int x, int y, int width, int height)
            : base(
                screen,
                ResourceManager.getTexture("stone_pane_top_left_corner"),
                ResourceManager.getTexture("stone_pane_top_right_corner"),
                ResourceManager.getTexture("stone_pane_bottom_right_corner"),
                ResourceManager.getTexture("stone_pane_bottom_left_corner"),
                ResourceManager.getTexture("stone_pane_left_side"),
                ResourceManager.getTexture("stone_pane_top_side"),
                ResourceManager.getTexture("stone_pane_right_side"),
                ResourceManager.getTexture("stone_pane_bottom_side"),
                ResourceManager.getTexture("stone_pane_background"),
                alignment,
                x,
                y,
                width,
                height)
        {
        }
    }
}
