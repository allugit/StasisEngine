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
            using (Main main = new Main())
                main.Run();
        }
    }
#endif
}

