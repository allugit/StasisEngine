using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
