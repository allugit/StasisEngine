using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.UI
{
    public interface ITranslatable : ITransitionTarget
    {
        float translationX { get; set; }
        float translationY { get; set; }
    }
}
