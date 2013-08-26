using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Components;
using StasisGame.Managers;
using StasisGame.UI;

namespace StasisGame.Systems
{
    public class DialogueSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;
        private int _currentEntityA;
        private int _currentEntityB;
        private CharacterDialogueComponent _currentDialogueComponent;

        public SystemType systemType { get { return SystemType.Dialogue; } }
        public int defaultPriority { get { return 40; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public DialogueSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void beginDialogue(string levelUid, int entityA, int entityB, CharacterDialogueComponent dialogueComponent)
        {
            ScreenSystem screenSystem = _systemManager.getSystem(SystemType.Screen) as ScreenSystem;
            LevelScreen levelScreen = screenSystem.getScreen(ScreenType.Level) as LevelScreen;

            _currentEntityA = entityA;
            _currentEntityB = entityB;
            _currentDialogueComponent = dialogueComponent;
            _entityManager.addComponent(levelUid, entityA, new InDialogueComponent());
            _entityManager.addComponent(levelUid, entityB, new InDialogueComponent());
            levelScreen.addDialoguePane(dialogueComponent);
        }

        public void endDialogue(string levelUid)
        {
            ScreenSystem screenSystem = _systemManager.getSystem(SystemType.Screen) as ScreenSystem;
            LevelScreen levelScreen = screenSystem.getScreen(ScreenType.Level) as LevelScreen;

            _entityManager.removeComponent(levelUid, _currentEntityA, ComponentType.InDialogue);
            _entityManager.removeComponent(levelUid, _currentEntityB, ComponentType.InDialogue);
            levelScreen.removeDialoguePane(_currentDialogueComponent);
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
