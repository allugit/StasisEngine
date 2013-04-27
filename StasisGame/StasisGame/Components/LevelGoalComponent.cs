using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class LevelGoalComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.LevelGoal; } }

        public LevelGoalComponent()
        {
        }
    }
}
