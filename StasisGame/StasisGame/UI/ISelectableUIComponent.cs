using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.UI
{
    public interface ISelectableUIComponent : IUIComponent
    {
        bool hitTest(Vector2 point);
        void onSelect();
        void onDeselect();
        void activate();
    }
}
