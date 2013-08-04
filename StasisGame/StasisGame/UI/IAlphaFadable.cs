using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisGame.UI
{
    public interface IAlphaFadable : ITransitionTarget
    {
        float alpha { get; set; }
    }
}
