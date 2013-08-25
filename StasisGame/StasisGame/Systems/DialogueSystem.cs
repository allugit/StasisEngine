using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class DialogueSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;

        public SystemType systemType { get { return SystemType.Dialogue; } }
        public int defaultPriority { get { return 40; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public DialogueSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update(GameTime gameTime)
        {
            LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;
            string levelUid = LevelSystem.currentLevelUid;

            if (levelSystem.finalized)
            {
                if (!_paused || _singleStep)
                {
                }
                _singleStep = false;
            }
        }
    }
}
