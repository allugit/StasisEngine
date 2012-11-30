using System;
using StasisEditor.Controller;

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
            using (XNAController xnaController = new XNAController())
            {
                xnaController.Run();
            }
        }
    }
#endif
}

