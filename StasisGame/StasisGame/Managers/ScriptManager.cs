using System;
using System.IO;
using System.Collections.Generic;
using StasisGame.Scripts;
using StasisGame.Systems;

namespace StasisGame.Managers
{
    public class ScriptManager
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private Dictionary<string, ScriptBase> _scripts;

        public ScriptManager(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _scripts = new Dictionary<string, ScriptBase>();
        }

        // addScript -- Adds a script, using its level uid as the key
        public void addScript(string levelUid, ScriptBase script)
        {
            _scripts.Add(levelUid, script);
        }

        // doAction -- Execute an action that isn't hooked to an event. Level scripts take precedence over global scripts.
        public void doAction(string key, string action)
        {
            bool tryGlobal = true;
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
            {
                tryGlobal = script.doAction(action);
                tryGlobal = key == "global" ? false : tryGlobal;    // prevent trying 'global' twice in a row
            }

            if (tryGlobal)
                _scripts["global"].doAction(action);
        }

        // registerGoals -- Hook for registering goals for a specific level
        public void registerGoals(string key, LevelSystem levelSystem)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.registerGoals(levelSystem);
        }

        // onLevelStart -- Hook for the start of the level
        public void onLevelStart(string key)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.onLevelStart();
        }

        // onLevelEnd -- Hook for the end of a level
        public void onLevelEnd(string key)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.onLevelEnd();
        }

        // onReturnToWorldMap -- Hook called when returning to the world map after a level ends
        public void onReturnToWorldMap(string key, LevelSystem levelSystem)
        {
            ScriptBase script = null;

            if (_scripts.TryGetValue(key, out script))
                script.onReturnToWorldMap(levelSystem);
        }
    }
}
