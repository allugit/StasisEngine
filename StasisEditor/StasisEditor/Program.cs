using System;

namespace StasisEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            EditorForm form = new EditorForm();
            Main main = new Main(form);
            form.main = main;
            form.Show();
            using (main)
                main.Run();
        }
    }
#endif
}

