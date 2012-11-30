using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Views;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    public class LevelController
    {
        private Level _level;

        // Constructor
        public LevelController(Level level)
        {
            _level = level;
        }
    }
}
