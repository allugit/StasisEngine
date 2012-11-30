using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;

namespace StasisEditor.Models
{
    public class Brush
    {
        private EditorController _editor;

        public Brush(EditorController editor)
        {
            _editor = editor;
        }
    }
}
