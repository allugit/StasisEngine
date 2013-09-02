using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Scripts
{
    public class HomeVillageScript : ScriptBase
    {
        private int _dagnyEntityId;
        private string _levelUid = "home_village";

        public HomeVillageScript(SystemManager systemManager, EntityManager entityManager)
            : base(systemManager, entityManager)
        {
        }

        private void createDagnyNpc()
        {
            _dagnyEntityId = _entityManager.factory.createDagny(_levelUid, new Vector2(-6.74f, -8.2f));
            _entityManager.addComponent(_levelUid, _dagnyEntityId, new AIWanderBehaviorComponent("dagny_idle", 4f, 30, 360));
        }

        public override void onLevelStart()
        {
            createDagnyNpc();
        }
    }
}
