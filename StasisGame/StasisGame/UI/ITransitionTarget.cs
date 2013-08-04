using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.UI
{
    public interface ITransitionTarget
    {
        Screen screen { get; }
    }
}
