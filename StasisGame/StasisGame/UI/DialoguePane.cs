using System;
using System.Collections.Generic;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.UI
{
    public class DialoguePane : BluePane
    {
        private DialogueManager _dialogueManager;
        private CharacterDialogueComponent _dialogueComponent;

        public CharacterDialogueComponent dialogueComponent { get { return _dialogueComponent; } }

        public DialoguePane(DialogueManager dialogueManager, Screen screen, UIAlignment alignment, int x, int y, int width, int height, CharacterDialogueComponent dialogueComponent)
            : base(screen, alignment, x, y, width, height)
        {
            _dialogueManager = dialogueManager;
            _dialogueComponent = dialogueComponent;
        }
    }
}
