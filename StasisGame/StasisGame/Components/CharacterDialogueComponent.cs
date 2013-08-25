using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class CharacterDialogueComponent : IComponent
    {
        private string _dialogueUid;

        public ComponentType componentType { get { return ComponentType.CharacterDialogue; } }
        public string dialogueUid { get { return _dialogueUid; } }

        public CharacterDialogueComponent(string dialogueUid)
        {
            _dialogueUid = dialogueUid;
        }
    }
}
