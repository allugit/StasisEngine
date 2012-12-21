using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using StasisEditor.Views;
using StasisEditor.Models;
using StasisCore.Resources;

namespace StasisEditor.Controllers
{
    public class TextureController : Controller
    {
        private EditorController _editorController;
        private TextureView _textureView;
        private List<EditorTexture> _textures;

        public TextureController(EditorController editorController, TextureView textureView)
        {
            _editorController = editorController;
            _textureView = textureView;
            _textures = new List<EditorTexture>();

            // Initialize texture view
            _textureView.setController(this);
        }

        // viewClosed
        public void viewClosed()
        {
            _textureView = null;
        }
    }
}
