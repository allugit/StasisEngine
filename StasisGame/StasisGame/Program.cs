using System;

namespace StasisGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LoderGame game = new LoderGame(args))
            {
                game.Run();
            }
        }
    }
}

