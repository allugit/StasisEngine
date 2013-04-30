using System;
using System.Collections.Generic;
using StasisGame.Managers;
using StasisGame.Systems;

namespace StasisGame
{
    public class ScriptBase
    {
        protected SystemManager _systemManager;
        protected EntityManager _entityManager;

        public ScriptBase(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        virtual public bool doAction(string action)
        {
            return false;
        }

        virtual public void onLevelStart()
        {
        }

        virtual public void onLevelEnd()
        {
        }

        virtual public void onReturnToWorldMap(LevelGoalSystem levelGoalSystem)
        {
        }
    }
}
