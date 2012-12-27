using System;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Views;

namespace StasisEditor
{
    static class Program
    {
        private static EditorController _editorController;

        [STAThread]
        static void Main(string[] args)
        {
            _editorController = new EditorController(new EditorView());
            Application.Run(_editorController.view);
        }
    }
}

