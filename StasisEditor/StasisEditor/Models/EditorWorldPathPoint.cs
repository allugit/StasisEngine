using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorWorldPathPoint : WorldPathPoint, IWorldControl
    {
        private EditorWorldPath _path;

        public EditorWorldPathPoint(EditorWorldPath path, Vector2 position)
            : base(position)
        {
            _path = path;
        }

        public void delete()
        {
            _path.delete();
        }
    }
}
