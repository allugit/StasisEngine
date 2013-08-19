using System;
using System.Collections.Generic;
using StasisGame.Managers;

namespace StasisGame.Scripts
{
    public class HomeVillageScript : ScriptBase
    {
        public HomeVillageScript(SystemManager systemManager, EntityManager entityManager)
            : base(systemManager, entityManager)
        {
        }

        public override void onLevelStart()
        {
            Console.WriteLine("Home village script started.");
        }
    }
}
