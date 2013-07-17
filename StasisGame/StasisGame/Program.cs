using System;
using System.Reflection;
using System.Diagnostics;

namespace StasisGame
{
    static class Program
    {
        public static string version;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Get version
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            version = string.Format("Version {0}.{1}", fvi.FileMajorPart, fvi.FileMinorPart);

            // Create logger
            StasisCore.Logger.initialize();
            StasisCore.Logger.log(string.Format("Loder's Fall ({0}) started.", version));

            // Run game
            using (LoderGame game = new LoderGame(args))
            {
                game.Run();
            }
        }
    }
}

