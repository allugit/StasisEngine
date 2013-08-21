using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Scripts
{
    public class DagnyHouseScript : ScriptBase
    {
        private string _levelUid = "dagny_house";
        private int _dagnyEntityId;

        public DagnyHouseScript(SystemManager systemManager, EntityManager entityManager)
            : base(systemManager, entityManager)
        {
        }

        public override void onLevelStart()
        {
            _dagnyEntityId = _entityManager.factory.createDagny(_levelUid, new Vector2(-6.74f, -8.2f));
            _entityManager.addComponent(_levelUid, _dagnyEntityId, new AIWanderBehaviorComponent("dagny_idle", 0.05f, 0.35f, 30, 360));
        }
    }
}
