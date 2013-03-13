using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.UI
{
    public interface IUIComponent
    {
        float layerDepth { get; }
        void onSelect();
        void UIUpdate();
        void UIDraw();
        bool selectable { get; }
    }
}
