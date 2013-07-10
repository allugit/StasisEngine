using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;

namespace StasisGame.UI
{
    public class BluePane : Pane
    {
        public BluePane(SpriteBatch spriteBatch, UIAlignment alignment, int x, int y, int width, int height)
            : base(
                spriteBatch,
                ResourceManager.getTexture("blue_pane_top_left_corner"),
                ResourceManager.getTexture("blue_pane_top_right_corner"),
                ResourceManager.getTexture("blue_pane_bottom_right_corner"),
                ResourceManager.getTexture("blue_pane_bottom_left_corner"),
                ResourceManager.getTexture("blue_pane_left_side"),
                ResourceManager.getTexture("blue_pane_top_side"),
                ResourceManager.getTexture("blue_pane_right_side"),
                ResourceManager.getTexture("blue_pane_bottom_side"),
                ResourceManager.getTexture("blue_pane_background"),
                alignment,
                x,
                y,
                width,
                height)
        {
        }
    }
}
