using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public class LevelView : GraphicsDeviceControl
    {
        private LevelController _controller;
        private Texture2D _playerSpawnIcon;
        private Texture2D _timerIcon;

        // setController
        public void setController(LevelController controller)
        {
            _controller = controller;
        }

        // Initialize
        protected override void Initialize()
        {
            
        }

        // Draw
        protected override void Draw()
        {
            
        }
    }
}
