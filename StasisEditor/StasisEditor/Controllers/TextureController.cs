using System;
using System.Collections.Generic;
using StasisEditor.Views;

namespace StasisEditor.Controllers
{
    public class TextureController : ITextureController
    {
        private IController _controller;
        private ITextureView _textureView;

        public TextureController(IController controller)
        {
            _controller = controller;
        }

        // openView
        public void openView()
        {
            if (_textureView == null)
            {
                _textureView = new TextureView();
                _textureView.setController(this);
                _textureView.Show();
            }
            else
            {
                _textureView.Focus();
            }
        }

        // viewClosed
        public void viewClosed()
        {
            _textureView = null;
        }
    }
}
