using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.UI
{
    public interface ITranslatable : ITransition
    {
        float translationX { get; set; }
        float translationY { get; set; }
    }
}
