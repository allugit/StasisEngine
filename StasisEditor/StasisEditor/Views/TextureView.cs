using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;

namespace StasisEditor.Views
{
    public partial class TextureView : Form, ITextureView
    {
        private ITextureController _controller;

        public TextureView()
        {
            InitializeComponent();
        }

        // setController
        public void setController(ITextureController controller)
        {
            _controller = controller;
        }

        // Form closed
        private void TextureView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.viewClosed();
        }
    }
}
