using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Models
{
    public interface IWorldControl
    {
        Vector2 position { get; set; }
        void delete();
    }
}
