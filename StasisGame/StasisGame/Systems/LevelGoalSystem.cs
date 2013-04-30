using System;
using System.Collections.Generic;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class LevelGoalSystem : ISystem
    {
        private LoderGame _game;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;
        private List<LevelGoalComponent> _registeredGoals;
        private List<LevelGoalComponent> _completedGoals;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.LevelGoal; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public EntityManager entityManager { get { return _entityManager; } }

        public LevelGoalSystem(LoderGame game, SystemManager systemManager, EntityManager entityManager)
        {
            _game = game;
            _systemManager = systemManager;
            _entityManager = entityManager;
            _registeredGoals = new List<LevelGoalComponent>();
            _completedGoals = new List<LevelGoalComponent>();
        }

        public void registerGoal(LevelGoalComponent goal)
        {
            _registeredGoals.Add(goal);
        }

        public void completeGoal(LevelGoalComponent goal)
        {
            if (!_completedGoals.Contains(goal))
            {
                Console.WriteLine("Completed a goal: {0}", goal);
                _completedGoals.Add(goal);
            }
        }

        public void update()
        {
            // Quick way to check for all goals being completed:
            if (_completedGoals.Count >= _registeredGoals.Count)
            {
                Console.WriteLine("All goals complete!");
                _game.exitLevel();
                _game.openWorldMap();
            }
        }
    }
}
