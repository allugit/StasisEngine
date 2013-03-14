using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.UI
{
    public interface IUIComponent
    {
        float layerDepth { get; }
        bool hitTest(Vector2 point);
        void onSelect();
        void onDeselect();
        void UIUpdate();
        void UIDraw();
        bool selectable { get; }
    }
}
