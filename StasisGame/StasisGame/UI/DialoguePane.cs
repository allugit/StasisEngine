using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.UI
{
    public class DialoguePane : BluePane
    {
        private DialogueManager _dialogueManager;
        private SpriteFont _font;
        private CharacterDialogueComponent _dialogueComponent;
        private float _margin = 16f;

        public CharacterDialogueComponent dialogueComponent { get { return _dialogueComponent; } }

        public DialoguePane(Screen screen, UIAlignment alignment, int x, int y, int width, int height, SpriteFont font, CharacterDialogueComponent dialogueComponent)
            : base(screen, alignment, x, y, width, height)
        {
            _dialogueComponent = dialogueComponent;
            _font = font;
            _dialogueManager = DataManager.dialogueManager;
        }

        public override void draw()
        {
            string message = Screen.wrapText(_font, _dialogueManager.getText(_dialogueComponent.dialogueUid), (float)_width - (_margin * 2f));

            base.draw();
            _spriteBatch.DrawString(_font, message, new Vector2(x + _margin, y + _margin), Color.White);
        }
    }
}
