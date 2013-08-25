using System;
using System.Collections.Generic;

namespace StasisGame.Components
{
    public class InDialogueComponent : IComponent
    {
        public ComponentType componentType { get { return ComponentType.InDialogue; } }

        public InDialogueComponent()
        {
        }
    }
}
