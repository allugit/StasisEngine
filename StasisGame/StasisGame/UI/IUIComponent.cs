using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.UI
{
    public interface IUIComponent
    {
        bool hitTest(Vector2 point);
        void onSelect();
        void onDeselect();
        void UIUpdate();
        void UIDraw();
        void activate();
        float layerDepth { get; }
        bool selectable { get; }
    }
}
