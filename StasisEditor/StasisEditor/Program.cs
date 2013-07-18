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
            // Create logger
            StasisCore.Logger.initialize();
            StasisCore.Logger.log(string.Format("StasisEditor started."));

            _editorController = new EditorController(new EditorView());
            Application.Run(_editorController.view);
        }
    }
}

