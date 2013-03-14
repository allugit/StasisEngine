using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.UI
{
    public interface IUIComponent
    {
        void UIUpdate();
        void UIDraw();
        float layerDepth { get; }
        bool selectable { get; }
    }
}
