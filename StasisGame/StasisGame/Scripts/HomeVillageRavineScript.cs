using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StasisGame.Components;
using StasisGame.Managers;
using StasisGame.Systems;

namespace StasisGame.Scripts
{
    public class HomeVillageRavineScript : ScriptBase
    {
        private int _dagnyEntityId;
        private string _levelUid = "home_village_ravine";

        public HomeVillageRavineScript(SystemManager systemManager, EntityManager entityManager)
            : base(systemManager, entityManager)
        {
        }

        public override void onSwitchLevel(string from, string to)
        {
            if (from == "home_village")
            {
                CharacterMovementSystem characterMovementSystem = _systemManager.getSystem(SystemType.CharacterMovement) as CharacterMovementSystem;

                characterMovementSystem.attemptRopeGrab(to, PlayerSystem.PLAYER_ID);
            }
        }
    }
}
