using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Managers;

namespace StasisGame.Scripts
{
    public class DagnyHouseScript : ScriptBase
    {
        public DagnyHouseScript(SystemManager systemManager, EntityManager entityManager)
            : base(systemManager, entityManager)
        {
        }

        public override void onLevelStart()
        {
            Console.WriteLine("Creating dagny...");
            _entityManager.factory.createDagny("dagny_house", new Vector2(-6.74f, -8.2f));
        }
    }
}
